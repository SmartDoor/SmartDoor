using SmartDoor.Controllers;
using System;
using System.Timers;
using SmartDoor.Utilities;
using System.Runtime.CompilerServices;
using SmartDoor.Templates;

namespace SmartDoor
{
    /// <summary>
    /// The Master Controller of the program. Will handle communication between
    ///  our different components.
    /// </summary>
    class MotorController : IObserver<Package>, IDisposable, Controller
    {
        private static MotorController instance;

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
        /// 
        /// </summary>
        public void Shutdown()
        {
            motorHandler.Shutdown();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void OnNext(Package value)
        {
           switch(value.type)
            {
                case packageType.motorPackageLocked:
                    Logger.DebugLog("MotorHandler : Door " + value.message);
                    lockTimer.Stop();
                    break;

                case packageType.motorPackageUnlocked:
                    Logger.DebugLog("MotorHandler : Door " + value.message);
                    break;

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
