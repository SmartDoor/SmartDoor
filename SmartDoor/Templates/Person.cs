using System;
using System.Collections;
using System.Text;

namespace SmartDoor.Templates
{
    class Person
    {
        public String rfid;
        public String name;
        public String email;
        public String birthday;

        private ArrayList messages;

        // Thread safe access locking object.
        [NonSerialized]
        private Object accessLock = new Object();


        /// <summary>
        /// Adds a message to this persons personalised 
        /// message queue.
        /// 
        /// Is thread safe due to accesslocking.
        /// </summary>
        /// <param name="message"></param>
        public void addMessage(String message)
        {
            lock(accessLock)
            {
                if (messages == null)
                    messages = new ArrayList();

                messages.Add(message);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void removeMessage(String message)
        {
            lock(accessLock)
            {
                if (messages != null)
                {
                    messages.Remove(message);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ArrayList getMessage()
        {
            lock(accessLock)
            {
                return this.messages;
            }
        }

        override
        public String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Name : " + name + "\n");
            sb.Append("Email : " + email + "\n");
            sb.Append("Birthday : " + birthday + "\n");
            return sb.ToString();
        }

        public static String UnkownToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Name : " + "Unknown" + "\n");
            sb.Append("Email : " + "Unknown" + "\n");
            sb.Append("Birthday : " + "Unknown" + "\n");
            return sb.ToString();
        }
    }
}
