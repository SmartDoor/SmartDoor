using SmartDoor.Utilities;
using System;
using System.Collections;

namespace SmartDoor.Templates
{
    /// <summary>
    /// 
    /// </summary>
    class PersonContainer
    {
        public ArrayList personDictionary;

        /// <summary>
        /// 
        /// </summary>
        public PersonContainer()
        {
            personDictionary = new ArrayList();
        }

        /// <summary>
        /// Checks if tag is Secure or not
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public bool isTagRegistred(String tag)
        {
            foreach(Person person in personDictionary)
            {
                if(person.rfid == tag)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the owner of tag if there is one
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public Person getOwnerOfTag(String tag)
        {
            foreach (Person person in personDictionary)
            {
                if (person.rfid == tag)
                {
                    return person;
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public bool RegisterRFIDTag(Person person)
        {
            foreach (Person registredPerson in personDictionary)
            {
                if (registredPerson.rfid == person.rfid)
                {
                    return false;
                }
            }

            Logger.Err("Success, Person is now registred");
            personDictionary.Add(person);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public bool UpdateRFIDTag(Person person)
        {
            foreach (Person registredPerson in personDictionary)
            {
                if (registredPerson.rfid == person.rfid)
                {
                    Logger.Log("Success, Person is now updated");
                    personDictionary.Remove(registredPerson);
                    personDictionary.Add(person);
                    return true;
                }
            }

            Logger.Err("Error, Person is not registred");
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public bool DeleteRFIDtag(String tag)
        {
            foreach (Person registredPerson in personDictionary)
            {
                if (registredPerson.rfid == tag)
                {
                    Logger.Log("Success, Person is now removed");
                    personDictionary.Remove(registredPerson);
                    return true;
                }
            }

            Logger.Err("Error, Person is not registred");
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public bool DeleteOwner(String name)
        {
            foreach (Person registredPerson in personDictionary)
            {
                if (registredPerson.name == name)
                {
                    Logger.Log("Success, person is now removed");
                    personDictionary.Remove(registredPerson);
                    return true;
                }
            }
            Logger.Log("Error, person is not registred");
            return false;
        }
    }
}
