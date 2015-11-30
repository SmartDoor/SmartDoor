using System;
using Phidgets;
using Phidgets.Events;

using System.Collections.Generic;
using SmartDoor.ComponentHandlers;

namespace SmartDoor
{
    class RFIDHandler : IObservable<Package>, Component
    {
        private RFID rfidReader;

        private List<IObserver<Package>> observers;

        /// <summary>
        /// 
        /// </summary>
        public RFIDHandler()
        {
            rfidReader = new RFID();

            //initialize our Phidgets RFID reader and hook the event handlers
            rfidReader.Attach += new AttachEventHandler(rfid_Attach);
            rfidReader.Detach += new DetachEventHandler(rfid_Detach);
            rfidReader.Error += new ErrorEventHandler(rfid_Error);

            rfidReader.Tag += new TagEventHandler(rfid_Tag);
            rfidReader.TagLost += new TagEventHandler(rfid_TagLost);

            observers = new List<IObserver<Package>>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void WaitForAttach()
        {
            try
            {
                rfidReader.open();

                //Wait for a Phidget RFID to be attached before doing anything with 
                //the object
                Console.WriteLine("RFIDHandler : waiting for attachment...");
                rfidReader.waitForAttachment();
            }
            catch(PhidgetException ex)
            {

                Console.WriteLine("RFIDHandler : A fatal error has occured:");
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.Description);

                Shutdown();
                Environment.Exit(-1);
            }

            //turn on the antenna and the led to show everything is working
            rfidReader.Antenna = true;
            rfidReader.LED = true;

            Console.Out.WriteLine("RFIDHandler : Done waiting for attachment!");
        }

        /// <summary>
        /// 
        /// </summary>
        public void Shutdown()
        {
            //turn off the led
            rfidReader.LED = false;

            //close the phidget and dispose of the object
            rfidReader.close();
        }

        private void rfid_TagLost(object sender, TagEventArgs e)
        {
            Package package = new Package(packageType.RfidPackageLost,e.Tag);
            foreach (var observer in observers)
                observer.OnNext(package);
        }

        private void rfid_Tag(object sender, TagEventArgs e)
        {
            Package package = new Package(packageType.RfidPackageFound, e.Tag);
            foreach (var observer in observers)
                observer.OnNext(package);

        }

        private void rfid_Error(object sender, ErrorEventArgs e)
        {
            Console.WriteLine("RfidReader an error has occured");
        }

        private void rfid_Detach(object sender, DetachEventArgs e)
        {
            Console.WriteLine("RfidReader detached");
        }

        private void rfid_Attach(object sender, AttachEventArgs e)
        {
            Console.WriteLine("RfidReader attached");
        }

        public IDisposable Subscribe(IObserver<Package> observer)
        {
            // Check whether observer is already registered. If not, add it
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }
            return new Unsubscriber<Package>(observers, observer);
        }
    }
}
