using System;
using Phidgets;
using Phidgets.Events;
using System.Collections.Generic;
using SmartDoor.ComponentHandlers;

namespace SmartDoor
{
    class MotorHandler : IObservable<Package>, Component
    {
        private static double DOOR_LOCKED = 210;
        private static double DOOR_UNLOCKED = 30;

        private AdvancedServo servoController;
        private AdvancedServoServo[] servoMotor;

        private double targetPosition;
        private double currentPosition;
        private List<IObserver<Package>> observers;

        /// <summary>
        /// Constructs a new Motorhandler.
        /// Only supports one motor for the moment.
        /// </summary>
        /// <param name="noOfMotors">Number of motors connected to the Servo</param>
        public MotorHandler(int noOfMotors)
        {
            servoController = new AdvancedServo();
            servoController.PositionChange += new PositionChangeEventHandler(advServo_PositionChange);
            servoMotor = new AdvancedServoServo[noOfMotors];

            observers = new List<IObserver<Package>>();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void advServo_PositionChange(object sender, PositionChangeEventArgs e)
        {
            Console.WriteLine("CurrPos : " + e.Position);
            currentPosition = e.Position;
        }

        public void SetEngaged(bool status)
        {
            servoMotor[0].Engaged = status;
        }

        /// <summary>
        /// Attach handler, TODO: Handle this later on.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void advServo_Attach(object sender, AttachEventArgs e)
        {
            AdvancedServo attached = (AdvancedServo)sender;

           
        }

        /// <summary>
        /// Handles shutdown of the Servo and motor
        /// by closing the Servo stream and setting the motors
        /// engaged status to false.
        /// </summary>
        public void Shutdown()
        {
            servoMotor[0].Engaged = false;
            servoController.close();
        }

        /// <summary>
        /// Detach handler, TODO: Handle this later on.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void advServo_Detach(object sender, DetachEventArgs e)
        {
            //TOOD: Handle detach event.
           
        }

        /// <summary>
        /// Waits for the motor to be attached. Must wait 
        /// </summary>
        public void WaitForAttach()
        {
            try
            {
                Console.Out.WriteLine("Waiting for attachment...");

                servoController.open();

                servoController.waitForAttachment();

                servoMotor[0] = servoController.servos[0];

                Console.Out.WriteLine("Done waiting for attachment!");

            }
            catch (PhidgetException e) {
                Console.Error.WriteLine("A fatal error occured: ");
                Console.Error.WriteLine(e.Description);
                servoController.close();
                Environment.Exit(-1);
            }
        }

        /// <summary>
        /// Unlocks the door by turning to <code>DOOR_UNLOCKED</code> degrees.
        /// </summary>
        public void Unlock()
        {
            Console.Out.WriteLine("Unlocking door");
            servoMotor[0].Position = DOOR_UNLOCKED;
            targetPosition = DOOR_UNLOCKED;
            servoMotor[0].Engaged = true;
            
            while(currentPosition != targetPosition)
            {
                servoMotor[0].Engaged = true;
            }

            Package package = new Package(packageType.motorPackageUnlocked, "unlocked");
            foreach (var observer in observers)
                observer.OnNext(package);
        }

        /// <summary>
        /// Locks the door by turning to <code>DOOR_LOCKED</code> degrees.
        /// </summary>
        public void Lock()
        {
            Console.Out.WriteLine("Locking door");
            targetPosition = DOOR_LOCKED;
            servoMotor[0].Position = DOOR_LOCKED;
            servoMotor[0].Engaged = true;

            while (currentPosition != targetPosition)
            {
                servoMotor[0].Engaged = true;
            }

            Package package = new Package(packageType.motorPackageLocked, "locked");
            foreach (var observer in observers)
                observer.OnNext(package);

        }

        public IDisposable Subscribe(IObserver<Package> observer)
        {
            // Check whether observer is already registered. If not, add it
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }
            return new Unsubscriber<Package>(observers, observer);
        }
    }
}
