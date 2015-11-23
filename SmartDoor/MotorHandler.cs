using System;

using Phidgets;
using Phidgets.Events;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SmartDoor
{
    class MotorHandler
    {
        private static double DOOR_LOCKED = 180;
        private static double DOOR_UNLOCKED = 0;

        private AdvancedServo m_ServoController;
        private AdvancedServoServo[] m_Servos;

        private double targetPosition;
        private double currentPosition;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="noOfMotors"></param>
        public MotorHandler(int noOfMotors)
        {
            m_ServoController = new AdvancedServo();
            m_ServoController.PositionChange += new PositionChangeEventHandler(advServo_PositionChange);


            m_Servos = new AdvancedServoServo[noOfMotors];

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
            m_Servos[0].Engaged = status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void advServo_Attach(object sender, AttachEventArgs e)
        {
            AdvancedServo attached = (AdvancedServo)sender;

           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void advServo_Detach(object sender, DetachEventArgs e)
        {
                 
        }

        public void waitForAttach()
        {
            try
            {
                Console.WriteLine("Waiting for attachment...");

                m_ServoController.open();

                m_ServoController.waitForAttachment();

                m_Servos[0] = m_ServoController.servos[0];

                Console.WriteLine("Done waiting for attachment!");

            }
            catch (PhidgetException e) {
                Console.Out.WriteLine(e.Description);
                // System.exit(-1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Unlock()
        {
            Console.WriteLine("Unlocking door");
            m_Servos[0].Position = DOOR_UNLOCKED;
            targetPosition = DOOR_UNLOCKED;
            m_Servos[0].Engaged = true;
            
            while(currentPosition != targetPosition)
            {
                m_Servos[0].Engaged = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Lock()
        {
            Console.WriteLine("Locking door");
            targetPosition = DOOR_LOCKED;
            m_Servos[0].Position = DOOR_LOCKED;
            m_Servos[0].Engaged = true;

            while (currentPosition != targetPosition)
            {
                m_Servos[0].Engaged = true;
            }
        }

    }
}
