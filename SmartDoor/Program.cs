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
            MasterController controller = new MasterController();
            controller.Setup();


            String input = "";

            Console.WriteLine("Welcome to Door-CLI");
            while (!input.Equals("exit"))
            {
                Console.Out.Write("$ ");
                input = Console.ReadLine();
                
            }
            // Handle shutdown...
            controller.Shutdown();
        }
    }
}
