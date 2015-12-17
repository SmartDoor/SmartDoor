using System;
using Phidgets;
using Phidgets.Events;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;

namespace SmartDoor.Controllers
{   
    /// <summary>
    /// Handles the LCD screen display.
    /// </summary>
    class LCDHandler : Component
    {
        public const int LOCKED_ICON_INDEX = 0;
        public const int UNLOCKED_ICON_INDEX = 1;
        public const int SLEEP_ICON_INDEX = 2;
        public const int EMPTY_ICON_INDEX = 3;

        private TextLCD lcdAdapter;
        private TextLCDScreen screen;

        private TextLCDCustomCharacter doorStatus;
        private TextLCDCustomCharacter sleepStatus;
        private TextLCDCustomCharacter empty;

        private String lastSecondRow { get; set; }
        private String lastFirstRow { get; set; }
        private String currentDate;

        /// <summary>
        /// Creates a new LCD Display.
        /// </summary>
        public LCDHandler()
        {
            lcdAdapter = new TextLCD();
            lastSecondRow = "";
            lastFirstRow = "";
            currentDate = string.Format("{0:HH:mm}", DateTime.Now);
        }

        /// <summary>
        /// Handles the shutdown of the program. Will close
        /// the stream to the LCD display.
        /// </summary>
        public void Shutdown()
        {
            displayStatus(true);
            lcdAdapter.close();
        }

        /// <summary>
        /// Displays a message on the LCD display.
        /// </summary>
        /// <param name="firstRow">String to be displayed on the first row of the LCD
        /// display.</param>
        /// <param name="secondRow">String to be displayed on the second row of the
        /// LCD display.</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void showMessage(String firstRow, String secondRow)
        {
            if(lcdAdapter.Attached)
            {
                if (screen.rows.Count > 1)
                {
                    if(firstRow.Length > 17)
                        firstRow = firstRow.Substring(0, 17);

                    if(secondRow.Length > 13)
                        secondRow = secondRow.Substring(0, 14);

                    firstRow = firstRow.PadRight(18) + sleepStatus.StringCode + doorStatus.StringCode;
                    secondRow = secondRow.PadRight(15) + string.Format("{0:HH:mm}", DateTime.Now);

                    screen.rows[0].DisplayString = firstRow;
                    screen.rows[1].DisplayString = secondRow;

                    lastFirstRow = firstRow;
                    lastSecondRow = secondRow;
                }
            }
        }

        /// <summary>
        /// Shows a message on the LCD Screen
        /// </summary>
        /// <param name="message"></param>
        public void showMessage(String message)
        {
            if (lcdAdapter.Attached)
            {
                if (screen.rows.Count > 1)
                {
                    lastSecondRow = lastSecondRow.Substring(0, 14);

                    if (message.Length > 13)
                        message = message.Substring(0, 14);

                    lastSecondRow = lastSecondRow.PadRight(18) + sleepStatus.StringCode + doorStatus.StringCode;
                    message = message.PadRight(15) + string.Format("{0:HH:mm}", DateTime.Now);

                    screen.rows[0].DisplayString = lastSecondRow;
                    screen.rows[1].DisplayString = message;

                    lastFirstRow = lastSecondRow;
                    lastSecondRow = message;
                }
            }
        }

        /// <summary>
        /// Forces a refresh of the LCD display with the latest data that
        /// the LCD is displaying such as datetime.
        /// </summary>
        public void updateTime()
        {
            if (currentDate == string.Format("{0:HH:mm}", DateTime.Now))
            {
                currentDate = string.Format("{0:HH:mm}", DateTime.Now);
                showMessage(lastFirstRow, lastSecondRow);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="character"></param>
        public void showMessage(String row, int character)
        {
            showMessage(row, screen.customCharacters[character].StringCode);
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
        /// <param name="status"></param>
        public void setLockedIcon(bool status)
        {
            if(status == true)
                doorStatus = screen.customCharacters[LOCKED_ICON_INDEX];
            else
                doorStatus = screen.customCharacters[UNLOCKED_ICON_INDEX];

            showMessage(lastFirstRow, lastSecondRow);
        }


        /// <summary>
        /// 
        /// </summary>
        public void WaitForAttach()
        {

            // Attach the event handlers.
            lcdAdapter.Attach += new AttachEventHandler(lcd_Attach);
            lcdAdapter.Detach += new DetachEventHandler(lcd_Detach);
            lcdAdapter.Error += new ErrorEventHandler(lcd_Error);

            lcdAdapter.open();

            Console.Out.WriteLine("LCD Waiting for attachment...");
            lcdAdapter.waitForAttachment();

            screen = lcdAdapter.screens[1];
            screen.ScreenSize = TextLCD.ScreenSizes._2x40;
            screen.initialize();

            // Setup the custom characters.
            screen.customCharacters[LOCKED_ICON_INDEX].setCustomCharacter(574912, 32639);
            screen.customCharacters[UNLOCKED_ICON_INDEX].setCustomCharacter(38050, 32639);
            screen.customCharacters[SLEEP_ICON_INDEX].setCustomCharacter(36812, 424033);
            screen.customCharacters[EMPTY_ICON_INDEX].setCustomCharacter(0, 0);
            empty = screen.customCharacters[EMPTY_ICON_INDEX];
            sleepStatus = empty;
        }

        public void setSleepIcon(bool status)
        {
            if (status)
                sleepStatus = screen.customCharacters[SLEEP_ICON_INDEX];
            else
                sleepStatus = empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lcd_Attach(object sender, AttachEventArgs e)
        {
            TextLCD attached = (TextLCD)sender;
            string name = attached.Name;
            string serialNo = attached.SerialNumber.ToString();

            Console.WriteLine("TextLCD name:{0} serial No.: {1} Attached!", name,
                                    serialNo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lcd_Detach(object sender, DetachEventArgs e)
        {
            TextLCD detached = (TextLCD)sender;
            string name = detached.Name;
            string serialNo = detached.SerialNumber.ToString();

            Console.WriteLine("TextLCD name:{0} serial No.: {1} Detached!", name,
                                    serialNo);
        }

        /// <summary>
        /// Is called if an error occurs during runtime.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lcd_Error(object sender, ErrorEventArgs e)
        {
            Console.WriteLine("LCD Error: e.Description");
        }
    }
}
