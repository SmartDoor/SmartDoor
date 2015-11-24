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
            MotorHandler motorHandler = new MotorHandler(1);

            motorHandler.waitForAttach();

            String input = "";

            Console.WriteLine("Welcome to Door-CLI");
            while (!input.Equals("exit"))
            {
                Console.Out.Write("$ ");
                input = Console.ReadLine();

                switch (input)
                {
                    case "Lock":
                        Console.WriteLine("Locking door");
                        motorHandler.Lock();
                        break;

                    case "Unlock":
                        Console.WriteLine("Unlocking door");
                        motorHandler.Unlock();
                        break;

                    case "Disengage":
                        Console.WriteLine("Disengage");
                        motorHandler.SetEngaged(false);
                        break;

                    case "Engage":
                        Console.WriteLine("Engaged");
                        motorHandler.SetEngaged(true);
                        break;

                    default:
                        Console.WriteLine("Unknown command");
                        break;
                }
            }
            // Handle shutdown...
            motorHandler.shutDown();
        }
    }
}
