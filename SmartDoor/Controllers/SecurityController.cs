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
        private static SecurityController instance;

        private SecurityController()
        {
            fileHandler = new FileHandler();
        }

        public static SecurityController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SecurityController();
                }
                return instance;
            }
        }

        private PersonContainer secureRFIDTags;
        private FileHandler fileHandler;

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
                    secureRFIDTags = new PersonContainer();
                }
            }
        }

        /// <summary>
        /// Adds a tag to the list of allowed RFID tags.
        /// </summary>
        /// <param name="tag">Tag to be added.</param>
        public void addTag(String tag)
        {
            Person person = new Person();
            Console.WriteLine("\n Tag [" + tag + "]");
            Console.Write("Enter name of owner : ");

            String input = "";
            input = Console.ReadLine();

            person.rfid = tag;
            person.name = input;
            person.email = person.name + "@gmail.com";
            person.birthday = "2000-00-00";

            Console.WriteLine(person.ToString());

            if (secureRFIDTags.RegisterRFIDTag(person))
            {
                Logger.Log("[Registred][" + tag + "] " + person.name);
                fileHandler.WriteToFile(secureRFIDTags);
            }
        }

        /// <summary>
        /// Removes an RFID tag from the allowed tags in memory.
        /// </summary>
        /// <param name="tag"></param>
        public void removeTag(string tag)
        {
            if (secureRFIDTags.DeleteRFIDtag(tag))
            {
                Logger.Log("[Removed][" + tag + "]");
                fileHandler.WriteToFile(secureRFIDTags);
            }
        }

        /// <summary>
        /// Removes an RFID tag from the allowed tags in memory.
        /// </summary>
        /// <param name="tag"></param>
        public void removeTagOwner(string owner)
        {
            if (secureRFIDTags.DeleteOwner(owner))
            {
                Logger.Log("[Removed] " + owner);
                fileHandler.WriteToFile(secureRFIDTags);
            }
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
