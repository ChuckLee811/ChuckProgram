using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.IO;
using System.IO.Ports;
using Others;

namespace Arduino_Tool
{
    public class Arduino_Func
    {
        /** \brief SerialPort   */
        public static SerialPort arduino_port = null;

        /** \brief 開始自走   */
        public static bool isbtnFrontBackClick = false;

        /** \brief SystemTimer   */
        public static System.Timers.Timer RecviveWeightTimer = new System.Timers.Timer();

        public static string Arduino_Connection(string btnArduino, string txtArduinoPortNumber, string txtArduinoBaudRate)
        {
            try
            {
                if (btnArduino == "OpenArduino")
                {
                    arduino_port = new SerialPort(txtArduinoPortNumber, Convert.ToInt16(txtArduinoBaudRate), Parity.None, 8, StopBits.One);
                    arduino_port.ReadTimeout = 4000;
                    arduino_port.Open();
                    if (arduino_port != null)
                    {
                        if (RecviveWeightTimer.Enabled == false)//開啟重量timer
                        {
                            if (RecviveWeightTimer.Interval != 333)
                            {
                                RecviveWeightTimer.Interval = 333;
                                RecviveWeightTimer.Elapsed += new System.Timers.ElapsedEventHandler(ReceiveWeight);
                            }
                            RecviveWeightTimer.Enabled = true;
                        }
                        return "CloseArduino";
                    }
                    else
                    {
                        return "0";
                    }
                }
                else
                {
                    RecviveWeightTimer.Enabled = false;
                    arduino_port.Close();
                    arduino_port = null;
                    return "OpenArduino";
                }
            }
            catch (Exception)
            {
                return "0";
            }

        }

        unsafe public static void ReceiveWeight(object sender, EventArgs e)//Timer 收高度資料
        {
            try
            {
                if (arduino_port != null && !isbtnFrontBackClick)
                {
                    arduino_port.Write("5");
                    string Receivedata = arduino_port.ReadLine();
                    int ReceiveValue = 0;
                    bool CheckValue = int.TryParse(Receivedata, out ReceiveValue);
                    if (CheckValue)
                    {
                        //int ReceiveValue = Convert.ToInt16(Receivedata);
                        GlobalVar.WeightVoltage = ((float)ReceiveValue / (float)1204) * 5;
                        // UpdateWeightUI(GlobalVar.WeightVoltage, label_Weight);
                    }
                }
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.ToString());
            }
        }

        public static int Arduino_Front()
        {
            if (arduino_port != null)
            {
                isbtnFrontBackClick = true;
                arduino_port.Write("1");
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static int Arduino_Back()
        {
            if (arduino_port != null)
            {
                isbtnFrontBackClick = true;
                arduino_port.Write("2");
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
