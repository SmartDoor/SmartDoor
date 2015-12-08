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
        private SecurityController securityController;
        private Logger debugger = Logger.GetInstance();

        /** options */
        private bool wantToAddTag;
        private bool wantToRemoveTag;
        private bool wantToChangeOwner;
        private bool wantToRemoveOwner;
        private bool wantToCheckTag;

        public AdminController(MasterController masterController, SecurityController securityController)
        {
            this.securityController = securityController;

            masterController.motorHandler.Subscribe(this);
            masterController.rfidHandler.Subscribe(this);
            wantToAddTag = false;
            wantToRemoveTag = false;
            wantToRemoveOwner = false;
            wantToChangeOwner = false;
            wantToCheckTag = false;
        }

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
                            debugger.ToggleDebugMode();
                            break;
                        case "log":
                            debugger.ToggleLogMode();
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
                wantToCheckTag)
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
                    RegisterTag(value);
                    RemoveTag(value);
                    RemoveOwner(value);
                    ChangeOwner(value);
                    CheckTagOwner(value);
                    break;
                case packageType.RfidPackageLost:
                    break;
                default:
                    throw new Exception("Unknown Package");
            }
        }

        private void RegisterTag(Package value)
        {
            if (wantToAddTag)
            {
                securityController.addTag(value.message);
                wantToAddTag = false;
            }
        }

        private void RemoveTag(Package value)
        {
            if (wantToRemoveTag)
            {
                securityController.removeTag(value.message);
                wantToRemoveTag = false;
            }
        }

        private void RemoveOwner(Package value)
        {
            if (wantToRemoveOwner)
            {
                String input = Console.ReadLine();
                securityController.removeTagOwner(input);
                wantToRemoveOwner = false;
            }
        }

        private void ChangeOwner(Package value)
        {
            if (wantToChangeOwner)
            {
                String input = Console.ReadLine();
                securityController.addTag(input);
                wantToChangeOwner = false;
            }
        }

        private void CheckTagOwner(Package value)
        {
            if (wantToCheckTag)
            {
                Person owner = securityController.retrieveTag(value.message);

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
