using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using AlgorithmTool;

namespace Others
{
    public class NavigationInfo
    {
        public int LocationX;          //NAV-X座標資訊
        public int LocationY;          //NAV-Y座標資訊
        public int Direction;          //NAV--方向資訊
    }

    public class GlobalVar
    {
        /** \brief是否要開預測紀錄*/
        public static bool isPredictLog = true;

        /** \brief是否要開PID紀錄*/
        public static bool isPIDLog = false;

        /** \brief系統初始化時間*/
        public static int SystemInitTimes = 8;

        /** \brief目前兩輪中心位置*/
        public static NavigationInfo CurrentPosition;

        /** \brief目前驅動馬達的位置*/
        public static NavigationInfo MotorPosition;

        /** \brief目前左前輪的位置*/
        public static NavigationInfo CarTirepositionL;

        /** \brief目前右前輪的位置*/
        public static NavigationInfo CarTirepositionR;

        /** \brief貨叉當前高度*/
        public static double ForkCurrentHeight;

        /** \brief目前左前輪速度*/
        public static double CarTireSpeedLeft;

        /** \brief目前右前輪速度*/
        public static double CarTireSpeedRight;

        /** \brief是否開始PID控制*/
        public static bool isStartPIDSimulation = false;

        /** \brief是否開始CanBus Debug*/
        public static bool isCanBusDebug = true;

        /** \brief馬達實際的轉向角度*/
        public static double RealMotorAngle;

        /** \brief馬達實際的轉向角度*/
        public static double RealMotorPower;

        /** \brief馬達轉向速度'*/
        public static byte RotateSpeed = 0x0A;

        /** \brief馬達行走加速度'*/
        public static byte MotorAcceleration = 150;

        /** \brief堆高機重量sensor'*/
        public static float WeightVoltage = 0;

        /** \brief系統時間計數'*/
        public static int SysTimerCounter = 0;

        /** \brief預測下一次位置資料'*/
        public static NavigationInfo Predictposition;

        /** \brief目前NAV收到資訊的時間'*/
        public static DateTime NavTimeStamp = new DateTime();

        /** \brief紀錄目前PLC是否連線*/
        public static bool isPLCConnect = false;

        /** \brief紀錄貨叉是否在原點校正*/
        public static bool islibratOrigin = false;

        /** \brief行走時的最大Power*/
        public static int MaxPower = 120;

        /** \brief是否按下暫停*/
        public static bool isPauseStart = true;

        /** \brief所有車身座標資訊*/
        public static rtCarData CarData;

        /** \brief這台AGV的ID*/
        public static int This_AGV_ID;

        /** \brief 讀取PLC資料的時間點*/
        public static DateTime Time_Read_PLC_Data;

        /** \brief 讀取PLC資料的時間點*/
        public static Stopwatch Watch_Read_PLC_Data;

        /** \brief 是否開啟Log*/
        public static bool isLog = false;

        public static string aboutUS = "程式版本：7.7.1.105"; //月.日.累計(再出版就持續累加.年)
    }
}
