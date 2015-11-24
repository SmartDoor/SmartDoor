using System;
using Phidgets;
using Phidgets.Events;

namespace SmartDoor
{
    class MotorHandler
    {
        private static double DOOR_LOCKED = 210;
        private static double DOOR_UNLOCKED = 30;

        private AdvancedServo servoController;
        private AdvancedServoServo[] servoMotor;

        private double targetPosition;
        private double currentPosition;

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
        public void shutDown()
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
        public void waitForAttach()
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
        }

    }
}
