using System;
using Phidgets;
using Phidgets.Events;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDoor.ComponentHandlers
{
    /// <summary>
    /// This class handles the interface connection to the 
    /// interfacekit.
    /// 
    /// This class can handle LED lights and some analog sensors
    /// that is connected to it.
    /// </summary>
    class InterfaceHandler : Component
    {
        private InterfaceKit interfaceKit;

        /// <summary>
        /// Constructs a new interface handler.
        /// </summary>
        public InterfaceHandler()
        {
            interfaceKit = new InterfaceKit();
        }

        /// <summary>
        /// Waits for attachment to be able to use the interface kit.
        /// This method should be called before the component is used.
        /// </summary>
        public void WaitForAttach()
        {
            Console.Out.WriteLine("Interface: Waiting for attachment...");

            interfaceKit.open();
            interfaceKit.waitForAttachment();

            Console.Out.WriteLine("Interface attached... OK!");
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
        /// Toggles the green LED.
        /// </summary>
        /// <param name="status">Status for the LED true is on and
        /// false is off.</param>
        public void GreenLED(bool status)
        {
            interfaceKit.outputs[0] = status;
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

        /// <summary>
        /// Toggles the yellow LED.
        /// </summary>
        /// <param name="status">Status for the LED true is on and
        /// false is off.</param>
        public void YellowLED(bool status)
        {
            interfaceKit.outputs[3] = status;
        }
    }
}
