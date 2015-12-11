using Phidgets;
using Phidgets.Events;
using SmartDoor.Templates;
using SmartDoor.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmartDoor.Controllers
{
    class InterfaceController : IObserver<Package>, Controller
    {
        private static InterfaceController instance;

        private InterfaceController()
        {
            interfaceKit = new InterfaceKit();
            ledHandler = new LEDHandler(interfaceKit);

            interfaceKit.SensorChange += new SensorChangeEventHandler(ifKit_SensorChange);
        }

        public static InterfaceController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InterfaceController();
                }
                return instance;
            }
        }

        private InterfaceKit interfaceKit;
        private LEDHandler ledHandler;

        private const int LIGHT_SENSOR_INDEX = 0;
        private const int IR_SENSOR_INDEX = 1;

        public bool isDark = false;
        public bool isDoorClosed = false;

        /// <summary>
        /// 
        /// </summary>
        public void Setup()
        {
            try
            {
                Console.Out.WriteLine("InterfaceKit: Waiting for attachment...");
                interfaceKit.open();
                interfaceKit.waitForAttachment();
            }
            catch (PhidgetException e)
            {
                Console.Error.WriteLine("InterfaceKit : A fatal error occured: ");
                Console.Error.WriteLine(e.Description);
                Environment.Exit(-1);
            }

            Console.Out.WriteLine("InterfaceKit : Done waiting for attachment!");

            ledHandler.WaitForAttach();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Shutdown()
        {
            ledHandler.Shutdown();
            interfaceKit.close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void OnNext(Package value)
        {
            switch (value.type)
            {
                case packageType.motorPackageLocked:
                    ledHandler.LockStatus(true);
                    break;

                case packageType.motorPackageUnlocked:
                    ledHandler.LockStatus(false);
                    break;

                case packageType.RfidPackageFound:
                    ledHandler.RfidFoundStatus(true);
                    break;

                case packageType.RfidPackageLost:
                    ledHandler.RfidFoundStatus(false);
                    break;

                default:
                    throw new Exception("Unknown Package");
            }
        }
        //Input Change event handler...Display the input index and the new value to the 
        //console
        static void ifKit_InputChange(object sender, InputChangeEventArgs e)
        {
            Console.WriteLine("Input index {0} value (1)", e.Index, e.Value.ToString());
        }

        //Output change event handler...Display the output index and the new valu to 
        //the console
        static void ifKit_OutputChange(object sender, OutputChangeEventArgs e)
        {
            Console.WriteLine("Output index {0} value {0}", e.Index, e.Value.ToString());
        }


        private void ifKit_SensorChange(object sender, SensorChangeEventArgs e)
        {
            switch (e.Index)
            {
                case LIGHT_SENSOR_INDEX:
                    if(e.Value < 150)
                    {
                        isDark = true;
                    } else
                    {
                        isDark = false;
                    }
                    Logger.DebugLog("isdark :" + isDark);
                    break;
                case IR_SENSOR_INDEX:
                    if (e.Value < 200)
                    {
                        isDoorClosed = true; 
                    }
                    else
                    {
                        isDoorClosed = false;
                    }
                    Logger.DebugLog("IsDoorClosed :" + isDoorClosed);
                    break;
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
