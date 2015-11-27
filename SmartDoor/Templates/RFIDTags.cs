using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDoor.Templates
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    class RFIDTags
    {
        private Dictionary<String,Person> secureRFIDTags;

        /// <summary>
        /// 
        /// </summary>
        public RFIDTags()
        {
            secureRFIDTags = new Dictionary<String, Person>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secureRFIDTags"></param>
        public RFIDTags(Dictionary<String, Person> secureRFIDTags)
        {
            this.secureRFIDTags = secureRFIDTags;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, Person> getRFIDTags()
        {
            return secureRFIDTags;
        }

        /// <summary>
        /// Checks if tag is Secure or not
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public bool isTagRegistred(String tag)
        {
            try
            {
                return secureRFIDTags[tag] != null ? true : false;
            }
            catch (KeyNotFoundException e){ }
            return false;
        }

        /// <summary>
        /// Gets the owner of tag if there is one
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public Person getOwnerOfTag(String tag)
        {
            try
            {
                return secureRFIDTags[tag];
            }
            catch (KeyNotFoundException e) { }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public void RegisterRFIDTag(String tag,Person owner)
        {
            try
            {
                /** try add */
                secureRFIDTags.Add(tag, owner);
            }
            catch (ArgumentException e)
            {
                /** Replace existing */
                secureRFIDTags[tag] = owner;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public bool DeleteRFIDtag(String tag)
        {
            try
            {
                secureRFIDTags.Remove(tag);
            } catch (KeyNotFoundException e)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public bool DeleteOwner(String name)
        {
            foreach(String key in secureRFIDTags.Keys)
            {
                if(secureRFIDTags[key].name == name)
                {
                    secureRFIDTags.Remove(key);
                    return true;
                }
            }
            return false;
        }

    }
}
