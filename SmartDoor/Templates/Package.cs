using System;

namespace SmartDoor.Controllers
{
    enum packageType
    {
        RfidPackageLost,
        RfidPackageFound,
        motorPackageLocked,
        motorPackageUnlocked
    };

    /// <summary>
    /// 
    /// </summary>
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
