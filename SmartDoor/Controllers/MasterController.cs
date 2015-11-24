using SmartDoor.ComponentHandlers;
using System;

namespace SmartDoor
{
    class MasterController : IObserver<Package>
    {
     

        private RFIDHandler rfidHandler;
        private MotorHandler motorHandler;

        public MasterController()
        {
            rfidHandler = new RFIDHandler();
            motorHandler = new MotorHandler(1);
        }

        public void Setup()
        {
            rfidHandler.waitForAttach();
            motorHandler.waitForAttach();

            rfidHandler.Subscribe(this);
            motorHandler.Subscribe(this);
        }

        public void Shutdown()
        {
            rfidHandler.ShutDown();
            motorHandler.shutDown();
        }

        public void OnNext(Package value)
        {
           switch(value.type)
            {
                case packageType.motorPackageLocked:
                    Console.Out.WriteLine("Controller: Locked Door: " + value.message);
                    break;

                case packageType.motorPackageUnlocked:
                    Console.Out.WriteLine("Controller: Unlocked Door: " + value.message);
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

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }
    }
}
