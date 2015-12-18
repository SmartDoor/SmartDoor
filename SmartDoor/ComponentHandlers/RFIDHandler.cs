using System;
using Phidgets;
using Phidgets.Events;
using System.Collections.Generic;
using SmartDoor.Controllers;
using SmartDoor.Utilities;

namespace SmartDoor
{
    class RFIDHandler : IObservable<Package>, Component
    {
        private RFID rfidReader;
        private RFID rfidReader2;
        private List<IObserver<Package>> observers;

        /// <summary>
        /// 
        /// </summary>
        public RFIDHandler()
        {
            rfidReader = new RFID();
            rfidReader2 = new RFID();
            //initialize our Phidgets RFID reader and hook the event handlers
            rfidReader.Attach += new AttachEventHandler(rfid_Attach);
            rfidReader2.Attach += new AttachEventHandler(rfid_Attach);

            rfidReader.Detach += new DetachEventHandler(rfid_Detach);
            rfidReader2.Detach += new DetachEventHandler(rfid_Detach);

            rfidReader.Error += new ErrorEventHandler(rfid_Error);
            rfidReader2.Error += new ErrorEventHandler(rfid_Error);

            rfidReader.Tag += new TagEventHandler(rfid_Tag);
            rfidReader2.Tag += new TagEventHandler(rfid_Tag);

            rfidReader.TagLost += new TagEventHandler(rfid_TagLost);
            rfidReader2.TagLost += new TagEventHandler(rfid_TagLost);

            observers = new List<IObserver<Package>>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void WaitForAttach()
        {
            try
            {
                rfidReader.open(332857);
                rfidReader2.open(309394);
                //Wait for a Phidget RFID to be attached before doing anything with 
                //the object
                Console.WriteLine("RFIDHandler : waiting for attachment...");
                rfidReader.waitForAttachment();
                rfidReader2.waitForAttachment();
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
            rfidReader2.Antenna = true;
            rfidReader.LED = true;
            rfidReader2.LED = false;

            Console.Out.WriteLine("RFIDHandler : Done waiting for attachment!");
        }

        /// <summary>
        /// 
        /// </summary>
        public void Shutdown()
        {
            //close the phidget and dispose of the object
            rfidReader.close();
            rfidReader2.close();
        }

        private void rfid_TagLost(object sender, TagEventArgs e)
        {
            Package package = new Package(packageType.RfidPackageLost,e.Tag);
            foreach (var observer in observers)
                observer.OnNext(package);
            rfidReader2.LED = false;
        }

        private void rfid_Tag(object sender, TagEventArgs e)
        {
            Package package = new Package(packageType.RfidPackageFound, e.Tag);
            foreach (var observer in observers)
                observer.OnNext(package);

            rfidReader2.LED = true;
        }

        private void rfid_Error(object sender, ErrorEventArgs e)
        {
            Logger.DebugErr("RfidReader an error has occured");
        }

        private void rfid_Detach(object sender, DetachEventArgs e)
        {
            Logger.DebugErr("RfidReader detached");
        }

        private void rfid_Attach(object sender, AttachEventArgs e)
        {
            Logger.DebugErr("RfidReader attached");
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
