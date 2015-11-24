using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDoor.ComponentHandlers
{
    /// <summary>
    /// 
    /// </summary>
    interface Component
    {
        void WaitForAttach();
        void Shutdown();
    }
}
