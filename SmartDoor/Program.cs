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
            InterfaceController.Instance.Setup();
            DisplayController.Instance.Setup();
            MotorController.Instance.Setup();
            SecurityController.Instance.Setup();
            AdminController.Instance.AdminCLI();

            // Handle shutdown...
            MotorController.Instance.Shutdown();
            InterfaceController.Instance.Shutdown();
            DisplayController.Instance.Shutdown();
            MotorController.Instance.Shutdown();
            SecurityController.Instance.Shutdown();
        }
    }
}
