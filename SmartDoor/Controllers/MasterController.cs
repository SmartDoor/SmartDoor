using SmartDoor.Controllers;
using System;
using System.Timers;
using SmartDoor.Utilities;

namespace SmartDoor
{
    /// <summary>
    /// The Master Controller of the program. Will handle communication between
    ///  our different components.
    /// </summary>
    class MasterController : IObserver<Package>, IDisposable
    {
        private static MasterController instance;

        private MasterController()
        {
            rfidHandler = new RFIDHandler();
            motorHandler = new MotorHandler(1);
            interfaceHandler = new InterfaceHandler();

            lockTimer = new Timer(1000 * 5);
            lockTimer.AutoReset = true;
            lockTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        }

        public static MasterController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MasterController();
                }
                return instance;
            }
        }


        public RFIDHandler rfidHandler { get; }
        public MotorHandler motorHandler { get; }
        public InterfaceHandler interfaceHandler { get; }
        public LCDHandler lcdHandler { get; }

        public SecurityController secController;

        private Timer lockTimer;

        /// <summary>
        /// 
        /// </summary>
        public void Setup()
        {
            secController = SecurityController.Instance;

            rfidHandler.WaitForAttach();
            motorHandler.WaitForAttach();
            interfaceHandler.WaitForAttach();

            rfidHandler.Subscribe(this);
            motorHandler.Subscribe(this);

            rfidHandler.Subscribe(DisplayController.Instance);
            motorHandler.Subscribe(DisplayController.Instance);

            motorHandler.Subscribe(AdminController.Instance);
            rfidHandler.Subscribe(AdminController.Instance);
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

                    if (secController.isSecureRFIDTag(value.message))
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
