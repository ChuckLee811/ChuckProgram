using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace LidarTool
{
    public class LidarData
    {
        public float[] X;
        public float[] Y;
        public float[] Z;
    }

    public class LidarFunc
    {
        #region Lidar變數宣告

        //UDP連線用
        public Thread UDPThread = null;
        public Socket UDPsocket;

        //Lodar資料
        public byte[] LidarBuffer;
        public double[] VerticalAngle;
        public double[] CosVerticalAngle;
        public double[] SinVerticalAngle;

        //720分割
        public LidarData rLidarData;

        //360分割
        public LidarData r_LidarData;
        public int LidarUpdateCount = 0;

        //存放Lidar-XYZ座標的List
        public List<float> LidarWriteBufferIndex = new List<float>();
        public List<float> LidarWriteBufferX = new List<float>();
        public List<float> LidarWriteBufferY = new List<float>();
        public List<float> LidarWriteBufferZ = new List<float>();


        //表是否開始記錄Lidar資料
        public bool isLidarDataRecode = false;

        //Lidar資料總數
        public int TotalLidarCount = 0;

        //lidar開始收資料
        public bool isLidarStart = false;

        //記錄SharpGL初始游標位置
        public Point _BakMousePosition = new Point(0, 0);

        //讀資料的StreamReader
        public StreamReader LidarReader;

        //讀檔案資料的Thread
        public Thread ReadFileThread = null;

        //紀錄是否讀到檔案底部
        public bool isEOF = false;

        #endregion

        public void Init()
        {
            LidarBuffer = new byte[1248];
            VerticalAngle = new double[32];
            CosVerticalAngle = new double[32];
            SinVerticalAngle = new double[32];

            rLidarData = new LidarData();
            rLidarData.X = new float[23040];
            rLidarData.Y = new float[23040];
            rLidarData.Z = new float[23040];

            r_LidarData = new LidarData();
            r_LidarData.X = new float[11520];
            r_LidarData.Y = new float[11520];
            r_LidarData.Z = new float[11520];
        }

        public void StartLidarUDP()
        {
            //UDP光達封包接收
            try
            {
                if (isLidarStart)
                {
                    int receivedDataLength;
                    byte[] data = new byte[1248];
                    IPEndPoint ip = new IPEndPoint(IPAddress.Any, 2368);
                    UDPsocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    UDPsocket.Bind(ip);
                    EndPoint Remote = (EndPoint)(ip);
                    while (true)
                    {
                        data = new byte[1248];
                        receivedDataLength = UDPsocket.ReceiveFrom(data, ref Remote);
                        if (receivedDataLength == 1206)
                        {
                            Array.Copy(data, LidarBuffer, data.Length);
                            //LidarBufferToDistence();
                            LidarPicketToDistance();
                        }
                        if (!isLidarStart) break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lidar UPD Server Start:" + ex.Message);
            }
        }

        public void LidarBufferToDistence() //360分割
        {
            //解析封包
            double LocationX = 0;
            double LocationY = 0;
            double LocationZ = 0;
            int RotationAngle = 0;
            for (int i = 0; i < 12; i++)
            {
                int VetricalCounter = 0;
                //水平角度查表
                RotationAngle = ((LidarBuffer[i * 100 + 3]) * 256 + LidarBuffer[i * 100 + 2]) / 100;
                double CosRotationAngle = MathFunc.CosTable(RotationAngle);
                double SinRotationAngle = MathFunc.SinTable(RotationAngle);
                for (int j = 4; j < 100; j += 3)
                {
                    //距離換算
                    float Distance = (float)(LidarBuffer[i * 100 + j + 1] * 256 + LidarBuffer[i * 100 + j]) / (float)500; //換成公尺

                    //反射強度
                    int Intensity = LidarBuffer[i * 100 + j + 2];
                    //if (Intensity < 8) continue;

                    //計算座標
                    LocationX = Distance * CosVerticalAngle[VetricalCounter] * SinRotationAngle;
                    LocationY = Distance * CosVerticalAngle[VetricalCounter] * CosRotationAngle;
                    LocationZ = Distance * SinVerticalAngle[VetricalCounter];

                    //有效距離
                    if (Math.Sqrt(LocationX * LocationX + LocationY * LocationY) > 0.1)
                    {
                        //資料不等於零 更新資料
                        if (LocationX != 0 && LocationY != 0 && LocationZ != 0)
                        {
                            int index = (j - 1) / 3 - 1;
                            r_LidarData.X[RotationAngle * 32 + index] = (float)LocationX;
                            r_LidarData.Y[RotationAngle * 32 + index] = (float)LocationY;
                            r_LidarData.Z[RotationAngle * 32 + index] = (float)LocationZ;
                        }
                    }
                    VetricalCounter++;
                }
            }
        }

        public void LidarPicketToDistance() //720分割
        {
            double LocationX = 0;
            double LocationY = 0;
            double LocationZ = 0;
            int RotationAngle = 0;

            for (int i = 0; i < 12; i++)
            {
                int VetricalCounter = 0;
                //水平角度查表
                RotationAngle = ((LidarBuffer[i * 100 + 3]) * 256 + LidarBuffer[i * 100 + 2]) / 100;
                int RoIndex = (int)((double)((LidarBuffer[i * 100 + 3]) * 256 + LidarBuffer[i * 100 + 2]) / (double)200);
                double CosRotationAngle = MathFunc.CosTable(RotationAngle);
                double SinRotationAngle = MathFunc.SinTable(RotationAngle);
                for (int j = 4; j < 100; j += 3)
                {
                    //距離換算
                    float Distance = (float)(LidarBuffer[i * 100 + j + 1] * 256 + LidarBuffer[i * 100 + j]) / (float)500; //換成公尺

                    //反射強度
                    int Intensity = LidarBuffer[i * 100 + j + 2];
                    //if (Intensity < 8) continue;

                    //計算座標
                    LocationX = Distance * CosVerticalAngle[VetricalCounter] * SinRotationAngle;
                    LocationY = Distance * CosVerticalAngle[VetricalCounter] * CosRotationAngle;
                    LocationZ = Distance * SinVerticalAngle[VetricalCounter];

                    //有效距離
                    if (Math.Sqrt(LocationX * LocationX + LocationY * LocationY) > 0.1)
                    {
                        //資料不等於零 更新資料
                        if (LocationX != 0 && LocationY != 0 && LocationZ != 0)
                        {
                            int index = (j - 1) / 3 - 1;
                            rLidarData.X[RoIndex * 32 + index] = (float)LocationX;
                            rLidarData.Y[RoIndex * 32 + index] = (float)LocationY;
                            rLidarData.Z[RoIndex * 32 + index] = (float)LocationZ;
                            if (isLidarDataRecode)
                            {
                                LidarWriteBufferIndex.Add(RoIndex * 32 + index);
                                LidarWriteBufferX.Add((float)LocationX);
                                LidarWriteBufferY.Add((float)LocationY);
                                LidarWriteBufferZ.Add((float)LocationZ);
                            }
                        }
                    }
                    VetricalCounter++;
                }
            }
        }

        public void LidarDataOut()
        {
            //輸出Lidar資料
            int LidarCount = 0;
            using (StreamWriter sw = new StreamWriter("LidarData" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".ldr"))   //小寫TXT     
            {
                sw.WriteLine("ALL Lidar Informations as Follow.");
                sw.WriteLine("--------------------------------------------------------------------");
                for (int i = 0; i < LidarWriteBufferX.Count; i++)
                {
                    sw.WriteLine(LidarWriteBufferIndex[i] + "," + LidarWriteBufferX[i] + "," + LidarWriteBufferY[i] + "," + LidarWriteBufferZ[i] + ",");
                    LidarCount++;
                }
            }
            LidarWriteBufferIndex.Clear();
            LidarWriteBufferX.Clear();
            LidarWriteBufferY.Clear();
            LidarWriteBufferZ.Clear();
        }

        public void ReadLidarDataFuncThread()
        {
            while (!isEOF)
            {
                //解析檔案裡的每行資料
                string Eachline = LidarReader.ReadLine();
                if (Eachline == null)
                {
                    //讀到最後一行
                    LidarReader.Close();
                    isEOF = true;
                    return;
                }
                //每個資料依逗號分割
                string[] EachData = Eachline.Split(',');
                int DataIndex = 0;
                if (EachData[0] == "" || EachData[1] == "" || EachData[2] == "" || EachData[3] == "") return;
                DataIndex = (int)Convert.ToDouble(EachData[0]);
                rLidarData.X[DataIndex] = (float)Convert.ToDouble(EachData[1]);
                rLidarData.Y[DataIndex] = (float)Convert.ToDouble(EachData[2]);
                rLidarData.Z[DataIndex] = (float)Convert.ToDouble(EachData[3]);
            }
        }

        public void LoadLidarVariable()
        {
            //讀取Lidar固定變數
            //Lidar垂直固定角度
            VerticalAngle[0] = -30.67;
            VerticalAngle[1] = -9.33;
            VerticalAngle[2] = -29.33;
            VerticalAngle[3] = -8;
            VerticalAngle[4] = -28;
            VerticalAngle[5] = -6.66;
            VerticalAngle[6] = -26.66;
            VerticalAngle[7] = -5.33;
            VerticalAngle[8] = -25.33;
            VerticalAngle[9] = -4;
            VerticalAngle[10] = -24;
            VerticalAngle[11] = -2.67;
            VerticalAngle[12] = -22.67;
            VerticalAngle[13] = -1.33;
            VerticalAngle[14] = -21.33;
            VerticalAngle[15] = 0;
            VerticalAngle[16] = -20;
            VerticalAngle[17] = 1.33;
            VerticalAngle[18] = -18.67;
            VerticalAngle[19] = 2.67;
            VerticalAngle[20] = -17.33;
            VerticalAngle[21] = 4;
            VerticalAngle[22] = -16;
            VerticalAngle[23] = 5.33;
            VerticalAngle[24] = -14.67;
            VerticalAngle[25] = 6.67;
            VerticalAngle[26] = -13.33;
            VerticalAngle[27] = 8;
            VerticalAngle[28] = -12;
            VerticalAngle[29] = 9.33;
            VerticalAngle[30] = -10.67;
            VerticalAngle[31] = 10.67;

            //Cos垂直
            CosVerticalAngle[0] = 0.860119473364679;
            CosVerticalAngle[1] = 0.986770965571766;
            CosVerticalAngle[2] = 0.871812912849925;
            CosVerticalAngle[3] = 0.99026806874157;
            CosVerticalAngle[4] = 0.882947592858927;
            CosVerticalAngle[5] = 0.993251859042339;
            CosVerticalAngle[6] = 0.89368485442988;
            CosVerticalAngle[7] = 0.995676196568517;
            CosVerticalAngle[8] = 0.903858661687312;
            CosVerticalAngle[9] = 0.997564050259824;
            CosVerticalAngle[10] = 0.913545457642601;
            CosVerticalAngle[11] = 0.998914402915095;
            CosVerticalAngle[12] = 0.922740022918746;
            CosVerticalAngle[13] = 0.999730593220608;
            CosVerticalAngle[14] = 0.931500901980512;
            CosVerticalAngle[15] = 1;
            CosVerticalAngle[16] = 0.939692620785908;
            CosVerticalAngle[17] = 0.999730593220608;
            CosVerticalAngle[18] = 0.947378020466135;
            CosVerticalAngle[19] = 0.998914402915095;
            CosVerticalAngle[20] = 0.954604963513407;
            CosVerticalAngle[21] = 0.997564050259824;
            CosVerticalAngle[22] = 0.961261695938319;
            CosVerticalAngle[23] = 0.995676196568517;
            CosVerticalAngle[24] = 0.967400487527919;
            CosVerticalAngle[25] = 0.993231602049105;
            CosVerticalAngle[26] = 0.97305828562062;
            CosVerticalAngle[27] = 0.99026806874157;
            CosVerticalAngle[28] = 0.978147600733806;
            CosVerticalAngle[29] = 0.986770965571766;
            CosVerticalAngle[30] = 0.982709876657223;
            CosVerticalAngle[31] = 0.982709876657223;

            //Sin垂直
            SinVerticalAngle[0] = -0.510092630351456;
            SinVerticalAngle[1] = -0.162120515372252;
            SinVerticalAngle[2] = -0.489838999047777;
            SinVerticalAngle[3] = -0.139173100960065;
            SinVerticalAngle[4] = -0.469471562785891;
            SinVerticalAngle[5] = -0.115977344808961;
            SinVerticalAngle[6] = -0.448695198283472;
            SinVerticalAngle[7] = -0.0928919349935815;
            SinVerticalAngle[8] = -0.427831181300314;
            SinVerticalAngle[9] = -0.0697564737441253;
            SinVerticalAngle[10] = -0.4067366430758;
            SinVerticalAngle[11] = -0.0465834267608028;
            SinVerticalAngle[12] = -0.385422949633143;
            SinVerticalAngle[13] = -0.0232107944450872;
            SinVerticalAngle[14] = -0.363739013042995;
            SinVerticalAngle[15] = 0;
            SinVerticalAngle[16] = -0.342020143325669;
            SinVerticalAngle[17] = 0.0232107944450872;
            SinVerticalAngle[18] = -0.320116988517741;
            SinVerticalAngle[19] = 0.0465834267608028;
            SinVerticalAngle[20] = -0.297874744877048;
            SinVerticalAngle[21] = 0.0697564737441253;
            SinVerticalAngle[22] = -0.275637355816999;
            SinVerticalAngle[23] = 0.0928919349935815;
            SinVerticalAngle[24] = -0.253251449612328;
            SinVerticalAngle[25] = 0.116150698194064;
            SinVerticalAngle[26] = -0.230559260896326;
            SinVerticalAngle[27] = 0.139173100960065;
            SinVerticalAngle[28] = -0.207911690817759;
            SinVerticalAngle[29] = 0.162120515372252;
            SinVerticalAngle[30] = -0.185152095101151;
            SinVerticalAngle[31] = 0.185152095101151;
        }
    }
}
