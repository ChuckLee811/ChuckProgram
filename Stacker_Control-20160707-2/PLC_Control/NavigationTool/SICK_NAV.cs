using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading;
using Others;

namespace NavigationTool
{
    public class SICK_NAV
    {
        /** \brief 存放NAV-TimeStamp   */
        public static string NAVTimeStamp = "";

        /** \brief X方向速度，要傳送給機器   */
        public int NAVSpeed_X = 0;

        /** \brief Y方向速度，要傳送給機器   */
        public int NAVSpeed_Y = 0;

        /** \brief 轉角速度，要傳送給機器   */
        public int NAVSpeedDirection = 0;

        /** \brief 當前座標資訊   */
        public NavigationInfo CrrentLocation = new NavigationInfo();

        /** \brief //右前端座標資訊(旋轉後)   */
        public NavigationInfo TFrontRightLocation = new NavigationInfo();

        /** \brief //左前端座標資訊(旋轉後)   */
        public NavigationInfo TFrontLeftLocation = new NavigationInfo();

        /** \brief 暫存上次的位置   */
        public NavigationInfo LastLocation = new NavigationInfo();

        /** \brief 兩輪中心座標(旋轉後)   */
        public NavigationInfo TFrontCenterLocation = new NavigationInfo();

        /** \brief NAV TimeStamp資訊   */
        public string NavTimeStamp = "";

        /** \brief NAV Sock宣告   */
        public Socket sender_TCP;

        /** \brief NAV接收資料buffer   */
        public byte[] d_bytes = new byte[2048];

        /** \brief NAV350收資料Timer   */
        public System.Timers.Timer TimerNavGetLocation = new System.Timers.Timer();

        /// <summary>
        /// NAV連線
        /// </summary>
        /// <param name="txtNAVIP">[IN] NAV IP</param>
        /// <param name="txtNAVPort">[IN] NAVPort</param>
        /// <returns>連線成功回傳"連線成功"，否則回傳"連線失敗"</returns>
        public string NAVConnect(string txtNAVIP, string txtNAVPort)
        {
            //執行連線
            try
            {
                if (ConnectNAV(txtNAVIP, txtNAVPort))
                {
                    //開執行序監聽資料
                    Thread DoThread = new Thread(new ParameterizedThreadStart(DoNavFunction));
                    DoThread.Start();
                    return "連線成功";
                }
                else return "連線失敗";
            }
            catch
            {
                return "連線失敗";
            }
        }

        /// <summary>
        /// 執行NAV斷線
        /// </summary>
        /// <returns>連線成功回傳"斷線成功"值，否則回傳"斷線失敗"</returns>
        public string NAVDisConnect()
        {
            //執行斷線
            if (sender_TCP == null) return "斷線失敗";
            if (sender_TCP.Connected)
            {
                sender_TCP.Shutdown(SocketShutdown.Both);
                sender_TCP.Close();
                sender_TCP = null;
                //txtNavReceive.Text = DateTime.Now.ToString() + ", 斷線成功\r\n" + txtNavReceive.Text;
                return "斷線成功";
            }
            else
            {
                return "斷線失敗";
            }
        }

