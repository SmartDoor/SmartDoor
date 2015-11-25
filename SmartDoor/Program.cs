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
            MasterController controller = new MasterController(secController);
            controller.Setup();

            String input = "";

            Console.WriteLine("Welcome to Door-CLI");
            while (!input.Equals("exit"))
            {
                Console.Out.WriteAsync("$ ");
                input = Console.ReadLine();

                switch (input)
                {
                    case "addtag":
                        Console.Out.WriteAsync("Enter tag ID: ");
                        String tagID = Console.ReadLine();
                        secController.addTag(tagID);
                        secController.writeSecureRFIDTags();

                        Console.Out.WriteLineAsync("Added tag: " + tagID);
                        break;

                    case "removetag":
                        secController.removeTag(Console.ReadLine());
                        secController.writeSecureRFIDTags();
                        break;
                    default:
                        break;
                }

            }
            // Handle shutdown...
            controller.Shutdown();
        }
    }
}
