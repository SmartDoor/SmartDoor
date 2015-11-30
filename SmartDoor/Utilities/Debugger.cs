using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDoor.Utilities
{
    class Logger
    {
        static Logger instance;
        static bool debugMode = false;
        static bool logMode = true;

        private Logger() { }

        public static Logger GetInstance()
        {
            if(instance == null)
            {
                instance = new Logger();
                return instance;
            }
            return instance;
        }

        public void ToggleDebugMode()
        {
            debugMode = !debugMode;
        }

        public void ToggleLogMode()
        {
            logMode = !logMode;
        }

        public static void DebugLog(String log)
        {
            if (debugMode)
            {
                Console.WriteLine("[Debug] " + log);
            }
        }

        public static void DebugErr(String err)
        {
            if (debugMode)
            {
                Console.Error.WriteLine("[Debug] " + err);
            }
        }

        public static void Log(String log)
        {
            if (logMode)
            {
                Console.WriteLine(log);
            }
        }

        public static void Err(String log)
        {
            if (logMode)
            {
                Console.Error.WriteLine(log);
            }
        }

    }
}
