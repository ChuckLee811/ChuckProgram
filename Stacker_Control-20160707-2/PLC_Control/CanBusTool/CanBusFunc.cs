using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using PLC_Control;
using Others;

namespace CanBusTool
{
    public class CanBusFunc
    {
        public static UInt32 m_devtype = 4;//USBCAN2
        public static UInt32 m_bOpen = 0;

        //Channel1
        public static UInt32 m_devind = 0;
        public static UInt32 m_canind = 0;
        public static VCI_CAN_OBJ[] m_recobj = new VCI_CAN_OBJ[1000];
        public static UInt32[] m_arrdevtype = new UInt32[20];

        //Channel2
        public static UInt32 m_devind_2 = 0;
        public static UInt32 m_canind_2 = 0;
        public static VCI_CAN_OBJ[] m_recobj_2 = new VCI_CAN_OBJ[1000];
        public static UInt32[] m_arrdevtype_2 = new UInt32[20];

        /** \brief 轉向-堆高機電腦資料   */
        public static byte[] ReceiveComputer;

        /** \brief 轉向-堆高機電腦暫存資料   */
        public static byte[] ReceiveComputer_Storage;

        /** \brief 轉向-堆高機馬達資料   */
        public static byte[] ReceiveMotor;

        /** \brief 轉向-堆高機馬達暫存資料   */
        public static byte[] ReceiveMotor_Storage;

        /** \brief 暫存資料，計算Check資料用   */
        public static byte[] BufferData;

        /** \brief 轉向-給主控修改的資料   */
        public static byte[] TransData;

        /** \brief 行走-堆高機電腦資料   */
        public static byte[] ReceiveMoveComputer;

        /** \brief 行走-堆高機電腦暫存資料   */
        public static byte[] ReceiveMoveComputer_Storage;

        /** \brief 行走-堆高機馬達資料   */
        public static byte[] ReceiveMoveMotor;

        /** \brief 行走-堆高機馬達暫存資料   */
        public static byte[] ReceiveMoveMotor_Storage;

        /** \brief 行走-給主控修改的資料   */
        public static byte[] TransMoveData;

        /** \brief 監聽堆高機電腦的執行緒   */
        public static Thread ComputerThread;

        /** \brief 監聽堆高機馬達的執行緒   */
        public static Thread MotorThread;

        //手動控制
        public static bool CanControl = false;
        public static bool isSend = false;
        public static bool isMoveSend = false;
        public static bool CANBUS_TurnLeft = false;
        public static bool CANBUS_TurnRight = false;

        /// <summary>
        /// CanBus初始數據
        /// </summary>
        public static void CanInit()
        {
            ReceiveComputer = new byte[8];
            ReceiveMotor = new byte[8];
            ReceiveComputer_Storage = new byte[8];
            ReceiveMotor_Storage = new byte[8];
            BufferData = new byte[8];
            TransData = new byte[8];
            TransData[0] = 0xca;
            TransData[1] = 0x01;
            TransData[4] = GlobalVar.RotateSpeed;
            TransData[5] = GlobalVar.RotateSpeed;

            //行走馬達儲存空間宣告
            ReceiveMoveComputer = new byte[8];
            ReceiveMoveComputer_Storage = new byte[8];
            ReceiveMoveMotor = new byte[8];
            ReceiveMoveMotor_Storage = new byte[8];
            TransMoveData = new byte[8];
        }