        /// <summary>
        /// 監聽回傳資料
        /// </summary>
        /// <param name="num">無使用</param>
        public void DoNavFunction(object num) //監聽回傳資料
        {
            if (sender_TCP.Connected)
            {
                while (true && sender_TCP != null)
                {
                    int bytesRec = sender_TCP.Receive(d_bytes);
                    string Receive = Encoding.ASCII.GetString(d_bytes, 0, bytesRec);
                    if (Receive == "") return;

                    CheckReceiveData(Receive);

                    //車輛後輪中心點位置
                    GlobalVar.MotorPosition = CrrentLocation;

                    //車輛前輪中心點位置
                    TFrontCenterLocation.LocationX = CrrentLocation.LocationX + 1500;
                    TFrontCenterLocation.LocationY = CrrentLocation.LocationY;
                    TrasformCoordinate(CrrentLocation, TFrontCenterLocation, TFrontCenterLocation, CrrentLocation.Direction);
                    TFrontCenterLocation.Direction = CrrentLocation.Direction;
                    GlobalVar.CurrentPosition = TFrontCenterLocation;

                    //右前輪中心點位置
                    TFrontRightLocation.LocationX = CrrentLocation.LocationX + 1500;
                    TFrontRightLocation.LocationY = CrrentLocation.LocationY - 600;
                    TrasformCoordinate(CrrentLocation, TFrontRightLocation, TFrontRightLocation, CrrentLocation.Direction);
                    GlobalVar.CarTirepositionR = TFrontRightLocation;

                    //左前輪中心點位置
                    TFrontLeftLocation.LocationX = CrrentLocation.LocationX + 1500;
                    TFrontLeftLocation.LocationY = CrrentLocation.LocationY + 600;
                    TrasformCoordinate(CrrentLocation, TFrontLeftLocation, TFrontLeftLocation, CrrentLocation.Direction);
                    GlobalVar.CarTirepositionL = TFrontLeftLocation;

                    //NAV TimeStamp資訊  
                    NAVTimeStamp = NavTimeStamp;

                    if (NAVTimeStamp != "")
                    {
                        NAVTimeStamp = "0x" + NAVTimeStamp.ToString();
                        NAVTimeStamp = NAVTimeStamp.Substring(0, NAVTimeStamp.Length - 1);
                    }
                    if (Receive.IndexOf("sAN mNPOSGetPose") != -1)
                    {
                        NAVSpeed_X = (CrrentLocation.LocationX - LastLocation.LocationX) * 7;
                        NAVSpeed_Y = (CrrentLocation.LocationY - LastLocation.LocationY) * 7;

                        int CrrDir = CrrentLocation.Direction;
                        int LasDir = LastLocation.Direction;
                        if (CrrDir > 180) CrrDir = -(360 - CrrDir);
                        if (LasDir > 180) LasDir = -(360 - LasDir);
                        NAVSpeedDirection = (CrrDir - LasDir) * 7000;

                        LastLocation.Direction = CrrentLocation.Direction;
                        LastLocation.LocationX = CrrentLocation.LocationX;
                        LastLocation.LocationY = CrrentLocation.LocationY;
                    }
                    //紀錄收到Sensor資料的時間
                    GlobalVar.NavTimeStamp = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// 堆高機座標推算
        /// </summary>
        /// <param name="Ori">[IN] 輸入座標</param>
        /// <param name="src">[IN] 輸入計算座標</param>
        /// <param name="dst">[OUT] 回傳計算結果座標</param>
        /// <param name="degrees">角度</param>
        public void TrasformCoordinate(NavigationInfo Ori, NavigationInfo src, NavigationInfo dst, int degrees)
        {
            NavigationInfo temp = new NavigationInfo();
            temp.LocationX = src.LocationX;
            temp.LocationY = src.LocationY;

            //座標旋轉
            int degr = degrees;
            if (degr > 180) degr = 360 - degr;
            else degr = -degr;
            double angle = Math.PI * degr / 180.0;
            double sinAngle = Math.Sin(angle);
            double cosAngle = Math.Cos(angle);

            dst.LocationX = (int)((double)(temp.LocationX - Ori.LocationX) * cosAngle + (double)(temp.LocationY - Ori.LocationY) * sinAngle) + Ori.LocationX;
            dst.LocationY = (int)((double)-(temp.LocationX - Ori.LocationX) * sinAngle + (double)(temp.LocationY - Ori.LocationY) * cosAngle) + Ori.LocationY;
        }

        /// <summary>
        /// NAV連線
        /// </summary>
        /// <param name="strIP">[IN] 輸入IP</param>
        /// <param name="strPort">[IN] 輸入Port</param>
        /// <returns>連線成功回傳true，否則回傳false</returns>
        public bool ConnectNAV(string strIP, string strPort)
        {
            try
            {
                if (strIP != null && strPort != null)           
                {
                    //New資訊空間
                    CrrentLocation = new NavigationInfo();
                    TFrontRightLocation = new NavigationInfo();
                    int port = Convert.ToInt16(strPort);
                    //開始連線
                    sender_TCP = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    sender_TCP.Connect(strIP, port);
                    if (sender_TCP.Connected) return true;
                    else return false;
                }
                else return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 開啟NAV接收座標timer連續或關閉
        /// </summary>
        /// <returns>回傳下一次執行按鈕的文字</returns>
        public string ContinueLocation()
        {
            if (!sender_TCP.Connected)
            {
                MessageBox.Show("請先進行網路連線");
                return "0";
            }

            if (TimerNavGetLocation.Enabled == false)//開啟NAV接收座標timer
            {
                if (TimerNavGetLocation.Interval != 150)
                {
                    TimerNavGetLocation.Interval = 150;
                    TimerNavGetLocation.Elapsed += new System.Timers.ElapsedEventHandler(GetNavigationInfo);
                }
                TimerNavGetLocation.Enabled = true;
                return "關閉連續";
            }
            else
            {
                TimerNavGetLocation.Enabled = false;
                return "開啟連續";
            }
        }

        /// <summary>
        /// 讀取NAV回傳座標資訊
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetNavigationInfo(object sender, EventArgs e)
        {
            GetNavigationInfo();//讀取NAV回傳座標資訊
        }

        /// <summary>
        /// 解析NAV回傳資料
        /// </summary>
        /// <param name="Receive">[IN] 資料字串</param>
        public void CheckReceiveData(string Receive)
        {
            //分析座標資訊
            if (Receive.IndexOf("sAN mNPOSGetPose") != -1)
            {
                //字串用空格隔開
                string[] EachData = Receive.Split(' ');
                if (EachData.Length < 4) return;

                //檢查錯誤碼
                string ErrorCode = EachData[3];
                if (ErrorCode != "0")
                {
                    //有錯誤 解析錯誤碼
                    Console.WriteLine(ChecNavkErrorCode(ErrorCode));
                    Console.WriteLine(Receive);
                    Console.WriteLine("------------------------------------------");
                    return;
                }
                if (EachData.Length < 7) return;

                //取得座標資訊
                int Phi = Convert.ToInt32(EachData[8], 16);                 //解析角度
                float Direction = (float)Phi / (float)1000;                 //角度單位換算
                NavigationInfo ReceiveInfo = new NavigationInfo();          //New資訊空間
                ReceiveInfo.LocationX = Convert.ToInt32(EachData[6], 16);   //解析X座標
                ReceiveInfo.LocationY = Convert.ToInt32(EachData[7], 16);   //解析Y座標
                ReceiveInfo.Direction = (int)Direction;                     //解析角度
                CrrentLocation = ReceiveInfo;                               //記錄當前座標
            }

            if (Receive.IndexOf("sAN mNAVGetTimestamp") != -1)
            {
                //字串用空格隔開
                string[] EachData = Receive.Split(' ');
                if (EachData.Length < 3) return;

                //檢查錯誤碼
                string ErrorCode = EachData[2];
                if (ErrorCode != "0")
                {
                    //有錯誤 解析錯誤碼
                    Console.WriteLine(ChecNavkErrorCode(ErrorCode));
                    return;
                }
                //解析NAV回傳之TimeStamp
                string StampData = EachData[3];
                if (StampData != "") NavTimeStamp = StampData;
            }

            if (Receive.IndexOf("sAN mNPOSSetSpeed") != -1)
            {
                //Console.WriteLine(Receive.ToString());
            }
        }

        /// <summary>
        /// 顯示錯誤識別碼
        /// </summary>
        /// <param name="ErrorCode">[IN] 錯誤代碼</param>
        /// <returns>回傳錯誤訊息</returns>
        public string ChecNavkErrorCode(string ErrorCode)
        {
            //錯誤碼識別
            if (ErrorCode == "1") return "Wrong Operating Mode";
            else if (ErrorCode == "2") return "Asynchrony Method Terminated";
            else if (ErrorCode == "3") return "Invalid Data";
            else if (ErrorCode == "4") return "No Position Available";
            else if (ErrorCode == "5") return "Timeout";
            else if (ErrorCode == "6") return "Method Already Active";
            else return "general error";
        }

        /// <summary>
        /// 傳送至NAV350
        /// </summary>
        /// <param name="Command">[IN] 要傳送之指令字串</param>
        /// <returns>傳送成功回傳true，否則回傳false</returns>
        public bool SendToNAV350(string Command)   //傳送至NAV350
        {
            if (sender_TCP == null)
            {
                MessageBox.Show("請先與機器取得連線");              //沒有連線
                return false;
            }
            if (sender_TCP.Connected && Command != "")
            {
                try
                {
                    byte[] msg = Encoding.ASCII.GetBytes(Command);  //將Command轉換成Byte
                    byte[] SendByte = new byte[msg.Length + 2];     //宣告傳送buffer
                    SendByte[0] = 0x02;                             //設定標頭STX
                    SendByte[msg.Length + 1] = 0x03;                //設定標尾EDX
                    Array.Copy(msg, 0, SendByte, 1, msg.Length);    //複製中間資料
                    int bytesSent = sender_TCP.Send(SendByte);      //傳送
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());               //傳送失敗
                    return false;
                }
            }
            else return false;
        }

        public void Mode_PowerDown()
        {
            //切換至PowerDown模式
            if (!SendToNAV350("sMN mNEVAChangeState 0")) MessageBox.Show("傳送失敗，請檢察");
        }

        public void Mode_Standby()
        {
            //切換至Standby模式
            if (!SendToNAV350("sMN mNEVAChangeState 1")) MessageBox.Show("傳送失敗，請檢察");
        }

        public void Mode_LandMark()
        {
            //切換至LandMark Detection模式
            if (!SendToNAV350("sMN mNEVAChangeState 3")) MessageBox.Show("傳送失敗，請檢察");
        }

        public void Mode_Mapping()
        {
            //切換至Mapping模式
            if (!SendToNAV350("sMN mNEVAChangeState 2")) MessageBox.Show("傳送失敗，請檢察");
        }

        public void Mode_Navigation()
        {
            //切換至Navigation模式
            if (!SendToNAV350("sMN mNEVAChangeState 4")) MessageBox.Show("傳送失敗，請檢察");
        }

        public void SetUserLevel()
        {
            //開啟使用者權限
            if (!SendToNAV350("sMN SetAccessMode 3 F4724744")) MessageBox.Show("傳送失敗，請檢察");
        }

        public void GetLocation()
        {
            //取得定位及角度資訊
            if (!SendToNAV350("sMN mNPOSGetPose 0")) MessageBox.Show("傳送失敗，請檢察");
        }

        public void GetNavigationInfo()
        {
            //取得定位及角度資訊
            SendToNAV350("sMN mNPOSGetPose 0");
        }

        public void SetTimeSync()
        {
            //設定時間同步TimeSync
            SendToNAV350("sWN NAVHardwareTimeSync 1 15");
        }

        public void GetTimeStamp()
        {
            //抓取機器的TimeStamp
            SendToNAV350("sMN mNAVGetTimestamp");
        }

        /// <summary>
        /// 抓取機器的TimeStamp
        /// </summary>
        /// <param name="XSpeed">X軸速度</param>
        /// <param name="YSpeed">Y軸速度</param>
        /// <param name="DirSpeed">角速度</param>
        /// <param name="TimeStamp">NAV時間</param>
        public void SendVelocity(int XSpeed,int YSpeed, int DirSpeed, int TimeStamp)
        {
            //抓取機器的TimeStamp
            //SendToNAV350("sMN mNPOSSetSpeed 0 0 0 " + TimeStamp + " 1");
            string strX = "";
            string strY = "";
            string strDir = "";
            if (XSpeed >= 0) strX = "+" + XSpeed.ToString();
            else strX = "-" + XSpeed.ToString();
            if (YSpeed >= 0) strY = "+" + YSpeed.ToString();
            else strY = "-" + YSpeed.ToString();
            if (DirSpeed >= 0) strDir = "+" + DirSpeed.ToString();
            else strDir = "-" + DirSpeed.ToString();
            SendToNAV350("sMN mNPOSSetSpeed " + strX + " " + strY + " " + strDir + " " + TimeStamp + " 1");
        }
    }
}