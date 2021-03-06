﻿using System;
using Phidgets;

namespace SmartDoor.Controllers
{
    /// <summary>
    /// This class handles the interface connection to the 
    /// interfacekit.
    /// 
    /// This class can handle LED lights and some analog sensors
    /// that is connected to it.
    /// </summary>
    class LEDHandler : Component
    {
        private InterfaceKit interfaceKit;

        /// <summary>
        /// Constructs a new interface handler.
        /// </summary>
        public LEDHandler(InterfaceKit interfaceKit)
        {
            this.interfaceKit = interfaceKit;
        }

        /// <summary>
        /// Waits for attachment to be able to use the interface kit.
        /// This method should be called before the component is used.
        /// </summary>
        public void WaitForAttach()
        {
            if (interfaceKit.Attached)
            {
                GreenLED(true);
                YellowLED(true);
                RedLED(true);
            } else
            {
                Console.Error.WriteLine("Could not find interfacekit");
            }
        }

        /// <summary>
        /// Shutdowns the interface kits. 
        /// Turns of the LED lights and closes the connection to the 
        /// interfacekit.
        /// </summary>
        public void Shutdown()
        {
            GreenLED(false);
            YellowLED(false);
            RedLED(false);

            interfaceKit.close();
        }

        /// <summary>
        /// Sets corresponding leds to the lock status.
        /// </summary>
        /// <param name="status"></param>
        public void LockStatus(bool status)
        {
            if (status)
            {
                GreenLED(false);
                RedLED(true);
            } else
            {
                GreenLED(true);
                RedLED(false);
            }
        }

        /// <summary>
        /// Sets the corresponding leds of the rfidfound status.
        /// </summary>
        /// <param name="status"></param>
        public void RfidFoundStatus(bool status)
        {
            if (status)
                YellowLED(true);
            else
                YellowLED(false);
        }

        /// <summary>
        /// Toggles the green LED.
        /// </summary>
        /// <param name="status">Status for the LED true is on and
        /// false is off.</param>
        public void GreenLED(bool status)
        {
            interfaceKit.outputs[0] = status;
        }

        /// <summary>
        /// Toggles the yellow LED.
        /// </summary>
        /// <param name="status">Status for the LED true is on and
        /// false is off.</param>
        public void YellowLED(bool status)
        {
            interfaceKit.outputs[3] = status;
        }

        /// <summary>
        /// Toggles the RED LED
        /// </summary>
        /// <param name="status">Status for the LED true is on and
        /// false is off.</param>
        public void RedLED(bool status)
        {
            interfaceKit.outputs[6] = status;
        }
    }
}
