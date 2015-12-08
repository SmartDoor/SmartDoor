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
            SecurityController secController = new SecurityController();
            secController.readSecureRFIDTags();
            MasterController masterController = new MasterController(secController);
            masterController.Setup();

            AdminController AdminController = new AdminController(masterController, secController);
            
            AdminController.AdminCLI();

            // Handle shutdown...
            masterController.Shutdown();
        }
    }
}
