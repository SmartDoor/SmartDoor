using System;
using System.IO;
using SmartDoor.Templates;
using System.Web.Script.Serialization;

namespace SmartDoor.Utilities
{
    class FileHandler
    {
        private JavaScriptSerializer JsonConvert;

        public FileHandler()
        {
            JsonConvert = new JavaScriptSerializer();
        }

        /// <summary>
        /// Writes a serializable class to a file
        /// </summary>
        /// <param name="securePersons"></param>
        public void WriteToFile(PersonHandler securePersons)
        {
            Logger.DebugLog("Updating file");
            String json = JsonConvert.Serialize(securePersons);
            File.WriteAllText("RFID.Secure", json);
        }

        /// <summary>
        /// Read a file that contains a serialized object
        /// </summary>
        /// <returns></returns>
        public PersonHandler ReadFile()
        {
            Logger.DebugLog("Trying to read file");
            PersonHandler securePersons = null;

            //Open the file written above and read values from it.
            Console.WriteLine("FILE : " + File.ReadAllText("RFID.Secure"));
            securePersons = JsonConvert.Deserialize<PersonHandler>(File.ReadAllText("RFID.Secure"));

            return securePersons;
        }
    }
}
