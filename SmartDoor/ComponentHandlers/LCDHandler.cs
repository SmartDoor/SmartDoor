﻿using System;
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
        public const int UNLOCKED_ICON_INDEX = 1;
        public const int LOCKED_ICON_INDEX = 0;

        private TextLCD lcdAdapter;
        private TextLCDScreen screen;

        private TextLCDCustomCharacter doorStatus;

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

                    firstRow = firstRow.PadRight(19) + doorStatus.StringCode;
                    secondRow = secondRow.PadRight(15) + string.Format("{0:HH:mm}", DateTime.Now);

                    screen.rows[0].DisplayString = firstRow;
                    screen.rows[1].DisplayString = secondRow;

                    lastFirstRow = firstRow;
                    lastSecondRow = secondRow;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstRow"></param>
        /// <param name="secondRow"></param>
        public void showMessage(String message)
        {
            if (lcdAdapter.Attached)
            {
                if (screen.rows.Count > 1)
                {
                    lastSecondRow = lastSecondRow.Substring(0, 14);

                    if (message.Length > 13)
                        message = message.Substring(0, 14);

                    lastSecondRow = lastSecondRow.PadRight(19) + doorStatus.StringCode;
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

        public void setLockedIcon(bool status)
        {
            if(status == true)
            {
                doorStatus = screen.customCharacters[LOCKED_ICON_INDEX];
            } else
            {
                doorStatus = screen.customCharacters[UNLOCKED_ICON_INDEX];
            }
            showMessage(lastFirstRow, lastSecondRow);
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

            screen.customCharacters[LOCKED_ICON_INDEX].setCustomCharacter(574912, 32639);
            screen.customCharacters[UNLOCKED_ICON_INDEX].setCustomCharacter(38050, 32639);
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