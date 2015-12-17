using SmartDoor.Templates;
using SmartDoor.Utilities;
using System;
using System.IO;

namespace SmartDoor.Controllers
{
    /// <summary>
    /// 
    /// Implemented with the singleton design pattern.
    /// </summary>
    class SecurityController : Controller
    {
        private static SecurityController instance;

        /// <summary>
        /// Private constructor, initialises the object.
        /// </summary>
        private SecurityController()
        {
            rfidHandler = new RFIDHandler();
            fileHandler = new FileHandler();
        }

        /// <summary>
        /// Class accessor attribute.
        /// </summary>
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

        private PersonHandler personHandler;
        public RFIDHandler rfidHandler;
        private FileHandler fileHandler;


        /// <summary>
        /// Setups the rfidhandler and
        /// subscribes the rfidhandler to the needed
        /// controllers.
        /// </summary>
        public void Setup()
        {
            rfidHandler.WaitForAttach();
            rfidHandler.Subscribe(MotorController.Instance);
            rfidHandler.Subscribe(DisplayController.Instance);
            rfidHandler.Subscribe(AdminController.Instance);
            rfidHandler.Subscribe(InterfaceController.Instance);
        }

        /// <summary>
        /// Reads a rfid tag
        /// </summary>
        public void readSecureRFIDTags()
        {
            try
            {
                personHandler = fileHandler.ReadFile();
            } catch (FileNotFoundException e)
            {
                if(personHandler == null)
                {
                    personHandler = new PersonHandler();
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

            if (personHandler.RegisterPerson(person))
            {
                Logger.Log("[Registred][" + tag + "] " + person.name);
                fileHandler.WriteToFile(personHandler);
            }
        }

        /// <summary>
        /// Removes an RFID tag from the allowed tags in memory.
        /// </summary>
        /// <param name="tag"></param>
        public void RemovePersonTag(string tag)
        {
            if (personHandler.DeleteRFIDtag(tag))
            {
                Logger.Log("[Removed][" + tag + "]");
                fileHandler.WriteToFile(personHandler);
            }
        }

        /// <summary>
        /// Removes an RFID tag from the allowed tags in memory.
        /// </summary>
        /// <param name="tag"></param>
        public void removeTagOwner(string owner)
        {
            if (personHandler.DeletePerson(owner))
            {
                Logger.Log("[Removed] " + owner);
                fileHandler.WriteToFile(personHandler);
            }
        }


        /// <summary>
        /// Retrieves an tag
        /// </summary>
        /// <param name="tag"></param>
        public Person RetrievePersonByTag(string tag)
        {
            return personHandler.getOwnerOfTag(tag);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public bool IsSecureRFIDTag(String tag)
        {
            readFIle();
            return personHandler.isTagRegistred(tag);
        }

        /// <summary>
        /// Starts the shutdown sequence for the rfidhandler.
        /// </summary>
        public void Shutdown()
        {
            rfidHandler.Shutdown();
        }

        private void readFIle()
        {
            try
            {
                personHandler = fileHandler.ReadFile();
            }
            catch (FileNotFoundException e)
            {
                if (personHandler == null)
                {
                    personHandler = new PersonHandler();
                }
            }
        }
    }
}
