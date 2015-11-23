using System;
using Phidgets;
using Phidgets.Events;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

/// <summary>
/// 
/// 
/// </summary>
namespace SmartDoor
{
    class Program
    {
        static void Main(string[] args)
        {
            MotorHandler motorHandler = new MotorHandler(1);

            motorHandler.waitForAttach();

            String input = "";

            Console.WriteLine("Welcome to Door-CLI");
            while (!input.Equals("exit"))
            {
                input = Console.ReadLine();
                switch(input)
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
            

         
        }
    }
}
