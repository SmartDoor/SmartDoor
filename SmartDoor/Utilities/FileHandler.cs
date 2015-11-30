using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SmartDoor.Templates;

namespace SmartDoor.Utilities
{
    class FileHandler
    {
        /// <summary>
        /// Writes a serializable class to a file
        /// </summary>
        /// <param name="secureRFIDTags"></param>
        public void WriteToFile(RFIDTags secureRFIDTags)
        {
            Logger.DebugLog("Updating file");
            FileStream stream = File.Create("RFID.Secure");

            BinaryFormatter bformatter = new BinaryFormatter();
            bformatter.Serialize(stream, secureRFIDTags);

            stream.Close();
        }

        /// <summary>
        /// Read a file that contains a serialized object
        /// </summary>
        /// <returns></returns>
        public RFIDTags ReadFile()
        {
            Logger.DebugLog("Trying to read file");
            RFIDTags secureRFIDTags = null;

            //Open the file written above and read values from it.
            FileStream stream = File.Open("RFID.Secure", FileMode.Open);
            BinaryFormatter bformatter = new BinaryFormatter();

            secureRFIDTags = (RFIDTags)bformatter.Deserialize(stream);
            stream.Close();

            return secureRFIDTags;
        }
    }
}