        /// <summary>
        /// CanBus連線
        /// </summary>
        /// <returns> 連線成功回傳1，否則回傳0</returns>
        public static int CanConnect()
        {
            //開始連線
            if (m_bOpen == 1)
            {
                CanBusTool.VCI_CloseDevice(m_devtype, m_devind);
                m_bOpen = 0;
            }
            else
            {
                //開啟第一個device，用於轉向馬達
                m_devtype = m_arrdevtype[1];
                m_devind = 0;
                m_canind = 0;
                m_devind_2 = 0;
                m_canind_2 = 1;

                if (CanBusTool.VCI_OpenDevice(m_devtype, m_devind, 0) == 0)
                {
                    return 0;
                }

                m_bOpen = 1;
                VCI_INIT_CONFIG config = new VCI_INIT_CONFIG();
                config.AccCode = System.Convert.ToUInt32("0x" + "40e00000", 16);
                config.AccMask = System.Convert.ToUInt32("0x" + "FFFFFFFF", 16);
                /*config.AccCode = System.Convert.ToUInt32("0x" + "41600000", 16);
                config.AccMask = System.Convert.ToUInt32("0x" + "0FFFFFFF", 16);*/
                config.Timing0 = System.Convert.ToByte("0x" + "03", 16);
                config.Timing1 = System.Convert.ToByte("0x" + "1c", 16);
                config.Filter = (Byte)(1);
                config.Mode = (Byte)0;

                VCI_INIT_CONFIG config2 = new VCI_INIT_CONFIG();
                config2.AccCode = System.Convert.ToUInt32("0x" + "30e00000", 16);
                config2.AccMask = System.Convert.ToUInt32("0x" + "FFFFFFFF", 16);
                /*config2.AccCode = System.Convert.ToUInt32("0x" + "31600000", 16);
                config2.AccMask = System.Convert.ToUInt32("0x" + "0FFFFFFF", 16);*/
                config2.Timing0 = System.Convert.ToByte("0x" + "03", 16);
                config2.Timing1 = System.Convert.ToByte("0x" + "1c", 16);
                config2.Filter = (Byte)(1);
                config2.Mode = (Byte)0;

                CanBusTool.VCI_InitCAN(m_devtype, m_devind, m_canind, ref config);
                CanBusTool.VCI_InitCAN(m_devtype, m_devind_2, m_canind_2, ref config2);

                ComputerThread = new Thread(StartComputerMonitorThread);
                MotorThread = new Thread(StartMotorMonitorThread);

            }
            if (m_bOpen == 1) return 1;//CAN連線
            else return 0;//CAN斷線
        }

        /// <summary>
        /// 開啟CanBus通訊
        /// </summary>
        /// <returns>連線成功回傳1，否則回傳0</returns>
        public static int OpenCan()
        {
            //開啟CanBus
            if (m_bOpen == 0) return 0;
            CanBusTool.VCI_StartCAN(m_devtype, m_devind, m_canind);
            CanBusTool.VCI_StartCAN(m_devtype, m_devind_2, m_canind_2);

            ComputerThread.Start();
            MotorThread.Start();
            if (m_bOpen == 1) return 1;
            else return 0;
        }

        /// <summary>
        /// 關閉CanBus通訊
        /// </summary>
        public static void ResetCan()
        {
            if (m_bOpen == 0) return;
            CanBusTool.VCI_ResetCAN(m_devtype, m_devind, m_canind);
            CanBusTool.VCI_ResetCAN(m_devtype, m_devind_2, m_canind_2);
        }

