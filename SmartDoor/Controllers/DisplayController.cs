using SmartDoor.ComponentHandlers;
using SmartDoor.Templates;
using System;
using System.Collections;
using System.Diagnostics;
using System.Timers;

namespace SmartDoor.Controllers
{
    /// <summary>
    /// Singleton observer controller class to handle
    /// events regarding the LCD display.
    /// </summary>
    class DisplayController : IObserver<Package>, IDisposable
    {
        private static DisplayController instance;
        public LCDHandler lcdHandler { get; }

        private TemperatureHandler tempComponent;
        private Timer updateLCDTimer;
        private Stopwatch lastChange;

        private IList screenMessages;

        /// <summary>
        /// Constructs a new Display controller class.
        /// </summary>
        private DisplayController() {
            lcdHandler = new LCDHandler();
            lastChange = new Stopwatch();
            tempComponent = new TemperatureHandler();

            updateLCDTimer = new Timer(1000);
            updateLCDTimer.AutoReset = true;
            updateLCDTimer.Elapsed += new ElapsedEventHandler(OnUpdateLCDEvent);
        }

        public static DisplayController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DisplayController();
                }
                return instance;
            }
        }


        /// <summary>
        /// Setups the needed data and waits for attachments on
        /// the componenets needed by this object.
        /// </summary>
        public void Setup()
        {
            lcdHandler.WaitForAttach();
            tempComponent.WaitForAttach();

            // Turns on the backlight on the screen
            lcdHandler.displayStatus(true);

            // Sets the locking icon as the initial icon showed on the screen.
            lcdHandler.setLockedIcon(true);

            // Starts the update timer.
            updateLCDTimer.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Shutdown()
        {
            this.Dispose();
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
                //Door is locked, sets the locking icon
                case packageType.motorPackageLocked:
                    lcdHandler.setLockedIcon(true);
                    break;
                
                // Sets the unlocking icon.
                case packageType.motorPackageUnlocked:
                    lcdHandler.setLockedIcon(false);
                    break;

                // RFID Tag found
                case packageType.RfidPackageFound:
                    if (SecurityController.Instance.isSecureRFIDTag(value.message))
                    {
                        lcdHandler.showMessage("Welcome home", SecurityController.Instance.retrieveTag(value.message).name);

                        // Replace the message list with the one from the person
                        // who is opening the door
                        screenMessages = SecurityController.Instance.retrieveTag(value.message).getMessage();
                    }
                    else
                    {
                        lcdHandler.showMessage("Greetings ", "Stranger");
                        screenMessages = null;
                    }
                    break;
                case packageType.RfidPackageLost:
                    // No implementation needed
                    break;
                default:
                    throw new Exception("Unknown Package");
            }

            if (!lastChange.IsRunning)
                lastChange.Start();
            else
                lastChange.Restart();
        }

        /// <summary>
        /// Updates the LCD screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUpdateLCDEvent(object sender, ElapsedEventArgs e)
        {
            if(lastChange.IsRunning && lastChange.Elapsed.Seconds > 3)
            {
                if(screenMessages != null && screenMessages.Count > 0)
                {
                    lcdHandler.displayStatus(true);
                    lcdHandler.showMessage(tempComponent.getTempString());
                } else
                {
                    lcdHandler.showMessage("", "");
                    lcdHandler.displayStatus(false);
                    lastChange.Stop();
                }
            }

            //Update time
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

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            updateLCDTimer.Dispose();
        }
    }  
}
