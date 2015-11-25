using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartDoor.Controllers;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SmartDoor.Utilities
{
    class FileHandler
    {
        public void WriteToFile(RFIDTags secureRFIDTags)
        {
            Stream stream = File.Open("RFID.Secure", FileMode.Create);
            BinaryFormatter bformatter = new BinaryFormatter();

            bformatter.Serialize(stream, secureRFIDTags);
            stream.Close();
        }


        public RFIDTags ReadFile()
        {
            RFIDTags secureRFIDTags = null;

            //Open the file written above and read values from it.
            Stream stream = File.Open("RFID.Secure", FileMode.Open);
            BinaryFormatter bformatter = new BinaryFormatter();

            secureRFIDTags = (RFIDTags)bformatter.Deserialize(stream);
            stream.Close();

            return secureRFIDTags;
        }
    }
}
