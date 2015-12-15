using SmartDoor.Controllers;
using System;
using System.Timers;
using SmartDoor.Utilities;
using System.Runtime.CompilerServices;
using SmartDoor.Templates;

namespace SmartDoor
{
    /// <summary>
    /// MotorController class, responsible for handling the locking/unlocking of
    /// the door and sending commands to the motor controller.
    /// 
    /// 
    /// Implemented by a singleton pattern to ensure we only can have one
    /// motor controller present at runtime.
    /// 
    /// The unlocking is done using the Timer that will automaticly lock the door 
    /// after a certain amount of time.
    /// </summary>
    class MotorController : IObserver<Package>, IDisposable, Controller
    {
        private static MotorController instance;

        /// <summary>
        /// Constructs a new MotorController object, will setup a 
        /// locking reset timer and a ComponentHandler for the electric motor
        /// that is in charge of locking door after 5 seconds is has been unlocked.
        /// </summary>
        private MotorController()
        {
            motorHandler = new MotorHandler(1);
            lockTimer = new Timer(1000 * 5);
            lockTimer.AutoReset = true;
            lockTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        }

        public static MotorController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MotorController();
                }
                return instance;
            }
        }

        private MotorHandler motorHandler;
        private Timer lockTimer;

        /// <summary>
        /// wait for motor attachment and set subscribers.
        /// </summary>
        public void Setup()
        {
            motorHandler.WaitForAttach();
            motorHandler.Subscribe(this);
            motorHandler.Subscribe(DisplayController.Instance);
            motorHandler.Subscribe(AdminController.Instance);
            motorHandler.Subscribe(InterfaceController.Instance);
        }
        
        /// <summary>
        /// Handles the shutdown of the components that the mastercontroller 
        /// is responsible for.
        /// </summary>
        public void Shutdown()
        {
            motorHandler.Shutdown();
        }

        /// <summary>
        /// This method handles incomming packages.
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void OnNext(Package value)
        {
           switch(value.type)
            {
                // Door locked
                case packageType.motorPackageLocked:
                    Logger.DebugLog("MotorHandler : Door " + value.message);
                    lockTimer.Stop();
                    break;
                
                // Door unlocked
                case packageType.motorPackageUnlocked:
                    Logger.DebugLog("MotorHandler : Door " + value.message);
                    break;

                // An RFID tag has been found
                case packageType.RfidPackageFound:
                    if (SecurityController.Instance.IsSecureRFIDTag(value.message))
                    {
                        Logger.Log("[" + value.message + "]" + " - " + SecurityController.Instance.RetrievePersonByTag(value.message).name);
                        motorHandler.UnlockDoor();
                    } else
                    {
                        Logger.Log("[" + value.message + "]" + " - " + "Unknown");
                    }
                    break;
                
                // RFID tag lost
                case packageType.RfidPackageLost:
                    if (SecurityController.Instance.IsSecureRFIDTag(value.message))
                    {
                        Logger.Log("[" + value.message + "]" + " - " + SecurityController.Instance.RetrievePersonByTag(value.message).name);
                        lockTimer.Start();
                    } else
                    {
                        Logger.Log("[" + value.message + "]" + " - " + "Unknown");
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

        public void OnError(Exception error) { }

        public void OnCompleted() { }

        /// <summary>
        /// Cancels the timer.
        /// </summary>
        public void Dispose()
        {
            lockTimer.Dispose();
        }
    }
}
