using SmartDoor.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDoor.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    class SecurityController
    {
        private RFIDTags secureRFIDTags;
        private FileHandler fileHandler;

        /// <summary>
        /// 
        /// </summary>
        public SecurityController()
        {
            fileHandler = new FileHandler();
        }

        /// <summary>
        /// 
        /// </summary>
        public void readSecureRFIDTags()
        {
            try
            {
                secureRFIDTags = fileHandler.ReadFile();
            } catch (FileNotFoundException e )
            {
                if(secureRFIDTags == null)
                {
                    secureRFIDTags = new RFIDTags();
                }
            }
        }

        /// <summary>
        /// Adds a tag to the list of allowed RFID tags.
        /// </summary>
        /// <param name="tag">Tag to be added.</param>
        public void addTag(String tag)
        {
            secureRFIDTags.addRFIDTag(tag);
           
        }

        /// <summary>
        /// Saves the current allowed RFID tags into file.
        /// </summary>
        public void writeSecureRFIDTags()
        {
            fileHandler.WriteToFile(secureRFIDTags);
        }

        /// <summary>
        /// Removes an RFID tag from the allowed tags in memory.
        /// </summary>
        /// <param name="v"></param>
        public void removeTag(string v)
        {
            secureRFIDTags.deleteRFIDtag(v);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public Boolean isSecureRFIDTag(String tag)
        {
            foreach(String secureTag in secureRFIDTags.getRFIDTags())
            {
                if (secureTag.Equals(tag))
                    return true;
            }
            return false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    class RFIDTags
    {
        private List<String> secureRFIDTags;

        /// <summary>
        /// 
        /// </summary>
        public RFIDTags()
        {
            secureRFIDTags = new List<String>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secureRFIDTags"></param>
        public RFIDTags(List<String> secureRFIDTags)
        {
            this.secureRFIDTags = secureRFIDTags;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<String> getRFIDTags()
        {
            return secureRFIDTags;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public void addRFIDTag(String tag)
        {
            secureRFIDTags.Add(tag);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public void deleteRFIDtag(String tag)
        {
            secureRFIDTags.Remove(tag);
        }
    }
}
