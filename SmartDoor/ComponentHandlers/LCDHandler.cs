using System;
using Phidgets;
using Phidgets.Events;
using System.Collections;

namespace SmartDoor.ComponentHandlers
{   
    /// <summary>
    /// Handles the LCD screen display.
    /// </summary>
    class LCDHandler : Component
    {
        private TextLCD lcdAdapter;
        private TextLCDScreen screen;

        /// <summary>
        /// Creates a new LCD Display.
        /// </summary>
        public LCDHandler()
        {
            lcdAdapter = new TextLCD();
        }

        /// <summary>
        /// Handles the shutdown of the program. Will close
        /// the stream to the LCD display.
        /// </summary>
        public void Shutdown()
        {
            lcdAdapter.close();
        }

        /// <summary>
        /// Displays a message on the LCD display.
        /// </summary>
        /// <param name="firstRow"></param>
        /// <param name="secondRow"></param>
        public void showMessage(String firstRow, String secondRow)
        {
            if(lcdAdapter.Attached)
            {
                Console.WriteLine("rows : " + screen.rows.Count);
                if (screen.rows.Count > 1)
                {
                    screen.rows[0].DisplayString = firstRow;

                    screen.rows[1].DisplayString = secondRow;
                }
            }
        }

        /// <summary>
        /// Toggles the backlight lighting of the LCD display.
        /// </summary>
        /// <param name="status"></param>
        public void displayStatus(bool status)
        {
            lcdAdapter.Backlight = status;
            screen.Backlight = status;
        }


        /// <summary>
        /// 
        /// </summary>
        public void WaitForAttach()
        {

            lcdAdapter.Attach += new AttachEventHandler(lcd_Attach);
            lcdAdapter.Detach += new DetachEventHandler(lcd_Detach);
            lcdAdapter.Error += new ErrorEventHandler(lcd_Error);

            lcdAdapter.open();

            Console.Out.WriteLine("LCD Waiting for attachment...");
            lcdAdapter.waitForAttachment();

            screen = lcdAdapter.screens[1];
            screen.ScreenSize = TextLCD.ScreenSizes._2x40;
            screen.initialize();
        }

        private void lcd_Attach(object sender, AttachEventArgs e)
        {
            TextLCD attached = (TextLCD)sender;
            string name = attached.Name;
            string serialNo = attached.SerialNumber.ToString();

            Console.WriteLine("TextLCD name:{0} serial No.: {1} Attached!", name,
                                    serialNo);
        }

        private void lcd_Detach(object sender, DetachEventArgs e)
        {
            TextLCD detached = (TextLCD)sender;
            string name = detached.Name;
            string serialNo = detached.SerialNumber.ToString();

            Console.WriteLine("TextLCD name:{0} serial No.: {1} Detached!", name,
                                    serialNo);
        }

        private void lcd_Error(object sender, ErrorEventArgs e)
        {
            Console.WriteLine("LCD Error: e.Description");
        }
    }
}
