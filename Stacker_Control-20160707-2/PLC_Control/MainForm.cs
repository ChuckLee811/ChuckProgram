using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using SharpGL;
using OpenGLTool;
using CanBusTool;
using LidarTool;
using NavigationTool;
using ObjectPLC;
using Others;
using AlgorithmTool;
using Arduino_Tool;

namespace PLC_Control
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            obj_PLC = new ObjectPLC_KV();
            obj_PLC.axDBCommManager = axDBCommManager_Detector;

            //堆高機初始設定
            ForkliftControl_Init();
        }

        #region FormFunction



        private void Form1_Load(object sender, EventArgs e)
        {
            LidarFun.Init();
            try
            {
                //讓Form在最上層
                this.TopMost = true;
                this.Focus();

                //初始化貨物、地圖設定，宣告rtAGV_Control
                InitConfig();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //初始化ShowLogDebugPanel大小
            panelDebug.Left = panelSetLocation.Left;
            panelDebug.Top = panelSetLocation.Top;
            panelDebug.Height = panelSetLocation.Height;
            panelDebug.Width = panelSetLocation.Width;

            panelDebug.Visible = false;


        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //關閉CanBus
            CanBusFunc.ResetCan();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //關閉程式
            Process.GetCurrentProcess().Kill();
            Application.Exit();
        }

        public void ForkliftControl_Init()
        {
            //堆高機控制
            btnOpenCan.Enabled = false;         //開啟CAN
            btnSendR.Enabled = false;           //逆時針
            btnSendL.Enabled = false;           //順時針
            btnOrigin.Enabled = false;          //原點復歸
            btnRelaxMotor.Enabled = false;      //放掉馬達
            btnMoveFront.Enabled = false;       //前進
            brnMoveBack.Enabled = false;        //後退
            btnClibratOrigin.Enabled = false;   //原點校正
            btnEmergencyStop.Enabled = false;   //緊急停止
            btnDisConnect.Enabled = false;      //PLC DisConnect
            btnClutch.Enabled = false;          //離合器
            btnBrakes.Enabled = false;          //煞車

            //貨叉控制
            btnUp.Enabled = false;       //上
            btnFront.Enabled = false;    //前
            btnOblique.Enabled = false;  //上傾斜
            btnLeft.Enabled = false;     //左
            btnDown.Enabled = false;     //下
            btnBack.Enabled = false;     //後
            btnSloping.Enabled = false;  //下傾斜
            btnRight.Enabled = false;    //右

            //SICK狀態
            btnNAVDisConnect.Enabled = false;    //斷線
            btn_Mode_Navigation.Enabled = false; //Navigation
            btnContinueLocation.Enabled = false; //連續座標
            btnStartServer.Enabled = false;      //Server連線
            btnContinue.Enabled = false;         //繼續
            btnPause.Enabled = false;            //暫停
            btnDoCmd.Enabled = false;            //執行
            button1.Enabled = false;             //RunDown1
            button2.Enabled = false;             //RunDown2
            button3.Enabled = false;             //RunDown3
            button4.Enabled = false;             //RunDown4
        }


        public void ForkliftControl_CanBusConnection()
        {
            Connection_Control_CanBus = true;   //CanBus開啟成功

            btnSendR.Enabled = true;           //逆時針
            btnSendL.Enabled = true;           //順時針
            btnOrigin.Enabled = true;          //原點復歸
            btnRelaxMotor.Enabled = true;      //放掉馬達

            if (Connection_Control_PLC == true) //如果PLC連線成功
            {
                btnClibratOrigin.Enabled = true;   //原點校正
                btnEmergencyStop.Enabled = true;   //緊急停止
            }
        }

        public void ForkliftControl_PLCConnection()
        {
            Connection_Control_PLC = true;  //PLC連線成功

            btnClutch.Enabled = true;          //離合器
            btnBrakes.Enabled = true;          //煞車
            //貨叉控制
            btnUp.Enabled = true;       //上
            btnFront.Enabled = true;    //前
            btnOblique.Enabled = true;  //上傾斜
            btnLeft.Enabled = true;     //左
            btnDown.Enabled = true;     //下
            btnBack.Enabled = true;     //後
            btnSloping.Enabled = true;  //下傾斜
            btnRight.Enabled = true;    //右
            btnDisConnect.Enabled = true;//PLC 可按斷線

            if (Connection_Control_CanBus == true)  //如果CanBus連線成功
            {
                btnClibratOrigin.Enabled = true;   //原點校正
                btnEmergencyStop.Enabled = true;   //緊急停止
            }
        }

        #endregion

        #region 變數宣告

        //PLC
        public static ObjectPLC_KV obj_PLC;

        //SystemTimer
        //public static System.Timers.Timer RecviveWeightTimer = new System.Timers.Timer();
        public static System.Timers.Timer TimerSetForkHeight = new System.Timers.Timer();         //貨叉高度設定(前進)
        public static System.Timers.Timer TimerAdjustOriginHighSpeed = new System.Timers.Timer(); //左右復歸校正Timer
        public static System.Timers.Timer TimerReceivePLC_Data = new System.Timers.Timer();

        //計算轉向誤差用
        private int AdjustOriginCounter = 0;
        private byte AdjustOriginTemp = 0;
        public static int H_Error = 0;
        public static int L_Error = 0;

        //更新UI
        private delegate void CallBackWeightUI(float Voltage, Label lblWeight);

        //CAN手自動切換
        private bool CANHandMode = false;

        //設置貨叉是否已在移動
        public static bool ForkisRunnung = false;

        //紀錄模擬行走的資料
        private List<string> LogCarInfo = new List<string>();
        private List<string> LogMainData = new List<string>();
        private List<string> LogDetailData = new List<string>();

        //存放座標資訊
        public static rtCarData LocateData = new rtCarData();

        //初始化時間
        private int btnFrontBack = 0;

        public rtAGV_Data a_tAGV_Data = new rtAGV_Data();

        //貨叉控制變數
        public static int Set_ForthDepthCount = 0;
        public static int Set_BackDepthCount = 0;

        //NAVClass用
        SICK_NAV NAVClass;

        //貨叉高度設定 
        public static int SetForkHeightValue = 0;

        //PLC連線狀態
        public bool Connection_Control_PLC = false;

        //CanBus連線狀態
        public bool Connection_Control_CanBus = false;

        //CanBus是否有連線成功(1為成功、0為失敗)
        public int CanConnect_Status = 0;

        //CanBus是否有開啟(1為開啟、0為尚未開啟)
        public int CanOpenStatus = 0;

        //SICK連線狀態
        public string Connection_Control_SICK = "0";

        //記錄初始游標位置(OpenGL使用)
        private Point _BakMousePosition = new Point(0, 0);

        //宣告LidarFunc
        LidarFunc LidarFun = new LidarFunc();

        #endregion

        #region Sensor接收

        private void btnArduino_Click(object sender, EventArgs e)//Arduino連接
        {
            Arduino_Tool.Arduino_Func.Arduino_Connection(btnArduino.Text, txtArduinoPortNumber.Text, txtArduinoBaudRate.Text);
        }

        private void UpdateWeightUI(float Voltage, Label lblWeight) //更新重量UI
        {
            if (this.InvokeRequired)
            {
                CallBackWeightUI DoUpdate = new CallBackWeightUI(UpdateWeightUI);
                this.Invoke(DoUpdate, Voltage, lblWeight);
            }
            else
            {
                lblWeight.Text = "目前重量: " + Voltage.ToString(); ;
            }
        }

        #endregion

        #region PLC及堆高機控制

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (obj_PLC.checkConnect(true, false) == true)
            {
                MessageBox.Show("已連線");
                return;
            }
            else
            {
                //未連線，進行連線
                if (obj_PLC.doMoniter() == false)
                {
                    return;
                }
                GlobalVar.isPLCConnect = true;

                if (TimerReceivePLC_Data.Enabled == false)//開啟讀取PLC資料
                {
                    if (TimerReceivePLC_Data.Interval != 30)
                    {
                        TimerReceivePLC_Data.Interval = 30;
                        TimerReceivePLC_Data.Elapsed += new System.Timers.ElapsedEventHandler(ReadPLC_Data);
                    }
                    TimerReceivePLC_Data.Enabled = true;
                }
                btnConnect.BackColor = System.Drawing.Color.Red;

                //PLC連線成功(開啟可使用的按鈕)
                ForkliftControl_PLCConnection();
            }
        }

        private void ReadPLC_Data(object sender, EventArgs e)
        {
            if (GlobalVar.islibratOrigin) return;
            if (DeliverData.tAGV_Data.ucAGV_Status != 0) return;
            GetFrontWheelSpeed();
        }

        private void btnDisConnect_Click(object sender, EventArgs e)
        {
            if (obj_PLC.checkConnect(true, false) == true)
            {
                TimerReceivePLC_Data.Enabled = false;
                //已連線，進行斷線
                obj_PLC.doDeMoniter();
                btnConnect.BackColor = SystemColors.Control;
            }
            else
            {
                MessageBox.Show("已斷線");
                return;
            }
        }

        private void btnEmergencyStop_Click(object sender, EventArgs e)
        {
            if (btnEmergencyStop.Text == "緊急停止")
            {
                //放油門
                MoveStop();

                //按剎車
                obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "5", 1600);
                obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "7", 1400);

                //結束DeliverThread執行緒
                if (DeliverThread != null)
                    DeliverThread.Abort();

                //解除自動駕駛
                btnEmergencyStop.Text = "解除狀態";
                btnEmergencyStop.BackColor = System.Drawing.Color.Red;
                TimerSetForkHeight.Enabled = false;
                DeliverFlowTimer.Enabled = false;
                obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "1", 2000);//貨插-停
                obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_MR, "111", 1);
            }
            else
            {
                //放煞車
                obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "5", 400);
                obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "7", 3600);
                obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_MR, "111", 0);
                btnEmergencyStop.Text = "緊急停止";
                btnEmergencyStop.BackColor = SystemColors.Control;
            }
        }

        private void btnSendR_MouseDown(object sender, MouseEventArgs e) //壓下順時針轉
        {
            btnSendR.BackColor = System.Drawing.Color.LightGreen;
            CanBusFunc.CANBUS_TurnRight = true;
        }

        private void btnSendR_MouseUp(object sender, MouseEventArgs e) //放開順時針轉
        {
            btnSendR.BackColor = SystemColors.Control;
            CanBusFunc.CANBUS_TurnRight = false;
        }

        private void btnSendL_MouseDown(object sender, MouseEventArgs e) //壓下逆時針轉
        {
            btnSendL.BackColor = System.Drawing.Color.LightGreen;
            CanBusFunc.CANBUS_TurnLeft = true;
        }

        private void btnSendL_MouseUp(object sender, MouseEventArgs e) //放開逆時針轉
        {
            btnSendL.BackColor = SystemColors.Control;
            CanBusFunc.CANBUS_TurnLeft = false;
        }

        private void btnUp_MouseDown(object sender, MouseEventArgs e)   //按下前端上升
        {
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "0", 1000);
            btnUp.BackColor = System.Drawing.Color.LightGreen;
        }

        private void btnUp_MouseUp(object sender, MouseEventArgs e)     //放開前端上升
        {
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "0", 2000);
            btnUp.BackColor = SystemColors.Control;
        }

        private void btnDown_MouseDown(object sender, MouseEventArgs e) //按下前端下降
        {
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "0", 3500);
            btnDown.BackColor = System.Drawing.Color.LightGreen;
            Button Temp = (Button)sender;
            Console.WriteLine(Temp.Text);
        }

        private void btnDown_MouseUp(object sender, MouseEventArgs e)   //放開前端下降
        {
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "0", 2000);
            btnDown.BackColor = SystemColors.Control;
        }

        private void btnFront_MouseDown(object sender, MouseEventArgs e) //按下前端向前
        {
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "1", 3250);
            btnFront.BackColor = System.Drawing.Color.LightGreen;
        }

        private void btnFront_MouseUp(object sender, MouseEventArgs e)  //放開前端向前
        {
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "1", 2000);
            btnFront.BackColor = SystemColors.Control;
        }

        private void btnBack_MouseDown(object sender, MouseEventArgs e)  //按下前端向後
        {
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "1", 750);
            btnBack.BackColor = System.Drawing.Color.LightGreen;
        }

        private void btnBack_MouseUp(object sender, MouseEventArgs e)  //放開前端向後
        {
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "1", 2000);
            btnBack.BackColor = SystemColors.Control;
        }

        private void btnLeft_MouseDown(object sender, MouseEventArgs e) //按下前端向左
        {
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "3", 3000);
            btnLeft.BackColor = System.Drawing.Color.LightGreen;
        }

        private void btnLeft_MouseUp(object sender, MouseEventArgs e)  //放開前端向左
        {
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "3", 2000);
            btnLeft.BackColor = SystemColors.Control;
        }

        private void btnRight_MouseDown(object sender, MouseEventArgs e) //按下前端向右
        {
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "3", 1000);
            btnRight.BackColor = System.Drawing.Color.LightGreen;
        }

        private void btnRight_MouseUp(object sender, MouseEventArgs e)  //放開前端向右
        {
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "3", 2000);
            btnRight.BackColor = SystemColors.Control;
        }

        private void btnOblique_MouseDown(object sender, MouseEventArgs e)//按下前端向上傾斜
        {
            btnOblique.BackColor = System.Drawing.Color.LightGreen;
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "2", 1000);
        }

        private void btnOblique_MouseUp(object sender, MouseEventArgs e)  //放開前段向上傾斜
        {
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "2", 2000);
            btnOblique.BackColor = SystemColors.Control;
        }

        private void btnSloping_MouseDown(object sender, MouseEventArgs e) //按下前端向下傾斜
        {
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "2", 3000);
            btnSloping.BackColor = System.Drawing.Color.LightGreen;
        }

        private void btnSloping_MouseUp(object sender, MouseEventArgs e)   //放開前端向下傾斜
        {
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "2", 2000);
            btnSloping.BackColor = SystemColors.Control;
        }

        private void btnClutch_Click(object sender, EventArgs e) //離合器
        {
            if (btnClutch.BackColor == System.Drawing.Color.LightGreen)
            {
                obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "6", 1440);
                btnClutch.BackColor = SystemColors.Control;
            }
            else
            {
                obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "6", 2200);
                btnClutch.BackColor = System.Drawing.Color.LightGreen;
            }
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_MR, "9003", 0);
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "4", 400);

            if (Connection_Control_CanBus == true && Connection_Control_PLC == true)//如果CanBus連線和PLC連線都成功
            {
                btnMoveFront.Enabled = true;       //前進
                brnMoveBack.Enabled = true;        //後退
            }
        }

        private void btnAccelerator_MouseDown(object sender, MouseEventArgs e) //按油門
        {
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_MR, "9003", 1);
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "4", 1450);
            btnAccelerator.BackColor = System.Drawing.Color.LightGreen;
        }

        private void btnAccelerator_MouseUp(object sender, MouseEventArgs e)  //放油門
        {
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_MR, "9003", 0);
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "4", 400);
            btnAccelerator.BackColor = SystemColors.Control;
        }

        private void btnBrakes_MouseDown(object sender, MouseEventArgs e)  //按煞車
        {
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "5", 1600);
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "7", 1400);
            btnBrakes.BackColor = System.Drawing.Color.LightGreen;
            //明天可以改DM5->800, DM7->1800.
        }

        private void btnBrakes_MouseUp(object sender, MouseEventArgs e)   //放煞車
        {
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "5", 400);
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "7", 3600);
            btnBrakes.BackColor = SystemColors.Control;
        }

        private void btnDirectionFront_Click(object sender, EventArgs e) //行走向前
        {
            int Arduino_Front = Arduino_Tool.Arduino_Func.Arduino_Front();
            if (Arduino_Front == 1)
            {
                btnDirectionFront.BackColor = System.Drawing.Color.LightGreen;
                btnDirectionBack.BackColor = SystemColors.Control;
            }
        }

        private void btnDirectionBack_Click(object sender, EventArgs e) //行走向後
        {
            int Arduino_Front = Arduino_Tool.Arduino_Func.Arduino_Back();
            if (Arduino_Front == 1)
            {
                btnDirectionBack.BackColor = System.Drawing.Color.LightGreen;
                btnDirectionFront.BackColor = SystemColors.Control;
            }
        }

        private void btnMoveFront_MouseDown(object sender, MouseEventArgs e)
        {
            CarMove(90);//直接控制前進
        }

        private void btnMoveFront_MouseUp(object sender, MouseEventArgs e)
        {
            MoveStop();//停止
        }

        private void brnMoveBack_MouseDown(object sender, MouseEventArgs e)
        {
            CarMove(-90);  //後退
        }

        private void brnMoveBack_MouseUp(object sender, MouseEventArgs e)
        {
            MoveStop();//停止
        }

        private void btnSetHeight_Click(object sender, EventArgs e)
        {
            SetForkHeightValue = Convert.ToInt32(txtSetHeight.Text);
            // setForkHeight(SetForkHeightValue);

            if (TimerSetForkHeight.Enabled == false)//開啟高度調整timer
            {
                if (TimerSetForkHeight.Interval != 50)
                {
                    TimerSetForkHeight.Interval = 50;
                    TimerSetForkHeight.Elapsed += new System.Timers.ElapsedEventHandler(SetForkHeight);
                }
                TimerSetForkHeight.Enabled = true;
            }
        }

        private void btnSetForkLeftRight_Click(object sender, EventArgs e)
        {
            int Value = Convert.ToInt16(txtForkValue.Text);
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "28", Value);
            Thread.Sleep(1);
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_MR, "014", 1);
        }

        private void btnClibratOrigin_Click(object sender, EventArgs e)
        {
            //轉向歸零
            AngleProcess(0);

            if (CanBusFunc.TransData[3] != 0) Thread.Sleep(3000);
            AdjustOriginCounter = 0;

            btnClibratOrigin.BackColor = SystemColors.Control;
            btnClibratOrigin.Text = "原點校正";

            //開啟轉向復歸校正涵式(快速)timer
            if (TimerAdjustOriginHighSpeed.Enabled == false)
            {
                if (TimerAdjustOriginHighSpeed.Interval != 150)
                {
                    TimerAdjustOriginHighSpeed.Interval = 150;
                    TimerAdjustOriginHighSpeed.Elapsed += new System.Timers.ElapsedEventHandler(AdjustOriginFuncHighSpeed);
                }
                TimerAdjustOriginHighSpeed.Enabled = true;
                GlobalVar.islibratOrigin = true;
            }
        }

        private void AdjustOriginFuncHighSpeed(object sender, EventArgs e) //轉向復歸校正涵式(快速)
        {
            if (CanBusFunc.TransData[3] <= 53)//小於70度
            {
                CanBusFunc.TransData[3] += 2;
                CanBusFunc.isSend = true;
                AdjustOriginTemp = CanBusFunc.TransData[3];
            }
            else
            {
                //開啟轉向復歸校正涵式(慢速)timer
                TimerAdjustOriginHighSpeed.Interval = 30;
                if (obj_PLC.doReadDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_MR, "110") != 1)
                {
                    AdjustOriginCounter += 4;
                    byte LowByte = (byte)(AdjustOriginCounter % 256);
                    byte HighByte = (byte)(AdjustOriginCounter / 256);
                    CanBusFunc.TransData[3] = (byte)(AdjustOriginTemp + HighByte);
                    CanBusFunc.TransData[2] = LowByte;
                    CanBusFunc.isSend = true;
                }
                else
                {
                    //抵達絕對90度
                    TimerAdjustOriginHighSpeed.Enabled = false;
                    GlobalVar.islibratOrigin = false;
                    Console.WriteLine("結束原點調整");
                    //計算原點誤差
                    H_Error = (int)CanBusFunc.TransData[3] - 64;
                    L_Error = (int)CanBusFunc.TransData[2] - 0;

                    btnClibratOrigin.BackColor = System.Drawing.Color.LightGreen;
                    btnClibratOrigin.Text = "校正完畢";
                }
            }
        }

        private void btnRelaxMotor_Click(object sender, EventArgs e)
        {

            //馬達控制-放掉
            CanBusFunc.TransMoveData[0] = System.Convert.ToByte("01", 16);
            CanBusFunc.TransMoveData[1] = System.Convert.ToByte("00", 16);
            CanBusFunc.TransMoveData[2] = System.Convert.ToByte("00", 16);
            CanBusFunc.TransMoveData[3] = System.Convert.ToByte("00", 16);
            CanBusFunc.TransMoveData[4] = System.Convert.ToByte("69", 16);
            CanBusFunc.TransMoveData[5] = System.Convert.ToByte("07", 16);
            CanBusFunc.TransMoveData[6] = System.Convert.ToByte("00", 16);
            CanBusFunc.TransMoveData[7] = System.Convert.ToByte("00", 16);
            CanBusFunc.isMoveSend = true;
        }

        private void btnRotation_Click(object sender, EventArgs e)
        {
            int RotationValue = Convert.ToInt16(txtRotationValue.Text);
            AngleProcess(RotationValue);
            GlobalVar.isCanBusDebug = true;
        }

        private void btnDoFrontBack_MouseDown(object sender, MouseEventArgs e)
        {
            int power = Convert.ToInt16(txtFrontBackValue.Text);
            int HiSpeed = 0;
            int LoSpeed = 0;
            PowerToSpeedTrans(power, out HiSpeed, out LoSpeed);
            SendToPowerMotor(HiSpeed, LoSpeed);
        }

        private void btnDoFrontBack_MouseUp(object sender, MouseEventArgs e)
        {
            MoveStop();
        }

        #endregion

        #region CANBusFunction

        private void btnCanConnect_Click(object sender, EventArgs e)
        {
            if (CanConnect_Status == 0) //如果裝置尚未連線
            {
                //CanBus開始連線
                CanConnect_Status = CanBusFunc.CanConnect();

                //CanBus連線成功
                if (CanConnect_Status == 1)
                {
                    btnCanConnect.BackColor = System.Drawing.Color.Red;
                    btnOpenCan.Enabled = true;
                }
                else  //CanBus連線失敗
                {
                    btnCanConnect.BackColor = SystemColors.Control;
                    MessageBox.Show("打開裝置失敗,請檢察", "錯誤",
                                 MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else if (CanConnect_Status == 1)    //如果裝置連線成功
            {
                MessageBox.Show("裝置已連線", "裝置狀態",
                                 MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnOpenCan_Click(object sender, EventArgs e)
        {
            if (CanOpenStatus == 0) //如果CanBus尚未開啟
            {
                //開啟CanBus
                CanBusFunc.CanInit();
                CanOpenStatus = CanBusFunc.OpenCan();

                //CanBus開啟成功
                if (CanOpenStatus == 1)
                {
                    timer_system.Enabled = true;
                    btnOpenCan.BackColor = System.Drawing.Color.LightGreen;

                    //CanBus開啟成功(開啟可使用的按鈕)
                    ForkliftControl_CanBusConnection();
                }
                else //CanBus開啟失敗
                {
                    MessageBox.Show("開啟Canbus失敗,請檢察", "錯誤",
                             MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else if (CanOpenStatus == 1)//如果CanBus已經開啟
            {
                MessageBox.Show("Canbus已開啟,請檢察", "裝置狀態",
                             MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        #region 系統timer
        //初始化時間
        private void timer_system_Tick(object sender, EventArgs e)
        {
            if (GlobalVar.SysTimerCounter > GlobalVar.SystemInitTimes) //Bus初始化
            {
                CanBusFunc.CanControl = true;
                labelCANStatus.Text = "開始介入";
                timer_system.Enabled = false;
            }
            else if (GlobalVar.SysTimerCounter <= GlobalVar.SystemInitTimes) GlobalVar.SysTimerCounter++;

            if (Arduino_Tool.Arduino_Func.isbtnFrontBackClick) //Arduino前後切換中，暫時停止重量接收
            {
                if (btnFrontBack == 1)
                {
                    Arduino_Tool.Arduino_Func.isbtnFrontBackClick = false;
                    btnFrontBack = 0;
                }
                btnFrontBack++;
            }
        }
        #endregion

        #region 左右急轉Timer
        //方向順逆時針旋轉
        private void timer_Rotation_Tick(object sender, EventArgs e)
        {
            //直接控制轉向
            if (CanBusFunc.CANBUS_TurnLeft) //順轉
            {
                CanBusFunc.TransData[3] += 1;
                if (CanBusFunc.TransData[3] > 120 && CanBusFunc.TransData[3] < 127) CanBusFunc.TransData[3] = 120;
                CanBusFunc.isSend = true;
            }
            if (CanBusFunc.CANBUS_TurnRight)//逆轉
            {
                CanBusFunc.TransData[3] -= 1;
                if (CanBusFunc.TransData[3] < 140 && CanBusFunc.TransData[3] > 127) CanBusFunc.TransData[3] = 140;
                CanBusFunc.isSend = true;
            }

            //更新車體資料狀態
            Update_Car_Status_Information();
        }

        //更新車體資料狀態
        public void Update_Car_Status_Information()
        {
            //更新車體資料狀態
            byte TempAGV_Status = 0;
            byte TempForkStatus = 0;
            if (DeliverData != null)
            {
                TempAGV_Status = DeliverData.tAGV_Data.ucAGV_Status;
                TempForkStatus = DeliverData.tAGV_Data.CFork.tForkData.ucStatus;
            }

            object[] obj = new object[10] { 
                GlobalVar.CurrentPosition.LocationX,
                GlobalVar.CurrentPosition.LocationY,
                GlobalVar.CurrentPosition.Direction,
                GlobalVar.ForkCurrentHeight,
                TempAGV_Status.ToString(),
                TempForkStatus.ToString(),
                GlobalVar.CarTireSpeedLeft.ToString(),
                GlobalVar.CarTireSpeedRight.ToString(),
                Math.Round(GlobalVar.RealMotorAngle, 2),
                 GlobalVar.RealMotorPower
            };

            dataGridView2.Rows.Clear();
            DataGridViewRow dgvr = new DataGridViewRow();
            dgvr.CreateCells(dataGridView2, obj);
            dgvr.Height = 35;
            dgvr.DefaultCellStyle.BackColor = System.Drawing.Color.LightSkyBlue;
            dataGridView2.Rows.Add(dgvr);
        }

        #endregion

        #region 轉向馬達函式

        //原點復歸
        private void btnOrigin_Click(object sender, EventArgs e)
        {
            AngleProcess(0);
        }

        private void btnCANMode_Click(object sender, EventArgs e)
        {
            if (btnCANMode.Text == "手動模式")
            {
                if (CanBusFunc.TransData[2] != 0 || CanBusFunc.TransData[3] != 0)
                {
                    MessageBox.Show("請先復歸");
                    return;
                }
                btnCANMode.Text = "自動模式";
                CANHandMode = true;
            }
            else
            {
                btnCANMode.Text = "手動模式";
                CANHandMode = false;
                CanBusFunc.TransData[2] = System.Convert.ToByte("00", 16);
                CanBusFunc.TransData[3] = System.Convert.ToByte("00", 16);
                CanBusFunc.TransData[4] = GlobalVar.RotateSpeed;
                CanBusFunc.TransData[5] = GlobalVar.RotateSpeed;
                CanBusFunc.isSend = true;
            }
        }
        #endregion

        #region 轉向涵式

        public static void AngleProcess(int value)
        {
            if (Math.Abs(value) > 140) return;
            if (value < 0) value = 360 + value;

            //高位元(TransData[3])將360度分成256間隔
            //低為元(TransData[2])將每一高位元的值分成256間隔
            double HT_Value = (double)value / (double)1.40625;
            int temp = Convert.ToInt16(Math.Floor(HT_Value));
            double LT_Value = HT_Value - temp;
            LT_Value *= (double)1.40625;
            LT_Value /= (double)0.0054931640625;

            int HValue = Convert.ToInt16(Math.Floor(HT_Value));
            int LValue = Convert.ToInt16(Math.Floor(LT_Value));

            HValue += H_Error;
            LValue += L_Error;
            if (LValue >= 255)
            {
                HValue = HValue + 1;
                LValue = LValue - 255;
            }
            CanBusFunc.TransData[2] = System.Convert.ToByte((byte)LValue);
            CanBusFunc.TransData[3] = System.Convert.ToByte((byte)HValue);
            CanBusFunc.TransData[4] = GlobalVar.RotateSpeed;
            CanBusFunc.TransData[5] = GlobalVar.RotateSpeed;
            CanBusFunc.isSend = true;
        }

        #endregion

        #region 行走馬達函式

        /// <summary>
        /// 車子往前走
        /// </summary>
        /// <param name="speed">speed為速度，輸入正為向前進，輸入負為向後退</param>
        public void CarMove(int speed)
        {
            //放開系統初始煞車
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_MR, "9003", 1);
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "4", 1450);

            int HiSpeed = 0;
            int LoSpeed = 0;
            PowerToSpeedTrans(speed, out HiSpeed, out LoSpeed);
            SendToPowerMotor(HiSpeed, LoSpeed);

        }

        public static void MoveStop()
        {
            //馬達控制-停止
            CanBusFunc.TransMoveData[0] = System.Convert.ToByte("09", 16);
            CanBusFunc.TransMoveData[1] = System.Convert.ToByte("00", 16);
            CanBusFunc.TransMoveData[2] = System.Convert.ToByte("00", 16);
            CanBusFunc.TransMoveData[3] = System.Convert.ToByte("00", 16);
            CanBusFunc.TransMoveData[4] = System.Convert.ToByte("69", 16);
            CanBusFunc.TransMoveData[5] = System.Convert.ToByte("07", 16);
            CanBusFunc.TransMoveData[6] = System.Convert.ToByte("00", 16);
            CanBusFunc.TransMoveData[7] = System.Convert.ToByte("00", 16);
            CanBusFunc.isMoveSend = true;
        }

        #endregion

        #endregion

        #region Lidar Function

        private void btnStartLidar_Click(object sender, EventArgs e)
        {
            if (!LidarFun.isLidarStart)
            {
                //載入Lidar座標轉換變數
                LidarFun.LoadLidarVariable();
                LidarFun.isLidarStart = true;
                LidarFun.UDPThread = new Thread(new ThreadStart(LidarFun.StartLidarUDP));
                LidarFun.UDPThread.Start();
                btnStartLidar.Text = "關閉光達";
            }
            else
            {
                LidarFun.isLidarStart = false;
                LidarFun.UDPThread = null;
                btnStartLidar.Text = "開啟光達";
            }
            /*if (!LidarFunc.isLidarStart)
            {
                //Lidar Buffer空間宣告
                LidarFunc.LidarBuffer = new byte[1248];
                LidarFunc.VerticalAngle = new double[32];
                LidarFunc.CosVerticalAngle = new double[32];
                LidarFunc.SinVerticalAngle = new double[32];
                LidarFunc.LidarX = new double[36000];
                LidarFunc.LidarY = new double[36000];
                LidarFunc.LidarZ = new double[36000];

                //載入Lidar座標轉換變數
                LidarFunc.LoadLidarVariable();      
                LidarFunc.isLidarStart = true;
                LidarFunc.UDPThread = new Thread(new ThreadStart(LidarFunc.StartLidarUDP));
                LidarFunc.UDPThread.Start();
                btnStartLidar.Text = "關閉光達";
            }
            else
            {
                LidarFunc.isLidarStart = false;
                LidarFunc.UDPThread = null;
                btnStartLidar.Text = "開啟光達";
            }*/
        }

        private void btnResetView_Click(object sender, EventArgs e)
        {
            //重置視角
            GLDrawObject.ResetView();
        }

        private void OpenGLDraw_Small(object sender, PaintEventArgs e)
        {
            //畫OpenGL圖
            OpenGLControl ctl = (OpenGLControl)sender;
            Draw3D.Draw3DInformation(ctl.OpenGL, LidarFun.rLidarData.X, LidarFun.rLidarData.Y, LidarFun.rLidarData.Z);
            // Draw3D.Draw3DInformation(ctl.OpenGL, LidarFunc.r_LidarData.X, LidarFunc.r_LidarData.Y, LidarFunc.r_LidarData.Z);
        }

        private void OpenGLCtrl_OpenGLDraw(object sender, PaintEventArgs e)
        {
            OpenGLControl ctl = (OpenGLControl)sender;
            Draw3D.Draw3DInformation(ctl.OpenGL, LidarFun.rLidarData.X, LidarFun.rLidarData.Y, LidarFun.rLidarData.Z);
        }

        private void OpenGLCtrl_MouseDown(object sender, MouseEventArgs e)
        {
            //OpenGL畫面滑鼠按下
            OpenGLControl ctrl = (OpenGLControl)sender;
            GLDrawObject._BakMousePosition = new Point(e.X, e.Y);
            if (e.Button == MouseButtons.Left)
            {
                GLDrawObject._3DMoveStatus = GLDrawObject.MoveStatus.Rotate;
            }
            else if (e.Button == MouseButtons.Right)
            {
                GLDrawObject._3DMoveStatus = GLDrawObject.MoveStatus.Shift;
            }
            ctrl.MouseMove += new MouseEventHandler(GLDrawObject.OpenGLCtrl_MouseMove);
            LidarFun._BakMousePosition.X = e.X;
            LidarFun._BakMousePosition.Y = e.Y;
        }

        private void OpenGLCtrl_MouseUp(object sender, MouseEventArgs e)
        {
            //OpenGL畫面滑鼠放開
            OpenGLControl ctrl = (OpenGLControl)sender;
            ctrl.MouseMove -= new MouseEventHandler(GLDrawObject.OpenGLCtrl_MouseMove);
        }

        void OpenGLCtrl_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //放大縮小
            GLDrawObject._LZ += e.Delta / 300.0f;
        }

        private void btnReset3DView_Click(object sender, EventArgs e)
        {
            GLDrawObject.ResetView();
        }

        private void btnLidarRecode_Click(object sender, EventArgs e)
        {
            if (!LidarFun.isLidarDataRecode)
            {
                LidarFun.isLidarDataRecode = true;
                btnLidarRecode.Text = "停止紀錄";
            }
            else
            {
                LidarFun.isLidarDataRecode = false;
                btnLidarRecode.Text = "開始紀錄";
            }
        }

        private void btnLidarDataOut_Click(object sender, EventArgs e)
        {
            LidarFun.LidarDataOut();
        }

        private void btnOpenLidarFile_Click(object sender, EventArgs e)
        {
            //開啟選擇檔案視窗
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Lidar Files|*.ldr";
            openFileDialog1.Title = "Select a Lidar File";
            string OpenFileName = "";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                OpenFileName = openFileDialog1.FileName;
            else return;

            LidarFun.LidarReader = new StreamReader(OpenFileName);
            string Eachline = LidarFun.LidarReader.ReadLine();
            Eachline = LidarFun.LidarReader.ReadLine();

            //開啟讀取檔案Thread
            LidarFun.isEOF = false;
            LidarFun.ReadFileThread = new Thread(new ThreadStart(LidarFun.ReadLidarDataFuncThread));
            LidarFun.ReadFileThread.Start();
        }

        #endregion

        #region SICK-NAV350

        private void btnNAVConnect_Click(object sender, EventArgs e)
        {
            if (NAVClass == null) NAVClass = new SICK_NAV();
            //執行連線
            if (txtNAVIP.Text != null && txtNAVPort.Text != null && Connection_Control_SICK != "連線成功")
            {
                Connection_Control_SICK = NAVClass.NAVConnect(txtNAVIP.Text, txtNAVPort.Text);
                txtNavReceive.Text = DateTime.Now.ToString() + ", " + Connection_Control_SICK + "\r\n" + txtNavReceive.Text;
                if (Connection_Control_SICK == "連線成功")
                {
                    btnNAVDisConnect.Enabled = true;    //斷線
                    btn_Mode_Navigation.Enabled = true; //Navigation
                    btnContinueLocation.Enabled = true; //連續座標
                    btnStartServer.Enabled = true;      //Server連線
                    btnContinue.Enabled = true;         //繼續
                    btnPause.Enabled = true;            //暫停
                    btnDoCmd.Enabled = true;            //執行
                    button1.Enabled = true;             //RunDown1
                    button2.Enabled = true;             //RunDown2
                    button3.Enabled = true;             //RunDown3
                    button4.Enabled = true;             //RunDown4

                }
            }

        }

        private void btnNAVDisConnect_Click(object sender, EventArgs e)
        {
            txtNavReceive.Text = DateTime.Now.ToString() + ", " + NAVClass.NAVDisConnect() + "\r\n" + txtNavReceive.Text;
        }

        private void btn_Mode_PowerDown_Click(object sender, EventArgs e)
        {

            NAVClass.Mode_PowerDown(); //切換至PowerDown模式
        }

        private void btn_Mode_Standby_Click(object sender, EventArgs e)
        {
            NAVClass.Mode_Standby();//切換至Standby模式
        }

        private void btn_Mode_LandMark_Click(object sender, EventArgs e)
        {
            NAVClass.Mode_LandMark();//切換至LandMark Detection模式
        }

        private void btn_Mode_Mapping_Click(object sender, EventArgs e)
        {
            NAVClass.Mode_Mapping();//切換至Mapping模式
        }

        private void btn_Mode_Navigation_Click(object sender, EventArgs e)
        {
            NAVClass.Mode_Navigation(); //切換至Navigation模式
        }

        private void btnSetUserLevel_Click(object sender, EventArgs e)
        {
            NAVClass.SetUserLevel();//開啟使用者權限
        }

        private void btnContinueLocation_Click(object sender, EventArgs e)
        {
            btnContinueLocation.Text = NAVClass.ContinueLocation();
        }

        #endregion

        #region 模擬自走

        private void btnClearList_Click(object sender, EventArgs e)
        {
            LogCarInfo.Clear();
            LogMainData.Clear();
            LogDetailData.Clear();
        }

        private void btnSaveTxt_Click(object sender, EventArgs e)
        {
            ExcelWrite.WriteLog("C:LogInfo" + DateTime.Now.ToString("yyyyMMdd-HHmmss"), LogCarInfo, LogMainData, LogDetailData);
            LogCarInfo.Clear();
            LogMainData.Clear();
            LogDetailData.Clear();
        }

        /*private void TrasformCoordinate(NavigationInfo Ori, NavigationInfo src, NavigationInfo dst, int degrees)
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
        }*/

        private void TrasformCoordinateForFork(NavigationInfo Ori, NavigationInfo src, NavigationInfo dst, int degrees)
        {
            //座標旋轉
            int degr = degrees;
            if (degr > 180) degr = 360 - degr;
            else degr = -degr;
            if (degr >= 88 && degr <= 90) degr = 90;//減少座標誤差
            double angle = Math.PI * degr / 180.0;
            double sinAngle = Math.Sin(angle);
            double cosAngle = Math.Cos(angle);

            dst.LocationX = (int)((double)(src.LocationX - Ori.LocationX) * cosAngle + (double)(src.LocationY - Ori.LocationY) * sinAngle) + Ori.LocationX;
            dst.LocationY = (int)((double)-(src.LocationX - Ori.LocationX) * sinAngle + (double)(src.LocationY - Ori.LocationY) * cosAngle) + Ori.LocationY;
        }

        public static void SetForkHeight(object sender, EventArgs e)  //設定貨叉高度一階
        {
            int DisHeight = Math.Abs((int)GlobalVar.ForkCurrentHeight - SetForkHeightValue);
            if (DisHeight > 1)
            {
                //設定高度
                if (ForkisRunnung) return;
                obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "20", SetForkHeightValue);
                //Thread.Sleep(1);
                obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_MR, "007", 1);

                ForkisRunnung = true;
            }
            else
            {
                TimerSetForkHeight.Enabled = false;
                ForkisRunnung = false;
                //Console.WriteLine("抵達高度");
            }
        }

        #endregion

        #region PID_Test

        public static void GetFrontWheelSpeed()
        {
            //跟PLC要右輪速度
            int SpeedRight = obj_PLC.doReadDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "52");
            if (SpeedRight > 32767) SpeedRight = -(65536 - SpeedRight);
            if (Math.Abs(SpeedRight) < 15) SpeedRight = 0;
            GlobalVar.CarTireSpeedRight = SpeedRight;

            //跟PLC要左輪速度
            int SpeedLeft = obj_PLC.doReadDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "54");
            if (SpeedLeft > 32767) SpeedLeft = -(65536 - SpeedLeft);
            if (Math.Abs(SpeedLeft) < 15) SpeedLeft = 0;
            GlobalVar.CarTireSpeedLeft = SpeedLeft;

            //跟PLC要目前高度
            int RecviveHeight = obj_PLC.doReadDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "12");
            GlobalVar.ForkCurrentHeight = RecviveHeight;
        }

        private void PowerToSpeedTrans(int Power, out int HiSpeed, out int LoSpeed)
        {
            //PID計算完的Power轉換至馬達訊號
            if (Power >= 0)
            {
                if (Power > GlobalVar.MaxPower) Power = GlobalVar.MaxPower;
                double TempValue = (double)Power * 6.97265625;
                HiSpeed = (int)(TempValue / 256);
                LoSpeed = (int)(TempValue % 256);
                //Console.WriteLine("HiSpeed:" + HiSpeed.ToString() + ", LoSpeed:" + LoSpeed.ToString());
            }
            else
            {
                if (Power < -GlobalVar.MaxPower) Power = -GlobalVar.MaxPower;
                double TempValue = ((double)Math.Abs(Power)) * 6.97265625;
                HiSpeed = 255 - (int)(TempValue / 256);
                LoSpeed = 255 - (int)(TempValue % 256);
                //Console.WriteLine("HiSpeed:" + HiSpeed.ToString() + ", LoSpeed:" + LoSpeed.ToString());
            }
        }

        private void ValueToAccelerationTrans(int Acceleration, out int HiSpeed, out int LoSpeed)
        {
            //PID計算完的Power轉換至馬達訊號
            if (Acceleration > 0)
            {
                if (Acceleration > 250) Acceleration = 250;
                double TempValue = (double)Acceleration * 6.97265625;
                HiSpeed = (int)(TempValue / 256);
                LoSpeed = (int)(TempValue % 256);
                //Console.WriteLine("HiSpeed:" + HiSpeed.ToString() + ", LoSpeed:" + LoSpeed.ToString());
            }
            else
            {
                if (Acceleration < -250) Acceleration = -250;
                double TempValue = ((double)Math.Abs(Acceleration)) * 6.97265625;
                HiSpeed = 255 - (int)(TempValue / 256);
                LoSpeed = 255 - (int)(TempValue % 256);
                //Console.WriteLine("HiSpeed:" + HiSpeed.ToString() + ", LoSpeed:" + LoSpeed.ToString());
            }
        }

        private void SendToPowerMotor(int HiSpeed, int LoSpeed)
        {
            //計算加速度值
            int HiAcceleration = 0;
            int LoAcceleration = 0;
            ValueToAccelerationTrans(GlobalVar.MotorAcceleration, out HiAcceleration, out LoAcceleration);
            CanBusFunc.TransMoveData[0] = System.Convert.ToByte("09", 16);
            CanBusFunc.TransMoveData[1] = System.Convert.ToByte("00", 16);
            CanBusFunc.TransMoveData[2] = Convert.ToByte(LoSpeed);
            CanBusFunc.TransMoveData[3] = Convert.ToByte(HiSpeed);
            CanBusFunc.TransMoveData[4] = Convert.ToByte(LoAcceleration);
            CanBusFunc.TransMoveData[5] = Convert.ToByte(HiAcceleration);
            CanBusFunc.TransMoveData[6] = System.Convert.ToByte("00", 16);
            CanBusFunc.TransMoveData[7] = System.Convert.ToByte("00", 16);
            CanBusFunc.isMoveSend = true;
        }

        #endregion

        private void DiffTimeForAlignment(ref rtVector PredictPosition, ref double PredictAngle, rtMotorCtrl Data)
        {
            TimeSpan DisTime = DateTime.Now - GlobalVar.NavTimeStamp;
            LocateData.tPosition.eX = GlobalVar.CurrentPosition.LocationX;
            LocateData.tPosition.eY = GlobalVar.CurrentPosition.LocationY;
            LocateData.eAngle = GlobalVar.CurrentPosition.Direction;
            LocateData.eWheelAngle = GlobalVar.RealMotorAngle;
            a_tAGV_Data.tCarInfo = LocateData;
            rtMotorCtrl.Motion_Predict(a_tAGV_Data.tCarInfo, Data, DisTime.TotalMilliseconds / 1000, out PredictPosition, out PredictAngle);
        }

        private void CoordinateTransformProcess(rtAGV_Data src, ref rtAGV_Data dst)
        {
            //定位座標轉換為車體座標資訊
            dst.tCarInfo.tPosition.eX = src.tCarInfo.tPosition.eX;
            dst.tCarInfo.tPosition.eY = src.tCarInfo.tPosition.eY;
            dst.tCarInfo.eAngle = src.tCarInfo.eAngle;

            rtVector L_wheelPosition;
            rtVector R_wheelPosition;
            rtVector Motor_Position;

            L_wheelPosition.eX = src.tCarInfo.tPosition.eX;
            L_wheelPosition.eY = src.tCarInfo.tPosition.eY + 600;

            R_wheelPosition.eX = src.tCarInfo.tPosition.eX;
            R_wheelPosition.eY = src.tCarInfo.tPosition.eY - 600;

            Motor_Position.eX = src.tCarInfo.tPosition.eX - 1500;
            Motor_Position.eY = src.tCarInfo.tPosition.eY;

            Trasform_rtVector(src.tCarInfo.tPosition, L_wheelPosition, ref dst.tCarInfo.tCarTirepositionL, src.tCarInfo.eAngle);
            Trasform_rtVector(src.tCarInfo.tPosition, R_wheelPosition, ref dst.tCarInfo.tCarTirepositionR, src.tCarInfo.eAngle);
            Trasform_rtVector(src.tCarInfo.tPosition, Motor_Position, ref dst.tCarInfo.tMotorPosition, src.tCarInfo.eAngle);
        }

        private void Trasform_rtVector(rtVector Ori, rtVector src, ref rtVector dst, double degrees)
        {
            rtVector temp = new rtVector();
            temp.eX = src.eX;
            temp.eY = src.eY;

            //座標旋轉
            int degr = (int)degrees;
            if (degr > 180) degr = 360 - degr;
            else degr = -degr;
            double angle = Math.PI * degr / 180.0;
            double sinAngle = Math.Sin(angle);
            double cosAngle = Math.Cos(angle);

            dst.eX = (int)((double)(temp.eX - Ori.eX) * cosAngle + (double)(temp.eY - Ori.eY) * sinAngle) + Ori.eX;
            dst.eY = (int)((double)-(temp.eX - Ori.eX) * sinAngle + (double)(temp.eY - Ori.eY) * cosAngle) + Ori.eY;
        }

        private bool setForkHeight(int SetForkHeightValue)
        {
            int DisHeight = Math.Abs((int)GlobalVar.ForkCurrentHeight - SetForkHeightValue);
            if (DisHeight > 1)
            {
                //設定高度
                //Console.WriteLine("ForkisRunnung:" + ForkisRunnung.ToString());
                //if (ForkisRunnung) return false;
                obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "20", SetForkHeightValue);
                Task.Delay(1);
                obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_MR, "007", 1);
                ForkisRunnung = true;
                return false;
            }
            else
            {
                ForkisRunnung = false;
                //Console.WriteLine("抵達高度");
                return true;
            }
        }

        public class ThreadWithState
        {
            //接收車體更新的資訊用
            public rtAGV_Control ThreadParameter;

            //接收指令用
            public ulong agv_Command;

            public void DoFlowFristProcess()
            {
                //執行agv_Command收到的指令
                Set_ForthDepthCount = 0;
                Set_BackDepthCount = 0;
                ForkisRunnung = false;
                DeliverFlowTimer.Enabled = true;
                byte[] Cmdbytes = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05 };
                ulong LongCmd = BitConverter.ToUInt64(Cmdbytes, 0);
                DeliverData.ExecuteCmd(LongCmd);

                Thread.Sleep(1000);
                Set_ForthDepthCount = 0;
                Set_BackDepthCount = 0;
                byte[] Cmdbytes2 = { 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x06 };
                LongCmd = BitConverter.ToUInt64(Cmdbytes2, 0);
                DeliverData.ExecuteCmd(LongCmd);

                Thread.Sleep(1000);
                Set_BackDepthCount = 0;
                byte[] Cmdbytes3 = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x07 };
                LongCmd = BitConverter.ToUInt64(Cmdbytes3, 0);
                DeliverData.ExecuteCmd(LongCmd);
            }

            public void DoFlowSecondProcess()
            {
                //執行agv_Command收到的指令
                Set_ForthDepthCount = 0;
                Set_BackDepthCount = 0;
                ForkisRunnung = false;
                DeliverFlowTimer.Enabled = true;
                byte[] Cmdbytes = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x05 };
                ulong LongCmd = BitConverter.ToUInt64(Cmdbytes, 0);
                DeliverData.ExecuteCmd(LongCmd);

                Thread.Sleep(1000);
                Set_ForthDepthCount = 0;
                Set_BackDepthCount = 0;
                byte[] Cmdbytes2 = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06 };
                LongCmd = BitConverter.ToUInt64(Cmdbytes2, 0);
                DeliverData.ExecuteCmd(LongCmd);

                Thread.Sleep(1000);
                Set_BackDepthCount = 0;
                byte[] Cmdbytes3 = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x07 };
                LongCmd = BitConverter.ToUInt64(Cmdbytes3, 0);
                DeliverData.ExecuteCmd(LongCmd);
            }

            public void DoFlowThirdProcess()
            {
                //執行agv_Command收到的指令
                Set_ForthDepthCount = 0;
                Set_BackDepthCount = 0;
                ForkisRunnung = false;
                DeliverFlowTimer.Enabled = true;
                byte[] Cmdbytes = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05 };
                ulong LongCmd = BitConverter.ToUInt64(Cmdbytes, 0);
                DeliverData.ExecuteCmd(LongCmd);

                Thread.Sleep(1000);
                Set_ForthDepthCount = 0;
                Set_BackDepthCount = 0;
                byte[] Cmdbytes2 = { 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x06 };
                LongCmd = BitConverter.ToUInt64(Cmdbytes2, 0);
                DeliverData.ExecuteCmd(LongCmd);

                Thread.Sleep(1000);
                Set_BackDepthCount = 0;
                byte[] Cmdbytes3 = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x07 };
                LongCmd = BitConverter.ToUInt64(Cmdbytes3, 0);
                DeliverData.ExecuteCmd(LongCmd);
            }

            public void DoFlowForthProcess()
            {
                //執行agv_Command收到的指令
                Set_ForthDepthCount = 0;
                Set_BackDepthCount = 0;
                ForkisRunnung = false;
                DeliverFlowTimer.Enabled = true;
                byte[] Cmdbytes = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x05 };
                ulong LongCmd = BitConverter.ToUInt64(Cmdbytes, 0);
                DeliverData.ExecuteCmd(LongCmd);

                Thread.Sleep(1000);
                Set_ForthDepthCount = 0;
                Set_BackDepthCount = 0;
                byte[] Cmdbytes2 = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06 };
                LongCmd = BitConverter.ToUInt64(Cmdbytes2, 0);
                DeliverData.ExecuteCmd(LongCmd);

                Thread.Sleep(1000);
                Set_BackDepthCount = 0;
                byte[] Cmdbytes3 = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x07 };
                LongCmd = BitConverter.ToUInt64(Cmdbytes3, 0);
                DeliverData.ExecuteCmd(LongCmd);
            }

            public void DoCommandProcess()
            {
                //執行agv_Command收到的指令
                Set_ForthDepthCount = 0;
                Set_BackDepthCount = 0;
                ForkisRunnung = false;
                DeliverFlowTimer.Enabled = true;
                Thread.Sleep(100);
                DeliverData.ExecuteCmd(agv_Command);
            }

            public void ThreadPause()
            {
                //執行暫停
                byte[] Cmdbytes = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03 };
                ulong LongCmd = BitConverter.ToUInt64(Cmdbytes, 0);
                DeliverData.ExecuteCmd(LongCmd);
            }

            public void ThreadContinue()
            {
                //執行恢復
                byte[] Cmdbytes = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02 };
                ulong LongCmd = BitConverter.ToUInt64(Cmdbytes, 0);
                DeliverData.ExecuteCmd(LongCmd);
            }
        }

        /** \brief 是否更新資訊給執行緒*/
        public static System.Timers.Timer DeliverFlowTimer = new System.Timers.Timer();

        /** \brief AGV 系統狀態*/
        public static rtAGV_Control DeliverData;

        /** \brief 推高機系統資訊*/
        public static ThreadWithState ThreatPara;

        /** \brief 預測紀錄執行緒*/
        public static Thread DeliverThread;

        public void UpdateDeliverData()
        {
            rtVector PredictPosition = new rtVector();
            rtAGV_Data src = new rtAGV_Data();
            rtAGV_Data dst = new rtAGV_Data();

            //讀取PLC的高度及速度資訊
            if (DeliverData.tAGV_Data.ucAGV_Status != 0) GetFrontWheelSpeed();

            //紀錄讀取完的時間
            //GlobalVar.Watch_Read_PLC_Data.Start();
            //GlobalVar.Time_Read_PLC_Data = DateTime.Now;

            //計算預測座標
            double PredictAngle = 0;
            DiffTimeForAlignment(ref PredictPosition, ref PredictAngle, DeliverData.tAGV_Data.CMotor);

            src.tCarInfo.tPosition.eX = PredictPosition.eX;
            src.tCarInfo.tPosition.eY = PredictPosition.eY;
            src.tCarInfo.eAngle = PredictAngle;

            //定位座標轉換為車體座標資訊
            CoordinateTransformProcess(src, ref dst);

            //更新車體資訊
            DeliverData.tAGV_Data.tCarInfo.tPosition.eX = PredictPosition.eX;
            DeliverData.tAGV_Data.tCarInfo.tPosition.eY = PredictPosition.eY;
            DeliverData.tAGV_Data.tCarInfo.eAngle = PredictAngle;
            DeliverData.tAGV_Data.tCarInfo.eCarTireSpeedLeft = GlobalVar.CarTireSpeedLeft; ;
            DeliverData.tAGV_Data.tCarInfo.eCarTireSpeedRight = GlobalVar.CarTireSpeedRight;

            DeliverData.tAGV_Data.tCarInfo.tCarTirepositionR.eX = dst.tCarInfo.tCarTirepositionR.eX;
            DeliverData.tAGV_Data.tCarInfo.tCarTirepositionR.eY = dst.tCarInfo.tCarTirepositionR.eY;
            DeliverData.tAGV_Data.tCarInfo.tCarTirepositionL.eX = dst.tCarInfo.tCarTirepositionL.eX;
            DeliverData.tAGV_Data.tCarInfo.tCarTirepositionL.eY = dst.tCarInfo.tCarTirepositionL.eY;
            DeliverData.tAGV_Data.tCarInfo.tMotorPosition.eX = dst.tCarInfo.tMotorPosition.eX;
            DeliverData.tAGV_Data.tCarInfo.tMotorPosition.eY = dst.tCarInfo.tMotorPosition.eY;

            /*DeliverData.tAGV_Data.tCarInfo.tPosition.eX = 17050;
            DeliverData.tAGV_Data.tCarInfo.tPosition.eY = -950;
            DeliverData.tAGV_Data.tCarInfo.eAngle = 90;
            /*DeliverData.tAGV_Data.tCarInfo.tMotorPosition.eX = 17223;
            DeliverData.tAGV_Data.tCarInfo.tMotorPosition.eY = -3058;*/

            //更新車Sensor資訊
            DeliverData.tAGV_Data.tCarInfo.eWheelAngle = GlobalVar.RealMotorAngle;
            DeliverData.tAGV_Data.tSensorData.tForkInputData.height = (int)GlobalVar.ForkCurrentHeight;

            /* DeliverData.tAGV_Data.tSensorData.tForkInputData.height = (int)120;
             DeliverData.tAGV_Data.ucAGV_Status = (byte)7;*/
        }

        //更新資訊給執行緒
        public void UpdataDeliverFlowData(object sender, EventArgs e)
        {
            //GlobalVar.Watch_Read_PLC_Data = new Stopwatch();
            //紀錄讀取完的時間
            //GlobalVar.Watch_Read_PLC_Data.Start();

            int HiSpeed = 0;
            int LoSpeed = 0;

            //更新車輛資訊
            UpdateDeliverData();

            //餵給執行緒更新資料
            ThreatPara.ThreadParameter = DeliverData;

            //避免間隔時間過短
            Task.Delay(1);

            //監聽貨叉是否需要執行動做，並紀錄Log
            rtAGVAndMainHandshake();

            //執行角度與Power數值
            AngleProcess(DeliverData.tAGV_Data.CMotor.tMotorData.lMotorAngle);
            if (DeliverData.tAGV_Data.CMotor.tMotorData.lMotorPower == 0) MoveStop();
            else
            {
                PowerToSpeedTrans(DeliverData.tAGV_Data.CMotor.tMotorData.lMotorPower, out HiSpeed, out LoSpeed);
                SendToPowerMotor(HiSpeed, LoSpeed);
            }
            //Console.WriteLine("lMotorPower: " + DeliverData.tAGV_Data.CMotor.tMotorData.lMotorPower);
            //GlobalVar.Watch_Read_PLC_Data.Stop();
        }

        //監聽貨叉是否需要執行動做
        private void rtAGVAndMainHandshake()
        {
            TimeSpan ts;
            ts = DateTime.Now - GlobalVar.Time_Read_PLC_Data;
            DeliverData.tAGV_Data.tSensorData.tForkInputData.distanceDepth = -1000;
            //執行SetHeight
            if (DeliverData.tAGV_Data.CFork.tForkData.ucStatus == (byte)rtForkCtrl.ForkStatus.ALIMENT &&
                DeliverData.tAGV_Data.CFork.tForkData.bEnable == true)
            {
                if (setForkHeight(DeliverData.tAGV_Data.CFork.tForkData.height))
                {
                    DeliverData.tAGV_Data.tSensorData.tForkInputData.distanceDepth = 0;
                    ForkisRunnung = false;
                }
            }

            //執行ReSetHeight
            if (DeliverData.tAGV_Data.CFork.tForkData.ucStatus == (byte)rtForkCtrl.ForkStatus.RESET_HEIGHT &&
                DeliverData.tAGV_Data.CFork.tForkData.bEnable == true)
            {
                if (setForkHeight(DeliverData.tAGV_Data.CFork.tForkData.height))
                {
                    DeliverData.tAGV_Data.tSensorData.tForkInputData.distanceDepth = 0;
                    ForkisRunnung = false;
                }
            }

            //執行伸貨叉
            if (DeliverData.tAGV_Data.CFork.tForkData.ucStatus == (byte)rtForkCtrl.ForkStatus.SET_DEPTH &&
                 DeliverData.tAGV_Data.CFork.tForkData.bEnable == true)
            {
                if (DoForkSet_ForthDepth(DeliverData.tAGV_Data.CFork.tForkData.distanceDepth))
                    DeliverData.tAGV_Data.tSensorData.tForkInputData.distanceDepth = DeliverData.tAGV_Data.CFork.tForkData.distanceDepth;
            }

            //執行收貨叉
            if (DeliverData.tAGV_Data.CFork.tForkData.ucStatus == (byte)rtForkCtrl.ForkStatus.RESET_DEPTH &&
                 DeliverData.tAGV_Data.CFork.tForkData.bEnable == true)
            {
                if (DoForkSet_BackDepth(DeliverData.tAGV_Data.CFork.tForkData.distanceDepth))
                    DeliverData.tAGV_Data.tSensorData.tForkInputData.distanceDepth = DeliverData.tAGV_Data.CFork.tForkData.distanceDepth;
            }

            //執行貨叉抬降動作
            if ((DeliverData.tAGV_Data.CFork.tForkData.ucStatus == (byte)rtForkCtrl.ForkStatus.PICKUP || DeliverData.tAGV_Data.CFork.tForkData.ucStatus == (byte)rtForkCtrl.ForkStatus.PICKDOWN) &&
                 DeliverData.tAGV_Data.CFork.tForkData.bEnable == true)
            {
                if (setForkHeight(DeliverData.tAGV_Data.CFork.tForkData.height))
                {
                    DeliverData.tAGV_Data.tSensorData.tForkInputData.distanceDepth = 4500;
                    ForkisRunnung = false;
                }
            }
            Log();
        }

        public void Log()
        {
            //Log資料新增
            if (GlobalVar.isLog)
            {
                int TempPathIndex = 0;
                byte TempPathucStatus = 0;
                byte TempPathTurnType = 0;
                rtVector TempPathSrc = new rtVector();
                rtVector TempPathDst = new rtVector();

                if (DeliverData.tAGV_Data.atPathInfo != null)
                {
                    if (DeliverData.tAGV_Data.atPathInfo.Length > 0)
                    {
                        TempPathIndex = DeliverData.tAGV_Data.CMotor.tMotorData.lPathNodeIndex;
                        TempPathucStatus = DeliverData.tAGV_Data.atPathInfo[TempPathIndex].ucStatus;
                        TempPathSrc = DeliverData.tAGV_Data.atPathInfo[TempPathIndex].tSrc;
                        TempPathDst = DeliverData.tAGV_Data.atPathInfo[TempPathIndex].tDest;
                        TempPathTurnType = DeliverData.tAGV_Data.atPathInfo[TempPathIndex].ucTurnType;
                    }
                }

                LocateData = new rtCarData();
                rtMotorCtrl PIDAns = new rtMotorCtrl();
                LocateData = DeliverData.tAGV_Data.tCarInfo;
                PIDAns = DeliverData.tAGV_Data.CMotor;

                LogCarInfo.Add(LocateData.eAngle + "," + LocateData.eWheelAngle + "," + Math.Round(LocateData.tPosition.eX, 2) + "," + Math.Round(LocateData.tPosition.eY, 2) + "," + Math.Round(LocateData.tCarTirepositionR.eX, 2) + "," +
                    Math.Round(LocateData.tCarTirepositionR.eY, 2) + "," + Math.Round(LocateData.tCarTirepositionL.eX, 2) + "," + Math.Round(LocateData.tCarTirepositionL.eY, 2) + "," + Math.Round(LocateData.tMotorPosition.eX, 2) + "," +
                    Math.Round(LocateData.tMotorPosition.eY, 2) + "," + LocateData.eCarTireSpeedLeft + "," + LocateData.eCarTireSpeedRight);

                LogMainData.Add(DeliverData.tAGV_Data.ucAGV_Status + "," + DeliverData.tAGV_Data.CFork.tForkData.ucStatus + "," + PIDAns.tMotorData.ePathError + "," + PIDAns.tMotorData.eDistance2Dest + "," + PIDAns.tMotorData.lMotorAngle + "," +
                    PIDAns.tMotorData.lMotorPower + "," + PIDAns.tMotorData.bFinishFlag + "," + TempPathIndex + "," + TempPathucStatus + "," + TempPathSrc.eX + "/" + TempPathSrc.eY + "," + TempPathDst.eX + "/" + TempPathDst.eY +
                    "," + "0" + "," + TempPathTurnType.ToString());

                LogDetailData.Add(Math.Round(PIDAns.tMotorData.Debug_ePathThetaError, 2) + "," + Math.Round(PIDAns.tMotorData.Debug_TargetAngleOffset1, 2) + "," + Math.Round(PIDAns.tMotorData.Debug_TargetAngleOffset2, 2) +
                    "," + PIDAns.tMotorData.Debug_CenterSpeed + "," + Math.Round(PIDAns.tMotorData.Debug_eDeltaCarAngle, 2) + "," + PIDAns.tMotorData.bOverDest + "," + PIDAns.tMotorData.bBackWard + "," + PIDAns.tMotorData.lNavigateOffset);
            }
        }


        public static bool DoForkSet_ForthDepth(int distance)
        {
            //執行伸貨叉
            if (Set_ForthDepthCount < distance)
            {
                if (Set_ForthDepthCount < distance)
                {
                    obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "1", 3250);
                    //Console.WriteLine("SetForth");
                    Set_ForthDepthCount += 50;
                }
                else Set_ForthDepthCount += 50;
                return false;
            }
            else
            {
                //貨插-停
                obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "1", 2000);
                Console.WriteLine("Finish SetForth");
                return true;
            }
        }

        public static bool DoForkSet_BackDepth(int distance)
        {
            //執行縮貨叉
            if (Set_BackDepthCount < 4500)
            {
                obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "1", 750);
            }
            if (Set_BackDepthCount >= 4500)
            {
                obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "1", 2000);
                Console.WriteLine("Finish ResetForth");
                return true;
            }
            Set_BackDepthCount += 50;
            return false;
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (DeliverThread == null) return;

            //結束原本執行緒
            DeliverThread.Abort();
            //執行暫停執行緒
            DeliverThread = new Thread(ThreatPara.ThreadPause);
            DeliverThread.Start();

            //貨叉停止
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "1", 2000);

            //高度停止
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_MR, "111", 1);

            //參數歸零
            Set_ForthDepthCount = 0;
            Set_BackDepthCount = 0;

            GlobalVar.isPauseStart = true;
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            if (DeliverThread == null) return;

            //高度解鎖
            obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_MR, "111", 0);
            ForkisRunnung = false;

            //結束原本執行緒
            DeliverThread.Abort();
            //執行繼續執行緒
            DeliverThread = new Thread(ThreatPara.ThreadContinue);
            DeliverThread.Start();
        }

        private void InitConfig()
        {
            //rtAGV_Control宣告
            DeliverData = new rtAGV_Control();
            ThreatPara = new ThreadWithState();
            GlobalVar.CurrentPosition = new NavigationInfo();

            //宣告使用到的Region數量
            DeliverData.tAGV_Cfg.tMapCfg.alRegionNum = 1;
            DeliverData.tAGV_Cfg.tMapCfg.Init();

            //宣告有幾個Region
            DeliverData.tAGV_Cfg.tMapCfg.alNodeNumLocal = new int[1];

            //Region裡面分別的節點數量
            DeliverData.tAGV_Cfg.tMapCfg.alNodeNumLocal[0] = 5;

            //宣告節點使用到的空間
            DeliverData.tAGV_Cfg.tMapCfg.atNodeLocal = new rtAGV_MAP_node[1][];
            DeliverData.tAGV_Cfg.tMapCfg.atNodeLocal[0] = new rtAGV_MAP_node[5];

            //節點座標資訊
            DeliverData.tAGV_Cfg.tMapCfg.atNodeLocal[0][0].tCoordinate.eX = 5000;
            DeliverData.tAGV_Cfg.tMapCfg.atNodeLocal[0][0].tCoordinate.eY = -3500;
            DeliverData.tAGV_Cfg.tMapCfg.atNodeLocal[0][0].tNodeId.lRegion = 0;
            DeliverData.tAGV_Cfg.tMapCfg.atNodeLocal[0][0].tNodeId.lIndex = 0;

            DeliverData.tAGV_Cfg.tMapCfg.atNodeLocal[0][1].tCoordinate.eX = 5000;
            DeliverData.tAGV_Cfg.tMapCfg.atNodeLocal[0][1].tCoordinate.eY = -950;
            DeliverData.tAGV_Cfg.tMapCfg.atNodeLocal[0][1].tNodeId.lRegion = 0;
            DeliverData.tAGV_Cfg.tMapCfg.atNodeLocal[0][1].tNodeId.lIndex = 1;

            DeliverData.tAGV_Cfg.tMapCfg.atNodeLocal[0][2].tCoordinate.eX = 17100;
            DeliverData.tAGV_Cfg.tMapCfg.atNodeLocal[0][2].tCoordinate.eY = -950;
            DeliverData.tAGV_Cfg.tMapCfg.atNodeLocal[0][2].tNodeId.lRegion = 0;
            DeliverData.tAGV_Cfg.tMapCfg.atNodeLocal[0][2].tNodeId.lIndex = 2;

            DeliverData.tAGV_Cfg.tMapCfg.atNodeLocal[0][3].tCoordinate.eX = 8400;
            DeliverData.tAGV_Cfg.tMapCfg.atNodeLocal[0][3].tCoordinate.eY = -950;
            DeliverData.tAGV_Cfg.tMapCfg.atNodeLocal[0][3].tNodeId.lRegion = 0;
            DeliverData.tAGV_Cfg.tMapCfg.atNodeLocal[0][3].tNodeId.lIndex = 3;

            DeliverData.tAGV_Cfg.tMapCfg.atNodeLocal[0][4].tCoordinate.eX = 11550;
            DeliverData.tAGV_Cfg.tMapCfg.atNodeLocal[0][4].tCoordinate.eY = -950;
            DeliverData.tAGV_Cfg.tMapCfg.atNodeLocal[0][4].tNodeId.lRegion = 0;
            DeliverData.tAGV_Cfg.tMapCfg.atNodeLocal[0][4].tNodeId.lIndex = 4;
            //11550

            //宣告貨物空間
            DeliverData.tAGV_Cfg.atWarehousingCfg = new rtWarehousingInfo[1][];
            DeliverData.tAGV_Cfg.atWarehousingCfg[0] = new rtWarehousingInfo[4];

            //貨物資訊
            DeliverData.tAGV_Cfg.atWarehousingCfg[0][0].eHeight = 51;
            DeliverData.tAGV_Cfg.atWarehousingCfg[0][0].DistanceDepth = 4500;

            DeliverData.tAGV_Cfg.atWarehousingCfg[0][1].eHeight = 110;
            DeliverData.tAGV_Cfg.atWarehousingCfg[0][1].DistanceDepth = 4500;

            DeliverData.tAGV_Cfg.atWarehousingCfg[0][2].eHeight = 110;
            DeliverData.tAGV_Cfg.atWarehousingCfg[0][2].DistanceDepth = 4500;

            DeliverData.tAGV_Cfg.atWarehousingCfg[0][3].eHeight = 110;
            DeliverData.tAGV_Cfg.atWarehousingCfg[0][3].DistanceDepth = 4500;

            //貨物地標
            DeliverData.tAGV_Cfg.atWarehousingCfg[0][0].tCoordinate.eX = 17050;
            DeliverData.tAGV_Cfg.atWarehousingCfg[0][0].tCoordinate.eY = 120;
            DeliverData.tAGV_Cfg.atWarehousingCfg[0][0].eDirection = 90;

            DeliverData.tAGV_Cfg.atWarehousingCfg[0][1].tCoordinate.eX = 8400;
            DeliverData.tAGV_Cfg.atWarehousingCfg[0][1].tCoordinate.eY = 120;
            DeliverData.tAGV_Cfg.atWarehousingCfg[0][1].eDirection = 90;

            DeliverData.tAGV_Cfg.atWarehousingCfg[0][2].tCoordinate.eX = 5000;
            DeliverData.tAGV_Cfg.atWarehousingCfg[0][2].tCoordinate.eY = -4000;
            DeliverData.tAGV_Cfg.atWarehousingCfg[0][2].eDirection = 90;

            DeliverData.tAGV_Cfg.atWarehousingCfg[0][3].tCoordinate.eX = 11550;
            DeliverData.tAGV_Cfg.atWarehousingCfg[0][3].tCoordinate.eY = 120;
            DeliverData.tAGV_Cfg.atWarehousingCfg[0][3].eDirection = 90;

            //貨物對應到的節點索引
            DeliverData.tAGV_Cfg.atWarehousingCfg[0][0].tNodeId.lIndex = 2;
            DeliverData.tAGV_Cfg.atWarehousingCfg[0][0].tNodeId.lRegion = 0;
            DeliverData.tAGV_Cfg.atWarehousingCfg[0][1].tNodeId.lIndex = 3;
            DeliverData.tAGV_Cfg.atWarehousingCfg[0][1].tNodeId.lRegion = 0;
            DeliverData.tAGV_Cfg.atWarehousingCfg[0][2].tNodeId.lIndex = 0;
            DeliverData.tAGV_Cfg.atWarehousingCfg[0][2].tNodeId.lRegion = 0;
            DeliverData.tAGV_Cfg.atWarehousingCfg[0][3].tNodeId.lIndex = 4;
            DeliverData.tAGV_Cfg.atWarehousingCfg[0][3].tNodeId.lRegion = 0;

            //路徑權重宣告
            DeliverData.tAGV_Cfg.tMapCfg.alPathTableLocal = new int[1][];

            DeliverData.tAGV_Cfg.tMapCfg.alPathTableLocal[0] = new int[] 
            {   0, 2550, 99999, 99999, 99999,
                2550, 0, 12080, 3400 , 6550,
                99999, 12080, 0, 8680, 5550,
                99999,  3400, 8680, 0, 3150,
                99999,  6550, 5550, 3150, 0 
            };
        }

        private rtAGV_communicate ConnectToServer;
        private void button4_Click(object sender, EventArgs e)
        {
            ConnectToServer = new rtAGV_communicate();
            ConnectToServer.ServerIP = "192.168.1.106";
            ConnectToServer.Port = 6101;
            if (ConnectToServer.ConnectToServerFunc())
            {
                Console.WriteLine("連線成功");
                txtReceiveServer.Text = DateTime.Now + ":連線成功" + "\r\n" + txtReceiveServer.Text;
                //開執行序監聽資料
                Thread DoThread = new Thread(new ParameterizedThreadStart(DoListenFunction));
                DoThread.Start();
            }
            else
            {
                Console.WriteLine("連線失敗");
                txtReceiveServer.Text = DateTime.Now + ":連線失敗" + "\r\n" + txtReceiveServer.Text;
            }

            if (DeliverFlowTimer.Enabled == false)//開啟重量timer
            {
                if (DeliverFlowTimer.Interval != 40)
                {
                    DeliverFlowTimer.Interval = 40;
                    DeliverFlowTimer.Elapsed += new System.Timers.ElapsedEventHandler(UpdataDeliverFlowData);
                }
            }
        }

        public void DoListenFunction(object num) //監聽回傳資料
        {
            if (ConnectToServer.sender_TCP.Connected)
            {
                while (true && ConnectToServer.sender_TCP != null)
                {
                    int bytesRec = ConnectToServer.sender_TCP.Receive(ConnectToServer.Receivebytes);
                    if (bytesRec == 8) //確定為server傳過來的指令
                    {
                        byte[] TempBuffer = new byte[bytesRec];
                        byte[] BufferReturn = new byte[bytesRec + 2];
                        Array.Copy(ConnectToServer.Receivebytes, TempBuffer, bytesRec);
                        Array.Copy(ConnectToServer.Receivebytes, BufferReturn, bytesRec);
                        if (ConnectToServer.CRC_CheckReceiveData(TempBuffer))
                        {
                            //回傳Server OK
                            BufferReturn[8] = 79;
                            BufferReturn[9] = 75;
                            ConnectToServer.SendData(BufferReturn);
                            //將接收到資料轉換為Command
                            ConvertByteToCommand(TempBuffer);
                            UpdateClient_UI(BufferReturn[0].ToString(), txtReceiveServer);
                        }
                        else
                        {
                            //回傳Server NG
                            BufferReturn[8] = 78;
                            BufferReturn[9] = 71;
                            ConnectToServer.SendData(BufferReturn);
                        }
                    }
                    ConnectToServer.tAGV_Data = DeliverData.tAGV_Data;
                    ConnectToServer.Send_AGV_InfoToServer();
                }
            }
        }

        //Update client端UI
        private delegate void CallBack_Client_UI(string Data, TextBox txtReceive);

        private void UpdateClient_UI(string Data, TextBox txtReceive) //更新UI
        {
            if (this.InvokeRequired)
            {
                CallBack_Client_UI DoUpdate = new CallBack_Client_UI(UpdateClient_UI);
                this.Invoke(DoUpdate, Data, txtReceive);
            }
            else
            {
                txtReceive.Text = DateTime.Now + ":收到指令: " + Data + "\r\n" + txtReceive.Text;
            }
        }

        public void ConvertByteToCommand(byte[] Receivebytes)
        {
            byte[] ByteCommand = new byte[8];
            ByteCommand[0] = Receivebytes[7];
            ByteCommand[1] = Receivebytes[6];
            ByteCommand[2] = Receivebytes[5];
            ByteCommand[3] = Receivebytes[4];
            ByteCommand[4] = Receivebytes[3];
            ByteCommand[5] = Receivebytes[2];
            ByteCommand[6] = Receivebytes[1];
            ByteCommand[7] = Receivebytes[0];

            for (int i = 0; i < 8; i++) Console.Write(Receivebytes[i] + ",");
            Console.WriteLine("");
            ulong LongCmd = BitConverter.ToUInt64(ByteCommand, 0);
            DoReceiveCommand(Receivebytes, LongCmd);
        }

        public void DoReceiveCommand(byte[] Receivebytes, ulong uLongCommand)
        {
            if (Receivebytes[0] == 0x03) //暫停
            {
                if (DeliverThread != null)
                {
                    //停止貨叉動作
                    DeliverData.tAGV_Data.CFork.tForkData.bEnable = false;

                    //結束原本執行緒
                    DeliverThread.Abort();
                    ThreatPara.agv_Command = uLongCommand;
                    //執行暫停執行緒
                    DeliverThread = new Thread(ThreatPara.DoCommandProcess);
                    DeliverThread.Start();

                    for (int i = 0; i < 3; i++)
                    {
                        //貨叉停止
                        obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_DM, "1", 2000);
                        Task.Delay(1);
                        //高度停止
                        obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_MR, "111", 1);
                        Task.Delay(1);
                    }
                    //參數歸零
                    Set_ForthDepthCount = 0;
                    Set_BackDepthCount = 0;
                    GlobalVar.isPauseStart = true;
                }
            }

            if (Receivebytes[0] == 0x02) //恢復
            {
                if (DeliverThread == null) return;

                //高度解鎖
                Set_ForthDepthCount = 0;
                Set_BackDepthCount = 0;
                for (int i = 0; i < 3; i++)
                {
                    obj_PLC.doWriteDevice(DATABUILDERAXLibLB.DBPlcDevice.DKV3000_MR, "111", 0);
                    Task.Delay(1);
                }
                ForkisRunnung = false;

                //結束原本執行緒
                DeliverThread.Abort();
                ThreatPara.agv_Command = uLongCommand;
                //執行繼續執行緒
                DeliverThread = new Thread(ThreatPara.DoCommandProcess);
                DeliverThread.Start();
            }

            if (Receivebytes[0] == 0x05 || Receivebytes[0] == 0x06) //Load and UnLoad
            {
                ThreatPara.agv_Command = uLongCommand;
                DeliverThread = new Thread(ThreatPara.DoCommandProcess);
                DeliverThread.Start();
            }

            if (Receivebytes[0] == 0x07) //Park
            {
                ThreatPara.agv_Command = uLongCommand;
                DeliverThread = new Thread(ThreatPara.DoCommandProcess);
                DeliverThread.Start();
            }
        }

        private void btnDoCmd_Click(object sender, EventArgs e)
        {
            if (txtAgvID.Text == "" || txtSrcRegion.Text == "" || txtSrcPosition.Text == "" || txtDstPosition.Text == "" || txtDstRegion.Text == "")
            {
                MessageBox.Show("請輸入完整資訊");
                return;
            }

            if (DeliverData == null) return;
            if (DeliverThread != null) DeliverThread.Abort();

            byte[] Cmdbytes = new byte[8];
            Cmdbytes[0] = 0x00;
            Cmdbytes[1] = 0x00;
            Cmdbytes[2] = 0x00;
            Cmdbytes[3] = Convert.ToByte(txtDstPosition.Text);
            Cmdbytes[4] = Convert.ToByte(txtDstRegion.Text);
            Cmdbytes[5] = Convert.ToByte(txtSrcPosition.Text);
            Cmdbytes[6] = Convert.ToByte(txtSrcRegion.Text);
            Cmdbytes[7] = Convert.ToByte(txtAgvID.Text);
            ulong LongCmd = BitConverter.ToUInt64(Cmdbytes, 0);

            if (DeliverFlowTimer.Enabled == false)//開啟更新車體資訊Timer
            {
                if (DeliverFlowTimer.Interval != 40)
                {
                    DeliverFlowTimer.Interval = 40;
                    DeliverFlowTimer.Elapsed += new System.Timers.ElapsedEventHandler(UpdataDeliverFlowData);
                }
                TimerReceivePLC_Data.Enabled = false;//關閉一直與PLC取資訊
                DeliverFlowTimer.Enabled = true;
            }
            Set_ForthDepthCount = 0;
            Set_BackDepthCount = 0;
            UpdateDeliverData();
            ThreatPara.ThreadParameter = DeliverData;

            ThreatPara.agv_Command = LongCmd;
            DeliverThread = new Thread(ThreatPara.DoCommandProcess);
            DeliverThread.Start();

            if (Cmdbytes[7] == 0x05)
                txtReceiveServer.Text = DateTime.Now.ToString() + "-執行Load指令 \r\n" + txtReceiveServer.Text;
            else if (Cmdbytes[7] == 0x06)
                txtReceiveServer.Text = DateTime.Now.ToString() + "-執行UnLoad指令 \r\n" + txtReceiveServer.Text;
            else if (Cmdbytes[7] == 0x07)
                txtReceiveServer.Text = DateTime.Now.ToString() + "-執行Park指令 \r\n" + txtReceiveServer.Text;
            else if (Cmdbytes[7] == 0x02)
                txtReceiveServer.Text = DateTime.Now.ToString() + "-執行Continue指令 \r\n" + txtReceiveServer.Text;
            else if (Cmdbytes[7] == 0x03)
                txtReceiveServer.Text = DateTime.Now.ToString() + "-執行Pause指令 \r\n" + txtReceiveServer.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (DeliverData == null) return;
            if (DeliverThread != null) DeliverThread.Abort();

            if (DeliverFlowTimer.Enabled == false)//開啟更新車體資訊Timer
            {
                if (DeliverFlowTimer.Interval != 40)
                {
                    DeliverFlowTimer.Interval = 40;
                    DeliverFlowTimer.Elapsed += new System.Timers.ElapsedEventHandler(UpdataDeliverFlowData);
                }
                TimerReceivePLC_Data.Enabled = false;//關閉一直與PLC取資訊
                DeliverFlowTimer.Enabled = true;
            }
            Set_ForthDepthCount = 0;
            Set_BackDepthCount = 0;
            UpdateDeliverData();
            ThreatPara.ThreadParameter = DeliverData;

            DeliverThread = new Thread(ThreatPara.DoFlowFristProcess);
            DeliverThread.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (DeliverData == null) return;
            if (DeliverThread != null) DeliverThread.Abort();

            if (DeliverFlowTimer.Enabled == false)//開啟更新車體資訊Timer
            {
                if (DeliverFlowTimer.Interval != 40)
                {
                    DeliverFlowTimer.Interval = 40;
                    DeliverFlowTimer.Elapsed += new System.Timers.ElapsedEventHandler(UpdataDeliverFlowData);
                }
                TimerReceivePLC_Data.Enabled = false;//關閉一直與PLC取資訊
                DeliverFlowTimer.Enabled = true;
            }
            Set_ForthDepthCount = 0;
            Set_BackDepthCount = 0;
            UpdateDeliverData();
            ThreatPara.ThreadParameter = DeliverData;

            DeliverThread = new Thread(ThreatPara.DoFlowSecondProcess);
            DeliverThread.Start();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (DeliverData == null) return;
            if (DeliverThread != null) DeliverThread.Abort();

            if (DeliverFlowTimer.Enabled == false)//開啟更新車體資訊Timer
            {
                if (DeliverFlowTimer.Interval != 40)
                {
                    DeliverFlowTimer.Interval = 40;
                    DeliverFlowTimer.Elapsed += new System.Timers.ElapsedEventHandler(UpdataDeliverFlowData);
                }
                TimerReceivePLC_Data.Enabled = false;//關閉一直與PLC取資訊
                DeliverFlowTimer.Enabled = true;
            }
            Set_ForthDepthCount = 0;
            Set_BackDepthCount = 0;
            UpdateDeliverData();
            ThreatPara.ThreadParameter = DeliverData;

            DeliverThread = new Thread(ThreatPara.DoFlowThirdProcess);
            DeliverThread.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (DeliverData == null) return;
            if (DeliverThread != null) DeliverThread.Abort();

            if (DeliverFlowTimer.Enabled == false)//開啟更新車體資訊Timer
            {
                if (DeliverFlowTimer.Interval != 40)
                {
                    DeliverFlowTimer.Interval = 40;
                    DeliverFlowTimer.Elapsed += new System.Timers.ElapsedEventHandler(UpdataDeliverFlowData);
                }
                TimerReceivePLC_Data.Enabled = false;//關閉一直與PLC取資訊
                DeliverFlowTimer.Enabled = true;
            }
            Set_ForthDepthCount = 0;
            Set_BackDepthCount = 0;
            UpdateDeliverData();
            ThreatPara.ThreadParameter = DeliverData;

            DeliverThread = new Thread(ThreatPara.DoFlowForthProcess);
            DeliverThread.Start();
        }

        //宣告Text_Debug 的User Control
        private Text_Debug Text_DebugSetting;

        //宣告Log_Debug 的User Control
        private Log_Debug Log_DebugSetting;

        private void 主要控制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelDebug.Visible = false;
        }

        private void log檔除錯ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelDebug.Controls.Clear();
            Log_DebugSetting = new Log_Debug();
            Log_DebugSetting.ShowLogDebugPanel = panelDebug;
            Log_DebugSetting.Location = new Point(0, 0);
            Log_DebugSetting.Size = new Size(panelDebug.Width, panelDebug.Height);
            Log_DebugSetting.Parent = panelDebug;
            panelDebug.Controls.Add(Log_DebugSetting);
            panelDebug.Visible = true;
        }

        private void 文字輸入除錯ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelDebug.Controls.Clear();
            Text_DebugSetting = new Text_Debug();
            Text_DebugSetting.ShowTextDebugPanel = panelDebug;
            Text_DebugSetting.Location = new Point(0, 0);
            Text_DebugSetting.Size = new Size(panelDebug.Width, panelDebug.Height);
            Text_DebugSetting.Parent = panelDebug;
            panelDebug.Controls.Add(Text_DebugSetting);
            panelDebug.Visible = true;
        }

        private void 關於ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aboutUS frm = new aboutUS();
            frm.ShowDialog(this);
        }
    }

    public class aboutUS : Form
    {
        PictureBox pictureBox_boltun_logo = new PictureBox();
        Label label_boltun = new Label();
        Label label_aboutUS = new Label();
        Label label_caveat = new Label();
        Button buttonOK = new Button();

        public aboutUS()
        {
            Text = "關於";
            label_boltun.Text = "關於：恒耀工業股份有限公司";
            label_boltun.SetBounds(9, 150, 372, 50);
            label_boltun.Font = new Font(label_boltun.Font.FontFamily, 14);

            label_aboutUS.Text = GlobalVar.aboutUS;
            label_aboutUS.SetBounds(9, 200, 372, 50);
            label_aboutUS.Font = new Font(label_aboutUS.Font.FontFamily, 14);

            label_caveat.Text = "警告：本程式受版權法和國際條約保護。未經許可擅自複製或分發程式或其中任何部分可能導致嚴重的民事及刑事懲罰，並會根據法律予以最嚴重的起訴。";
            label_caveat.SetBounds(9, 250, 372, 50);

            buttonOK.Text = "確定";
            buttonOK.DialogResult = DialogResult.OK;
            buttonOK.SetBounds(160, 320, 80, 40);
            buttonOK.Font = new Font(buttonOK.Font.FontFamily, 12);

            pictureBox_boltun_logo.Size = new Size(100, 30);
            this.Controls.Add(pictureBox_boltun_logo);

            Bitmap flag = new Bitmap(System.Environment.CurrentDirectory + "\\boltun_logo.jpg");
            pictureBox_boltun_logo.Image = flag;
            pictureBox_boltun_logo.SetBounds(25, 1, 348, 99);

            buttonOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            ClientSize = new Size(400, 370);
            Controls.AddRange(new Control[] { label_boltun, label_aboutUS, label_caveat, buttonOK, pictureBox_boltun_logo });

            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
            MinimizeBox = false;
            MaximizeBox = false;
        }
    }
}
