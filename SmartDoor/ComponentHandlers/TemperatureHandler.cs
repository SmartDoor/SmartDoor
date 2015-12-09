using System;
using SmartDoor.Controllers;
using Phidgets;
using Phidgets.Events;

namespace SmartDoor.ComponentHandlers
{
    /// <summary>
    /// Handles the interaction and connectivity of the temperature sensor.
    /// 
    /// For this class we have used the 1045 IR Temperature sensor as our
    /// phidget component.
    /// </summary>
    class TemperatureHandler : Component
    {

        private TemperatureSensor tempSensor;
        private int currentTemperature;

        /// <summary>
        /// Constructs a new Temperature handler.
        /// </summary>
        public TemperatureHandler()
        {
            tempSensor = new TemperatureSensor();
        }

        /// <summary>
        /// Handles the shutdown sequence of the componenent
        /// </summary>
        public void Shutdown()
        {

            tempSensor.close();
            tempSensor = null;
        }

        /// <summary>
        /// Handles the attachment sequence of the component
        /// </summary>
        public void WaitForAttach()
        {

            //Attach eventhooks.
            tempSensor.Attach += new AttachEventHandler(tempSensor_Attach);
            tempSensor.Detach += new DetachEventHandler(tempSensor_Detach);
            tempSensor.Error += new ErrorEventHandler(tempSensor_Error);

            tempSensor.TemperatureChange += new TemperatureChangeEventHandler
                                                        (tempSensor_TemperatureChange);
            // Open the stream for the component
            tempSensor.open();

            Console.Out.WriteLine("Waiting for Temperature sensor to be attached...");
            tempSensor.waitForAttachment();
        }

        /// <summary>
        /// Eventhandler for handling temperature changes in the 
        /// environment.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tempSensor_TemperatureChange(object sender, TemperatureChangeEventArgs e)
        {
            currentTemperature = (int)e.Temperature;
        }

        /// <summary>
        /// Error handler for the temperature component
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tempSensor_Error(object sender, ErrorEventArgs e)
        {
            Console.WriteLine(e.Description);
        }

        /// <summary>
        /// Detach handler for the component
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tempSensor_Detach(object sender, DetachEventArgs e)
        {
            Console.WriteLine("Temperature Sensor {0} detached!",
                                   e.Device.SerialNumber.ToString());
        }

        public string getTempString()
        {
            return "Temp: " + currentTemperature + "C";
        }

        /// <summary>
        /// Attach handler for the component
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tempSensor_Attach(object sender, AttachEventArgs e)
        {
            Console.WriteLine("Temperature Sensor {0} attached!",
                                    e.Device.SerialNumber.ToString());
        }
    }
}
