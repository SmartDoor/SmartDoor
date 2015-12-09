using SmartDoor.Controllers;
using System;

/// <summary>
/// File: Program.cs
/// Version: 2015-11-24
/// 
/// Description:
///  Main Program file, responsible for starting the the services needed by
///  out project.
/// </summary>
namespace SmartDoor
{
    class Program
    {

        /// <summary>
        /// Launches the doors commandline interface.
        /// </summary>
        /// <param name="args">Not used by this program yet.</param>
        static void Main(string[] args)
        {
            SecurityController.Instance.readSecureRFIDTags();
            DisplayController.Instance.Setup();
            MasterController.Instance.Setup();
            AdminController.Instance.AdminCLI();

            // Handle shutdown...
            MasterController.Instance.Shutdown();
        }
    }
}
