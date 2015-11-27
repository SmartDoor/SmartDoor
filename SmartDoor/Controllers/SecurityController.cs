using SmartDoor.Templates;
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
            Person owner = new Person();
            Console.WriteLine("\n Tag [" + tag + "]");
            Console.Write("Enter name of owner : ");
            owner.name = Console.ReadLine();
            Console.WriteLine("");
            owner.email = owner.name + "@gmail.com";
            owner.birthday = "2000-00-00";

            secureRFIDTags.RegisterRFIDTag(tag,owner);
            fileHandler.WriteToFile(secureRFIDTags);
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
        /// <param name="tag"></param>
        public void removeTag(string tag)
        {
            secureRFIDTags.DeleteRFIDtag(tag);
            fileHandler.WriteToFile(secureRFIDTags);
        }

        /// <summary>
        /// Removes an RFID tag from the allowed tags in memory.
        /// </summary>
        /// <param name="tag"></param>
        public void removeTagOwner(string owner)
        {
            secureRFIDTags.DeleteOwner(owner);
            fileHandler.WriteToFile(secureRFIDTags);
        }


        /// <summary>
        /// Retrieves an tag
        /// </summary>
        /// <param name="tag"></param>
        public Person retrieveTag(string tag)
        {
            return secureRFIDTags.getOwnerOfTag(tag);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public bool isSecureRFIDTag(String tag)
        {
            return secureRFIDTags.isTagRegistred(tag);
        }
    }
}
