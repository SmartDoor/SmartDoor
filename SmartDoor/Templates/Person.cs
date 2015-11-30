using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDoor.Templates
{
    [Serializable]
    class Person
    {
        public String name;
        public String email;
        public String birthday;

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
