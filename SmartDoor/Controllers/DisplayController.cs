using System;
using System.Diagnostics;
using System.Timers;

namespace SmartDoor.Controllers
{
    class DisplayController : IObserver<Package>, IDisposable
    {
        public LCDHandler lcdHandler { get; }

        private Timer updateLCDTimer;

        private SecurityController secController;
        private Stopwatch lastChange;

        public DisplayController(SecurityController secController)
        {
            this.secController = secController;
            lcdHandler = new LCDHandler();
            lastChange = new Stopwatch();

            updateLCDTimer = new Timer(1000);
            updateLCDTimer.AutoReset = true;
            updateLCDTimer.Elapsed += new ElapsedEventHandler(OnUpdateLCDEvent);

        }


        /// <summary>
        /// 
        /// </summary>
        public void Setup()
        {
            lcdHandler.WaitForAttach();

            lcdHandler.displayStatus(true);
            lcdHandler.setLockedIcon(true);

            updateLCDTimer.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Shutdown()
        {
            lcdHandler.Shutdown();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void OnNext(Package value)
        {
            switch (value.type)
            {
                case packageType.motorPackageLocked:
                    lcdHandler.setLockedIcon(true);
                    break;
                case packageType.motorPackageUnlocked:
                    lcdHandler.setLockedIcon(false);
                    break;
                case packageType.RfidPackageFound:
                    if (secController.isSecureRFIDTag(value.message))
                    {
                        lcdHandler.showMessage("Welcome home", secController.retrieveTag(value.message).name);
                    }
                    else
                    {
                        lcdHandler.showMessage("Greetings ", "Unknown person");
                    }
                    break;
                case packageType.RfidPackageLost:
                   
                    break;
                default:
                    throw new Exception("Unknown Package");
            }

            if (!lastChange.IsRunning)
                lastChange.Start();
            else
                lastChange.Restart();
        }

        private void OnUpdateLCDEvent(object sender, ElapsedEventArgs e)
        {
            if(lastChange.IsRunning && lastChange.Elapsed.Seconds > 5)
            {
                lcdHandler.showMessage("", "");
                lastChange.Stop();
            }

            lcdHandler.updateTime();
        }

        /// <summary>
        /// Handle errors from observable objects this controller has subscribed to.
        /// </summary>
        /// <param name="error"></param>
        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            updateLCDTimer.Dispose();
        }
    }  
}
