using System;
using System.Collections.Concurrent;
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
        private ConcurrentDictionary<String,Person> secureRFIDTags;

        /// <summary>
        /// 
        /// </summary>
        public RFIDTags()
        {
            secureRFIDTags = new ConcurrentDictionary<String, Person>();
        }

        /// <summary>
        /// Checks if tag is Secure or not
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public bool isTagRegistred(String tag)
        {
            Person owner;
            return secureRFIDTags.TryGetValue(tag,out owner);
        }

        /// <summary>
        /// Gets the owner of tag if there is one
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public Person getOwnerOfTag(String tag)
        {
            Person owner;
            secureRFIDTags.TryGetValue(tag, out owner);
            return owner;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public bool RegisterRFIDTag(String tag,Person owner)
        {
            if (!secureRFIDTags.TryAdd(tag, owner))
            {
                Console.Error.WriteLine("Tag already exists, use updatetag");
            } else
            {
                Console.WriteLine("Success, tag is now registred");
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public bool UpdateRFIDTag(String tag, Person owner)
        {
            Person lastOwner;
            if(secureRFIDTags.TryGetValue(tag, out lastOwner))
            {
                if (!secureRFIDTags.TryUpdate(tag, owner, lastOwner))
                {
                    Console.Error.WriteLine("Error,Could not update tag");
                } else
                {
                    Console.WriteLine("Success, tag is now registred");
                    return true;
                }
            } else
            {
                Console.Error.WriteLine("Error, Tag is not registred");
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public bool DeleteRFIDtag(String tag)
        {
            Person lastOwner;
            if (!secureRFIDTags.TryRemove(tag, out lastOwner))
            {
                Console.Error.WriteLine("Error,Could not remove tag");
            }
            else
            {
                if (lastOwner == null)
                {
                    Console.Error.WriteLine("Error, Tag is not registred");
                } else
                {
                    Console.WriteLine("Success, Tag is now removed");
                    return true;
                }
            }
            return false;
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
                    Person lastOwner;
                    if(!secureRFIDTags.TryRemove(key,out lastOwner))
                    {
                        Console.Error.WriteLine("Error, Could not remove tag");
                    } else if (lastOwner == null)
                    {
                        Console.Error.WriteLine("Error, Tag is not registred");
                    } else
                    {
                        Console.WriteLine("Success, Tag is now removed");
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
