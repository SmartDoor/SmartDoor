using SmartDoor.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SmartDoor.Templates
{
    /// <summary>
    /// 
    /// </summary>
    class PersonHandler
    {
        public List<Person> personDictionary;

        /// <summary>
        /// 
        /// </summary>
        public PersonHandler()
        {
            personDictionary = new List<Person>();
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
        public bool RegisterPerson(Person person)
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
        public bool UpdatePerson(Person person)
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
        public bool DeletePerson(String name)
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
