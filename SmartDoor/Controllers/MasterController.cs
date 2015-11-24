using SmartDoor.ComponentHandlers;
using System;

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

        public MasterController()
        {
            rfidHandler = new RFIDHandler();
            motorHandler = new MotorHandler(1);
            interfaceHandler = new InterfaceHandler();
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
                    interfaceHandler.GreenLED(false);
                    interfaceHandler.RedLED(true);
                    break;

                case packageType.motorPackageUnlocked:
                    Console.Out.WriteLine("Controller: Unlocked Door: " + value.message);
                    interfaceHandler.GreenLED(true);
                    interfaceHandler.RedLED(false);
                    break;

                case packageType.RfidPackageFound:
                    Console.Out.WriteLine("Controller: Found RFID TAG: " + value.message);
                    motorHandler.Unlock();
                        break;

                case packageType.RfidPackageLost:
                    Console.Out.WriteLine("Controller: LOST RFID TAG: " + value.message);
                    motorHandler.Lock();
                    break;

                default:
                    throw new Exception("Unknown typeEnum.");                   
            }
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
