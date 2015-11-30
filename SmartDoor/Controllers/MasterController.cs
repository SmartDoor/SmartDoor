using SmartDoor.ComponentHandlers;
using System;
using System.Timers;
using SmartDoor.Controllers;
using SmartDoor.Utilities;

namespace SmartDoor
{
    /// <summary>
    /// The Master Controller of the program. Will handle communication between our different devices.
    /// </summary>
    class MasterController : IObserver<Package>, IDisposable
    {
        public RFIDHandler rfidHandler { get; }
        public MotorHandler motorHandler { get; }
        public InterfaceHandler interfaceHandler { get; } 
        private Timer lockTimer;
        private SecurityController secController;

        public MasterController(SecurityController secController)
        {
            this.secController = secController;

            rfidHandler = new RFIDHandler();
            motorHandler = new MotorHandler(1);
            interfaceHandler = new InterfaceHandler();

            lockTimer = new Timer(1000 * 5);
            lockTimer.AutoReset = true;
            lockTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Setup()
        {
            rfidHandler.WaitForAttach();
            motorHandler.WaitForAttach();
            interfaceHandler.WaitForAttach();

            rfidHandler.Subscribe(this);
            motorHandler.Subscribe(this);
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void Shutdown()
        {
            rfidHandler.Shutdown();
            motorHandler.Shutdown();
            interfaceHandler.Shutdown();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void OnNext(Package value)
        {
           switch(value.type)
            {
                case packageType.motorPackageLocked:
                    Logger.DebugLog("MotorHandler : Door " + value.message);

                    lockTimer.Stop();

                    interfaceHandler.GreenLED(false);
                    interfaceHandler.RedLED(true);
                    break;

                case packageType.motorPackageUnlocked:
                    Logger.DebugLog("MotorHandler : Door " + value.message);

                    interfaceHandler.GreenLED(true);
                    interfaceHandler.RedLED(false);
                    break;

                case packageType.RfidPackageFound:
                    Logger.DebugLog("RFIDHandler : " + (secController.isSecureRFIDTag(value.message) ? "Secure" : "Unknown") + " Tag found [" +value.message +"]");

                    if(secController.isSecureRFIDTag(value.message))
                    {
                        Logger.Log("[" + value.message + "]" + " - " + secController.retrieveTag(value.message).name); 
                        motorHandler.UnlockDoor();
                    } else
                    {
                        Logger.Log("[" + value.message + "]" + " - " + "Unknown");
                    }
                    break;

                case packageType.RfidPackageLost:
                    Logger.DebugLog("RFIDHandler : " + (secController.isSecureRFIDTag(value.message) ? "Secure" : "Unknown") + " Tag lost [" + value.message + "]");
                    if (secController.isSecureRFIDTag(value.message))
                    {
                        lockTimer.Start();
                    }
                    break;

                default:
                    throw new Exception("Unknown Package");                   
            }
        }

        /// <summary>
        /// Stops the timer for the event and locks the door.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            lockTimer.Stop();
            motorHandler.LockDoor();
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
            lockTimer.Dispose();
        }
    }
}
