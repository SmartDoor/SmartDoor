using SmartDoor.Controllers;
using SmartDoor.Templates;
using SmartDoor.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartDoor.Controllers
{
    class AdminController : IObserver<Package>
    {
        private static AdminController instance;

        private AdminController()
        {
            wantToAddTag = false;
            wantToRemoveTag = false;
            wantToRemoveOwner = false;
            wantToChangeOwner = false;
            wantToCheckTag = false;
            wantToAddMessage = false;
            setSleep = true;
        }

        public static AdminController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AdminController();
                }
                return instance;
            }
        }

        /** options */
        private bool wantToAddTag;
        private bool wantToRemoveTag;
        private bool wantToChangeOwner;
        private bool wantToRemoveOwner;
        private bool wantToCheckTag;
        private bool wantToAddMessage;
        private bool setSleep;

        public void AdminCLI()
        {
            String input = "";

            Console.WriteLine("Welcome to Door-CLI");
            while (!input.Equals("exit"))
            {
                if (!isBusy())
                {
                    Console.Out.WriteAsync("$ ");
                    input = Console.ReadLine();

                    switch (input)
                    {
                        case "addtag":
                            wantToAddTag = true;
                            break;
                        case "removetag":
                            wantToRemoveTag = true;
                            break;
                        case "removeowner":
                            wantToRemoveOwner = true;
                            break;
                        case "changeowner":
                            wantToChangeOwner = true;
                            break;
                        case "checktag":
                            wantToCheckTag = true;
                            break;
                        case "debug":
                            Logger.Instance.ToggleDebugMode();
                            break;
                        case "log":
                            Logger.Instance.ToggleLogMode();
                            break;
                        case "addmessage":
                            wantToAddMessage = true;
                            break;
                        case "open":
                            new Thread(new ThreadStart(MotorController.Instance.getHandler().UnlockDoor)).Start();
                            break;
                        case "close":
                            new Thread(new ThreadStart(MotorController.Instance.getHandler().LockDoor)).Start();
                            break;
                        case "sleep":
                            DisplayController.Instance.getHandler().setSleepIcon(setSleep);
                            setSleep = !setSleep;
                            break;
                        default:
                            break;
                    }
                }
                Thread.Sleep(20);
            }

        }

        public bool isBusy()
        {
            if (wantToAddTag ||
                wantToRemoveTag ||
                wantToRemoveOwner ||
                wantToChangeOwner ||
                wantToCheckTag ||
                wantToAddMessage )
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void OnNext(Package value)
        {
            switch (value.type)
            {
                case packageType.motorPackageLocked:
                    break;
                case packageType.motorPackageUnlocked:
                    break;
                case packageType.RfidPackageFound:
                    break;
                case packageType.RfidPackageLost:
                    RegisterTag(value);
                    RemoveTag(value);
                    RemoveOwner(value);
                    ChangeOwner(value);
                    CheckTagOwner(value);
                    AddMessage(value);
                    break;
                default:
                    throw new Exception("Unknown Package");
            }
        }

        private void AddMessage(Package value)
        {
            if (wantToAddMessage)
            {
                if(SecurityController.Instance.IsSecureRFIDTag(value.message)){
                    Person person = SecurityController.Instance.RetrievePersonByTag(value.message);
                    Console.WriteLine("Enter message : ");
                    person.addMessage(Console.ReadLine());
                    Console.WriteLine("Done");
                    wantToAddMessage = false;
                }
            }
        }

        private void RegisterTag(Package value)
        {
            if (wantToAddTag)
            {
                SecurityController.Instance.addTag(value.message);
                wantToAddTag = false;
            }
        }

        private void RemoveTag(Package value)
        {
            if (wantToRemoveTag)
            {
                SecurityController.Instance.RemovePersonTag(value.message);
                wantToRemoveTag = false;
            }
        }

        private void RemoveOwner(Package value)
        {
            if (wantToRemoveOwner)
            {
                String input = Console.ReadLine();
                SecurityController.Instance.removeTagOwner(input);
                wantToRemoveOwner = false;
            }
        }

        private void ChangeOwner(Package value)
        {
            if (wantToChangeOwner)
            {
                String input = Console.ReadLine();
                SecurityController.Instance.addTag(input);
                wantToChangeOwner = false;
            }
        }

        private void CheckTagOwner(Package value)
        {
            if (wantToCheckTag)
            {
                Person owner = SecurityController.Instance.RetrievePersonByTag(value.message);

                Console.WriteLine("\n[" + value.message + "]");
                if (owner == null)
                    Console.WriteLine(Person.UnkownToString());
                else
                    Console.WriteLine(owner.ToString());

                wantToCheckTag = false;
            }
        }

        public void OnCompleted()
        {
            /** ignore this */
        }

        public void OnError(Exception error)
        {
            /** ignore this */
        }
    }

}
