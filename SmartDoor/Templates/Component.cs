using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDoor.Controllers
{
    /// <summary>
    /// A Simple interface that helps the usages
    /// of Phidget components.
    /// </summary>
    interface Component
    {
        void WaitForAttach();
        void Shutdown();
    }
}
