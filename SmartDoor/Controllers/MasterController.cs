using SmartDoor.ComponentHandlers;
using System;
using System.Timers;
using SmartDoor.Controllers;

namespace SmartDoor
{
    /// <summary>
    /// The Master Controller of the program. Will handle communication between our different devices.
    /// </summary>
    class MasterController : IObserver<Package>
    {
        private RFIDHandler rfidHandler;
        private MotorHandler motorHandler;
        private InterfaceHandler interfaceHandler;
        private Timer aTimer;
        private SecurityController secController;

        public MasterController(SecurityController secController)
        {
            this.secController = secController;

            rfidHandler = new RFIDHandler();
            motorHandler = new MotorHandler(1);
            interfaceHandler = new InterfaceHandler();

            aTimer = new Timer(1000 * 5);
            aTimer.AutoReset = true;
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
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
                    Console.Out.WriteLine("Controller: Locked Door: " + value.message);
                    aTimer.Stop();
                    interfaceHandler.GreenLED(false);
                    interfaceHandler.RedLED(true);
                    break;

                case packageType.motorPackageUnlocked:
                    Console.Out.WriteLine("Controller: Unlocked Door: " + value.message);
                    interfaceHandler.GreenLED(true);
                    interfaceHandler.RedLED(false);
                    break;

                case packageType.RfidPackageFound:
                    Console.Out.WriteLine("Controller: Found RFID TAG: " + value.message + " is secure " + secController.isSecureRFIDTag(value.message));

                    if(secController.isSecureRFIDTag(value.message))
                    {
                        motorHandler.Unlock();
                    }
                    break;

                case packageType.RfidPackageLost:
                    Console.Out.WriteLine("Controller: LOST RFID TAG: " + value.message +" is secure " + secController.isSecureRFIDTag(value.message) );
                    aTimer.Start();
                    break;

                default:
                    throw new Exception("Unknown typeEnum.");                   
            }
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            motorHandler.Lock();
            aTimer.Enabled = false;
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
    }
}
