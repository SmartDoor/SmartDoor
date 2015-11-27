using SmartDoor.ComponentHandlers;
using SmartDoor.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDoor.Controllers
{
    class AdminController : IObserver<Package>
    {
        private SecurityController securityController;

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
                    case "changeOwner":
                        wantToChangeOwner = true;
                        break;
                    case "checktag":
                        wantToCheckTag = true;
                        break;
                    default:
                        break;
                }

            }

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

                Console.WriteLine("[" + value.message + "]");
                Console.WriteLine("Owner : " + (owner == null ? "unknown" :owner.name));
                Console.WriteLine("Email : " + (owner == null ? "unknown" : owner.email));
                Console.WriteLine("birthday : " + (owner == null ? "unknown" : owner.birthday + "\n"));
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
