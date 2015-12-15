using System;

namespace SmartDoor.Utilities
{
    class Logger
    {
        static bool debugMode = false;
        static bool logMode = true;

        private static Logger instance;

        private Logger(){ }

        public static Logger Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Logger();
                }
                return instance;
            }
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
