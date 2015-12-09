using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
        public void WriteToFile(PersonContainer securePersons)
        {
            Logger.DebugLog("Updating file");
            String json = JsonConvert.Serialize(securePersons);
            File.WriteAllText("RFID.Secure", json);
        }

        /// <summary>
        /// Read a file that contains a serialized object
        /// </summary>
        /// <returns></returns>
        public PersonContainer ReadFile()
        {
            Logger.DebugLog("Trying to read file");
            PersonContainer securePersons = null;

            //Open the file written above and read values from it.
            Console.WriteLine("FILE : " + File.ReadAllText("RFID.Secure"));
            securePersons = JsonConvert.Deserialize<PersonContainer>(File.ReadAllText("RFID.Secure"));

            return securePersons;
        }
    }
}
