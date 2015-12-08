using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDoor.Controllers
{
    enum packageType
    {
        RfidPackageLost,
        RfidPackageFound,
        motorPackageLocked,
        motorPackageUnlocked
    };

    class Package
    {
        public packageType type { get; private set; }

        public String message { get; private set; }

        public Package(packageType type, String message)
        {
            this.type = type;
            this.message = message;
        }
    }
}
