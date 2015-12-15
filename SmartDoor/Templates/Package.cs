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
    /// A simple datastructure class for defining 
    /// how information is sent between objects that is using
    /// the observer-observable pattern.
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
