using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartDoor.ComponentHandlers;
using System.Threading;

namespace ComponentTests
{
    [TestClass]
    public class LCDHandlerTest
    {

        private LCDHandler lcdHandler;

        [TestMethod]
        public void TestShowMessage()
        {
            lcdHandler = new LCDHandler();

            lcdHandler.WaitForAttach();
            Console.Out.Write("Waiting for attachment in test\n");
            Thread.Sleep(1000);

            lcdHandler.displayStatus(true);

            lcdHandler.showMessage("HEJ", "HEJ2");

            Console.In.ReadLine();

            lcdHandler.displayStatus(false);

            lcdHandler.Shutdown();

        }
    }
}