        /// <summary>
        /// 攔截堆高機電腦傳送過來資料，修改後傳送給馬達 
        /// </summary>
        unsafe private static void  StartComputerMonitorThread()
        {
            while (true)
            {
                UInt32 res = new UInt32();
                res = CanBusTool.VCI_Receive(m_devtype, m_devind, m_canind, ref m_recobj[0], 1000, 100);
                if (res > 10000 || res < 0) return;
                for (UInt32 i = 0; i < res; i++)
                {
                    if (!CanControl) //初始化
                    {
                        //收到電腦傳送過來資料，直接傳送給馬達 
                        VCI_CAN_OBJ recobj = m_recobj[i];
                        string StrID = Convert.ToString(recobj.ID, 16);
                        //Console.WriteLine("ReceiveComputerID: " + StrID);

                        byte Datalen = (byte)(m_recobj[i].DataLen % 9);
                        byte[] TransData = new byte[Datalen];
                        for (int k = 0; k < TransData.Length; k++) TransData[k] = recobj.Data[k];
                        SendCommandToMotor(recobj.ID, Datalen, TransData);
                        //ID 207 轉向馬達
                        if (recobj.ID == 519)
                        {
                            Array.Copy(TransData, ReceiveComputer_Storage, TransData.Length);
                            Array.Copy(TransData, ReceiveComputer, TransData.Length);
                        }
                        //ID 20B 行走馬達
                        if (recobj.ID == 523)
                        {
                            Array.Copy(TransData, ReceiveMoveComputer_Storage, TransData.Length);
                            Array.Copy(TransData, ReceiveMoveComputer, TransData.Length);
                        }
                    }
                    else //開始介入
                    {
                        VCI_CAN_OBJ recobj = m_recobj[i];
                        string StrID = Convert.ToString(recobj.ID, 16);
                        byte Datalen = (byte)(recobj.DataLen % 9);
                        //收到ID 207
                        if (recobj.ID == 519) //轉向馬達站號
                        {
                            //將上次傳送結果複製給比較buffer
                            Array.Copy(ReceiveComputer, BufferData, BufferData.Length);
                            //更改ID207資料
                            if (isSend)
                            {
                                Array.Copy(TransData, ReceiveComputer, TransData.Length);//傳送資料
                                isSend = false;
                            }
                            //計算ID207的Check的資料
                            CalCheckData();
                            //傳送指令給馬達
                            SendCommandToMotor(recobj.ID, ReceiveComputer.Length, ReceiveComputer);
                            //回傳給電腦虛擬馬達ID187資料
                            SendCommand(391, ReceiveMotor_Storage.Length, ReceiveMotor_Storage);
                        }
                        if (recobj.ID == 523) //行走馬達站號
                        {
                            //修改ID523資料
                            if (isMoveSend)
                            {
                                Array.Copy(TransMoveData, ReceiveMoveComputer, TransMoveData.Length);//傳送資料
                                isMoveSend = false;
                            }
                            //傳送指令給馬達
                            SendCommandToMotor(recobj.ID, ReceiveMoveComputer.Length, ReceiveMoveComputer);
                            //回傳給電腦虛擬馬達ID18B資料
                            SendCommand(395, ReceiveMoveMotor_Storage.Length, ReceiveMoveMotor_Storage);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 收到堆高機馬達傳送過來資料，修改後傳送給電腦 
        /// </summary>
        unsafe public static void StartMotorMonitorThread()
        {
            while (true)
            {
                UInt32 res = new UInt32();
                res = CanBusTool.VCI_Receive(m_devtype, m_devind_2, m_canind_2, ref m_recobj_2[0], 1000, 100);
                if (res > 10000 || res < 0) return;
                if (!CanControl) //初始化
                {
                    for (UInt32 i = 0; i < res; i++)
                    {
                        //收到馬達傳送過來資料，傳送給電腦 
                        VCI_CAN_OBJ recobj = m_recobj_2[i];
                        byte Datalen = (byte)(m_recobj_2[i].DataLen % 9);
                        byte[] TransData = new byte[Datalen];
                        for (int k = 0; k < TransData.Length; k++) TransData[k] = recobj.Data[k];
                        SendCommand(recobj.ID, Datalen, TransData);
                        if (recobj.ID == 391) //ID187 轉向站號
                        {
                            Array.Copy(TransData, ReceiveMotor, TransData.Length);
                            Array.Copy(TransData, ReceiveMotor_Storage, TransData.Length);
                        }
                        if (recobj.ID == 395) //ID18B 行走站號
                        {
                            Array.Copy(TransData, ReceiveMoveMotor, TransData.Length);
                            Array.Copy(TransData, ReceiveMoveMotor_Storage, TransData.Length);
                        }
                    }
                }
                else
                {
                    if (GlobalVar.isCanBusDebug) //記錄回傳角度資訊
                    {
                        for (UInt32 i = 0; i < res; i++)
                        {
                            VCI_CAN_OBJ recobj = m_recobj_2[i];

                            //分析馬達回傳資料
                            AnalysisMotorData(recobj, i);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 解析馬達回傳資料
        /// </summary>
        /// <param name="recobj">[IN] recobj 參數</param>
        /// <param name="i">[IN] Index</param>
        unsafe public static void AnalysisMotorData(VCI_CAN_OBJ recobj, uint i)
        {
            if (recobj.ID == 391) //ID187 轉向站號
            {
                byte Datalen = (byte)(m_recobj_2[i].DataLen % 9);
                int LowByte = Convert.ToInt16(recobj.Data[4]);
                int HiByte = Convert.ToInt16(recobj.Data[5]);
                LowByte -= MainForm.L_Error;
                HiByte -= MainForm.H_Error;
                double LowDegree = (LowByte * 1.40625) / 255;
                double HiDegree = HiByte * 1.40625;
                double AngleTemp = LowDegree + HiDegree;
                if (AngleTemp > 180) AngleTemp = AngleTemp - 360;
                GlobalVar.RealMotorAngle = AngleTemp;
            }

            if (recobj.ID == 395) //ID187 行走站號
            {
                byte Datalen = (byte)(m_recobj_2[i].DataLen % 9);
                int LowByte = Convert.ToInt16(recobj.Data[2]);
                int HiByte = Convert.ToInt16(recobj.Data[3]);
                if (HiByte > 128)
                {
                    HiByte = 255 - HiByte;
                    LowByte = 255 - LowByte;
                    double Temp = -((double)HiByte * (double)256 + (double)LowByte) / 6.97265625;
                    if (Temp < -255) Temp = -255;
                    GlobalVar.RealMotorPower = Temp;
                }
                else
                {
                    double Temp = ((double)HiByte * (double)256 + (double)LowByte) / 6.97265625;
                    if (Temp > 255) Temp = 255;
                    GlobalVar.RealMotorPower = Temp;
                }
            }
        }

        /// <summary>
        /// 傳送資料給堆高機的電腦
        /// </summary>
        /// <param name="ID">IN] 傳送ID</param>[
        /// <param name="len">IN] 數據長度</param>[
        /// <param name="data">[IN] 傳送數據</param>
        unsafe public static void SendCommand(uint ID, int len, byte[] data)
        {
            if (m_bOpen == 0) return;
            VCI_CAN_OBJ sendobj = new VCI_CAN_OBJ();
            sendobj.RemoteFlag = 0;
            sendobj.ExternFlag = 0;
            sendobj.ID = ID;
            sendobj.DataLen = System.Convert.ToByte(len);
            for (int i = 0; i < len; i++) sendobj.Data[i] = data[i];
            CanBusTool.VCI_Transmit(m_devtype, m_devind, m_canind, ref sendobj, 1);
        }

        /// <summary>
        /// 傳送資料給堆高機的馬達
        /// </summary>
        /// <param name="ID">[IN] 傳送ID</param>
        /// <param name="len">[IN] 數據長度</param>
        /// <param name="data">[IN] 傳送數據</param>
        unsafe public static void SendCommandToMotor(uint ID, int len, byte[] data)
        {
            if (m_bOpen == 0) return;
            VCI_CAN_OBJ sendobj = new VCI_CAN_OBJ();
            sendobj.RemoteFlag = 0;
            sendobj.ExternFlag = 0;
            sendobj.ID = ID;
            sendobj.DataLen = System.Convert.ToByte(len);
            for (int i = 0; i < len; i++) sendobj.Data[i] = data[i];
            CanBusTool.VCI_Transmit(m_devtype, m_devind_2, m_canind_2, ref sendobj, 1);
        }

        /// <summary>
        /// 利用BufferData(上一筆)與ReceiveComputer(當前要傳送的資料)來計算ID 207之Check資料
        /// </summary>
        public static void CalCheckData()
        {
            int dis = 0;
            for (int i = 0; i < 6; i++)
            {
                if (ReceiveComputer[i] >= BufferData[i])
                    dis += ReceiveComputer[i] - BufferData[i];
                else
                    dis += (256 - BufferData[i] + ReceiveComputer[i]);
            }
            int value = BufferData[7] + 6 + dis;
            value = value % 256;
            int fixdata = BufferData[6] + 6;
            fixdata = fixdata % 256;
            ReceiveComputer[6] = Convert.ToByte(fixdata);
            ReceiveComputer[7] = Convert.ToByte(value);
        }
    }
}
