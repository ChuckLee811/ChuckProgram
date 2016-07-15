#define rtAGV_DEBUG_PREDICT

// #define rtAGV_DEBUG_PRINT

#define rtAGV_DEBUG_OFFSET_MODIFY

#define rtAGV_POWER_SMOOTH

#define rtAGV_BACK_MODE

#define rtAGV_PATH_OFFSET

using System;

namespace AlgorithmTool
{

    /// <summary> PID Coefficient struct </summary> 
    public struct rtPID_Coefficient
    {
		/** \brief Kp   */
        public double eKp;

		/** \brief Ki   */
        public double eKi;

		/** \brief Kd   */
        public double eKd;

        /// <summary> init PID Coefficient struct </summary> 
        public void Init()
        {
            eKp = 0;
            eKi = 0;
            eKd = 0;
        }
    }

    /// <summary> configure of motor control. </summary> 
    public struct rtMotor_Cfg
    {
		/** \brief Define: PID Power Coeffient Kp   */
        public const double PID_POWER_COE_KP = 0.0256;

		/** \brief Define: PID Coefficient for Theta Offset Kp   */
        public const double PID_THETA_OFFSET_COE_KP = 0.34;

		/** \brief Define: PID Coefficient for Car Angle to motor angle Kp   */
        public const double PID_ANGLE_CAR_MOTOR_COE_KP = 0.75;

		/** \brief Define: Radius of smooth mode: 旋轉半徑 (判斷是否到達定點 開始準備轉向動作)   */
        public const int RADIUS_SMOOTH = 1000;

		/** \brief Configure: PID Power Coeffient   */
        public rtPID_Coefficient tPID_PowerCoe;

		/** \brief Configure: PID Coefficient for Theta Offset   */
        public rtPID_Coefficient tPID_ThetaOffsetCoe;

		/** \brief Configure: PID Coefficient for Car Angle to motor angle   */
        public rtPID_Coefficient tPID_MotorAngleCoe;

		/** \brief Configure: Rotation distance of turn in smooth mode   */
        public int lRotationDistance;

        /// <summary> init configure of motor control. </summary> 
		/** \brief    */
        public void Init()
        {
            tPID_PowerCoe.Init();
            tPID_ThetaOffsetCoe.Init();
            tPID_MotorAngleCoe.Init();
            lRotationDistance = 0;
        }

        /// <summary> Load Default configure of motor control. </summary> 
        public void LoadDefault()
        {
            tPID_PowerCoe.Init();
            tPID_ThetaOffsetCoe.Init();
            tPID_MotorAngleCoe.Init();

            tPID_PowerCoe.eKp = PID_POWER_COE_KP;
            tPID_ThetaOffsetCoe.eKp = PID_THETA_OFFSET_COE_KP;
            tPID_MotorAngleCoe.eKp = PID_ANGLE_CAR_MOTOR_COE_KP;
            lRotationDistance = RADIUS_SMOOTH;
        }
    }

    /// <summary> the struct of motor control output data. </summary> 
    public struct rtMotor_Data
    {
		/** \brief InOut Data: path segment index   */
        public int lPathNodeIndex;

		/** \brief InOut Data: navigate offset: 決定要靠路線左側(負) 還是右側(正) 走   */
        public int lNavigateOffset;

		/** \brief Output Data: Finish Flag   */
        public bool bFinishFlag;

		/** \brief Output Data: Turn (Rotation) Radius of turn in smooth mode   */
        public int lTurnRadius;

		/** \brief Output Data: Turn (rotation) center of smooth turn   */
        public rtVector tTurnCenter;

		/** \brief Output Data: 旋轉方向: 1: 中心在左邊>> 向右轉  -1:中心在右邊>> 向左轉  0: 出錯   */
        public int lTurnDirection;

		/** \brief Output Data: motor power   */
        public int lMotorPower;

		/** \brief Output Data: motor torsion   */
        public int lMotorTorsion;

		/** \brief Output Data: motor angle   */
        public int lMotorAngle;

		/** \brief Output Data: 轉彎角度的Offset   */
        public double lTargetAngle;

		/** \brief Output Data: 車身改變的角度 預測模型用   */
        public double eDeltaAngle;

		/** \brief Output Data: 預測實際圓心   */
        public rtVector tPredictRotationCenter;
		
		/** \brief Output Data: 當下是否是反向行走   */
        public bool bBackWard;

		/** \brief 記錄車身是否已轉正 每段路徑都得轉正再開始執行   */
        public bool bPathAngleMatch;

		/** \brief 判斷車子是否已超過該段路徑的終點   */
        public bool bOverDest;

		/** \brief 路徑偏差   */
        public double ePathError;

		/** \brief 車身角度和路徑角度偏差多少   */
        public double Debug_ePathThetaError;

		/** \brief 沒考慮車速的目標車身角度   */
        public double Debug_TargetAngleOffset1;


		/** \brief 有考慮車速的目標車身角度   */
        public double Debug_TargetAngleOffset2;

		/** \brief 兩前輪中心的速度   */
        public double Debug_CenterSpeed;

		/** \brief 目標車身角度與當下車身角度的差距   */
        public double Debug_eDeltaCarAngle;

		/** \brief 距離終點的角度或長度   */
        public double eDistance2Dest;

        /// <summary> stop all action of motor </summary>
        public void StopCar()
        {
            lMotorPower = 0;
            lMotorTorsion = 0;
            lMotorAngle = 0;
        }


        /// <summary> Clear Data of turn like radius and center </summary>
        public void ClearTurnData()
        {
            tTurnCenter.Init();
            lTurnRadius = 0;
        }

        /// <summary> init function of motor control output data </summary>
        public void Init()
        {
            lPathNodeIndex = 0;
            lNavigateOffset = 0;
            bFinishFlag = false;
            lTurnRadius = 0;
            tTurnCenter.Init();
            lTurnDirection = 0;
            lMotorPower = 0;
            lMotorTorsion = 0;
            lMotorAngle = 0;
            lTargetAngle = 0;
            eDeltaAngle = 0;
            tPredictRotationCenter.Init();
            ePathError = 0;
            Debug_ePathThetaError = 0;
            bPathAngleMatch = false;
            bBackWard = false;
        }
    }

    /// <summary> core class of motor control </summary>
    public class rtMotorCtrl
    {
        public enum rtTurnType_Simple { ERROR = 0, CAR_TURN_LEFT = 1, CAR_TURN_RIGHT = -1 };

		/** \brief Define: 角度對齊的閥值   */
        public const double ANGLE_MATCH_TH = 1.0;

		/** \brief Define: 角度對齊時決定馬達力道的角度閥值   */
        public const double ALIGNMENT_SPEED_ANGLE_TH = 15.0;

		/** \brief Define: 系統頻率 8Hz = 0.125s 1次   */
        public const double FREQUENCY = 8;

		/** \brief Define: angle threshold: 判斷是否走過頭用的   */
        public const double ANGLE_TH = 90;

		/** \brief Define: 可以在原地打轉的角度   */
        public static int ANGLE_ROTATION = 85;

		/** \brief Define: 馬達(驅動輪胎)在這角度以內以直行計算   */
        public const int ANGLE_TH_MOTION_PREDICT = 5;

		/** \brief Define: 路徑間夾角的最大值限制   */
        public const int ANGLE_TH_PATH = 80;

		/** \brief Define: 與期望車身角度最大差距限制   */
        public const int DELTA_CAR_ANGLE_LIMIT = 90;

		/** \brief Define: 直行時角度超過這設定要車身校正   */
        public const int ANGLE_TH_NEED_ALIGNMENT = 30;

		/** \brief Define: 路徑偏差距離上限 >> 超過就不再增加角度   */
        public const int PATH_ERROR_LIMT = 2000;

		/** \brief Define: 預設對路徑的偏移量 >>   */
        public const int DEFAULT_PATH_OFFSET = 150;

		/** \brief Define: distance threshold of simple mode: 判斷是否到達定點 開始準備轉向動作或停止   */
        public const double DISTANCE_ERROR_SIMPLE = 60;

		/** \brief Define: 判斷是否做完轉彎的動作   */
        public const double THETA_ERROR_TURN = 5;

		/** \brief Define: 基礎速度(mm/s) : 判斷車身跟路徑所需夾角的基礎速度   */
        public const double BASE_SPEED = 125;

		/** \brief Define: 轉向時馬達的power   */
        public const int TURN_POWER = 50;

		/** \brief Define: max power of motor   */
        public const int MAX_POWER = 255;

		/** \brief Define: min power of motor   */
        public const int MIN_POWER = 30;

		/** \brief Define: max angle value of motor   */
        public const int MAX_ANGLE_OFFSET_MOTOR = 70;

		/** \brief Define: max angle value of path   */
        public const int MAX_ANGLE_OFFSET_PATH = 70;

		/** \brief 判斷想要的角度跟感測器回傳的角度是否差距過大的閥值   */
        public const int DELTA_ANGLE_TH = 40;

		/** \brief Motor Control configure   */
        public rtMotor_Cfg tMotorCfg;

		/** \brief Motor Control Data   */
        public rtMotor_Data tMotorData;

		/** \brief 自轉時記錄車身是否已轉正   */
        bool bAlignmentCarAngleMatch;

#if rtAGV_DEBUG_PREDICT
		/** \brief Output Data: 預測下次的位置資訊   */
        public rtVector tNextPositionTest;

		/** \brief Output Data: 預測counter   */
        public int lCntTest = 0;

		/** \brief Output Data: 預測error   */
        public double ePredictErrorTest = 0;

        /// <summary> test function for coordinate predict </summary>
        /// <param name="a_tCarData">[IN] car data information </param>
        /// <param name="a_CMotorInfo">[IN] motor control class </param>
        public static void Test_Predict(rtCarData a_tCarData, ref rtMotorCtrl a_CMotorInfo)
        {
            if (a_CMotorInfo.lCntTest > 0)
            {
                a_CMotorInfo.ePredictErrorTest = rtVectorOP_2D.GetDistance(a_tCarData.tPosition, a_CMotorInfo.tNextPositionTest);
            }
            else
            {
                a_CMotorInfo.ePredictErrorTest = 0;
            }
            a_CMotorInfo.tNextPositionTest = Coordinate_Predict(a_tCarData.tPosition, a_tCarData, a_CMotorInfo, (1 / FREQUENCY));

            a_CMotorInfo.lCntTest++;
        }
#endif

        /// <summary> constructor function </summary>
        public rtMotorCtrl()
        {
            // Load default configure
            tMotorCfg.LoadDefault();

            // init output data
            tMotorData.Init();

            bAlignmentCarAngleMatch = false;
        }


        /// <summary> distance error calculate during Straight mode </summary>
        /// <param name="a_atPathInfo">[IN] entire path data for navigate </param>
        /// <param name="a_tPosition">[IN] current car position </param>
        /// <param name="a_lPathNodeIndex">[IN] index of current path </param>
        /// <returns> distance error </returns>
        public static double MotorPower_StraightErrorCal(rtPath_Info[] a_atPathInfo, rtVector a_tPosition, int a_lPathNodeIndex)
        {
            double eErrorCurrent = 0;
            rtVector tV_C2D; // current point to destination

#if rtAGV_POWER_SMOOTH
            rtVector tV_C2D_Last; // current point to destination
            int lPathLength = 0;
            int lNodeIndex = 0;
            double ePathTheta = 0;
#endif
            tV_C2D.eX = a_atPathInfo[a_lPathNodeIndex].tDest.eX - a_tPosition.eX;
            tV_C2D.eY = a_atPathInfo[a_lPathNodeIndex].tDest.eY - a_tPosition.eY;
            eErrorCurrent = rtVectorOP_2D.GetLength(tV_C2D);

#if rtAGV_POWER_SMOOTH
            lPathLength = a_atPathInfo.Length;
            for (lNodeIndex = a_lPathNodeIndex+1; lNodeIndex < lPathLength; lNodeIndex++)
            {
                tV_C2D.eX = a_atPathInfo[lNodeIndex].tDest.eX - a_atPathInfo[lNodeIndex].tSrc.eX;
                tV_C2D.eY = a_atPathInfo[lNodeIndex].tDest.eY - a_atPathInfo[lNodeIndex].tSrc.eY;

                tV_C2D_Last.eX = a_atPathInfo[lNodeIndex - 1].tDest.eX - a_atPathInfo[lNodeIndex - 1].tSrc.eX;
                tV_C2D_Last.eY = a_atPathInfo[lNodeIndex - 1].tDest.eY - a_atPathInfo[lNodeIndex - 1].tSrc.eY;
                ePathTheta = rtVectorOP_2D.GetTheta(tV_C2D_Last, tV_C2D);

                if (ePathTheta< THETA_ERROR_TURN)
                {
                    eErrorCurrent += rtVectorOP_2D.GetLength(tV_C2D);
                }
                else
                {
                    break;
                }
            }            
#endif
            return eErrorCurrent;
        }

        /// <summary> calculate distance to the destination </summary>
        /// <param name="a_atPathInfo">[IN] entire path data for navigate </param>
        /// <param name="a_tPosition">[IN] current car position </param>
        /// <param name="a_lPathNodeIndex">[IN] index of current path </param>
        /// <returns> distance to the destination </returns>
        public static double Distance2PathDestCal(rtPath_Info[] a_atPathInfo, rtVector a_tPosition, int a_lPathNodeIndex)
        {
            double eErrorCurrent = 0, eTheta = 0;
            rtVector tV_C2D; // current point to destination
            rtVector tVectorD2S = new rtVector();
            rtVector tVetorProject = new rtVector();

#if rtAGV_POWER_SMOOTH
            rtVector tV_C2D_Last; // current point to destination
            int lPathLength = 0;
            int lNodeIndex = 0;
            double ePathTheta = 0;
#endif
            tV_C2D = rtVectorOP_2D.GetVector(a_tPosition, a_atPathInfo[a_lPathNodeIndex].tDest);
            tVectorD2S = rtVectorOP_2D.GetVector(a_atPathInfo[a_lPathNodeIndex].tDest, a_atPathInfo[a_lPathNodeIndex].tSrc);
            eTheta = 180 - rtVectorOP_2D.GetTheta(tV_C2D, tVectorD2S);
            eErrorCurrent = rtVectorOP_2D.GetLength(tV_C2D);

            if (eTheta > 0.1)
            {   // 角度夠大 >> 投影至路徑向量上看長度
                tVetorProject = rtVectorOP_2D.VectorProject(tV_C2D, tVectorD2S);
                eErrorCurrent = rtVectorOP_2D.GetLength(tVetorProject);
            }

#if rtAGV_POWER_SMOOTH
            lPathLength = a_atPathInfo.Length;
            for (lNodeIndex = a_lPathNodeIndex + 1; lNodeIndex < lPathLength; lNodeIndex++)
            {
                tV_C2D = rtVectorOP_2D.GetVector(a_atPathInfo[lNodeIndex].tSrc, a_atPathInfo[lNodeIndex].tDest);
                tV_C2D_Last = rtVectorOP_2D.GetVector(a_atPathInfo[lNodeIndex - 1].tSrc, a_atPathInfo[lNodeIndex - 1].tDest);
                ePathTheta = rtVectorOP_2D.GetTheta(tV_C2D_Last, tV_C2D);

                if (ePathTheta < THETA_ERROR_TURN)
                {
                    eErrorCurrent += rtVectorOP_2D.GetLength(tV_C2D);
                }
                else
                {
                    break;
                }
            }
#endif
            return eErrorCurrent;
        }

        /// <summary> 判斷從線開始到終點是否屬於一直線 (中途是否有過大的轉彎) </summary>
        /// <param name="a_atPathInfo">[IN] entire path data for navigate </param>
        /// <param name="a_lPathNodeIndex">[IN] index of current path </param>
        /// <returns> true: 沒過大的轉彎 false: 中間會有大轉彎 </returns>
        public static bool Link2DestCheck(rtPath_Info[] a_atPathInfo, int a_lPathNodeIndex)
        {
            rtVector tV_C2D;        // current point to destination
            rtVector tV_C2D_Last;   // current point to destination
            int lPathLength = 0;
            int lNodeIndex = 0;
            double ePathTheta = 0;

            lPathLength = a_atPathInfo.Length;
            for (lNodeIndex = a_lPathNodeIndex + 1; lNodeIndex < lPathLength; lNodeIndex++)
            {
                tV_C2D.eX = a_atPathInfo[lNodeIndex].tDest.eX - a_atPathInfo[lNodeIndex].tSrc.eX;
                tV_C2D.eY = a_atPathInfo[lNodeIndex].tDest.eY - a_atPathInfo[lNodeIndex].tSrc.eY;
                tV_C2D_Last.eX = a_atPathInfo[lNodeIndex - 1].tDest.eX - a_atPathInfo[lNodeIndex - 1].tSrc.eX;
                tV_C2D_Last.eY = a_atPathInfo[lNodeIndex - 1].tDest.eY - a_atPathInfo[lNodeIndex - 1].tSrc.eY;
                ePathTheta = rtVectorOP_2D.GetTheta(tV_C2D_Last, tV_C2D);

                if (ePathTheta >= THETA_ERROR_TURN)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary> check if car need to BackWard </summary>
        /// <param name="a_atPathInfo">[IN] entire path data for navigate </param>
        /// <param name="a_eCarAngle">[IN] direction of car </param>
        /// <returns> true: need false: do not need </returns>
        public static bool BackWardVerify(rtPath_Info a_tPathInfo, double a_eCarAngle)
        {
            double eDeltaAngle = 0;
            rtVector tV_S2D; // source point to destination

            // 算出當前路徑向量
            tV_S2D = rtVectorOP_2D.GetVector(a_tPathInfo.tSrc, a_tPathInfo.tDest);

            // 算出車身向量與路徑向量的夾角
            eDeltaAngle = rtAngleDiff.GetAngleDiff(tV_S2D, a_eCarAngle);

            // 判斷是否該倒退走比較合適
            if (Math.Abs(eDeltaAngle) >= ANGLE_TH)
            { // 角度過大 適合倒退走
                return true;
            }
            return false;
        }

        /// <summary> check if car over destination  </summary>
        /// <param name="a_atPathInfo">[IN] entire path data for navigate </param>
        /// <param name="a_tPosition">[IN] Position of car </param>
        /// <param name="a_lPathNodeIndex">[IN] index of ccurrent path </param>
        /// <returns> true: yes false: no </returns>
        public static bool OverDestination(rtPath_Info[] a_atPathInfo, rtVector a_tPosition, int a_lPathNodeIndex)
        {
            double eTheta = 0;
            rtVector tV_C2D; // current point to destination
            rtVector tV_S2D; // source point to destination

            tV_S2D.eX = a_atPathInfo[a_lPathNodeIndex].tDest.eX - a_atPathInfo[a_lPathNodeIndex].tSrc.eX;
            tV_S2D.eY = a_atPathInfo[a_lPathNodeIndex].tDest.eY - a_atPathInfo[a_lPathNodeIndex].tSrc.eY;
            tV_C2D.eX = a_atPathInfo[a_lPathNodeIndex].tDest.eX - a_tPosition.eX;
            tV_C2D.eY = a_atPathInfo[a_lPathNodeIndex].tDest.eY - a_tPosition.eY;

            eTheta = rtVectorOP_2D.GetTheta(tV_S2D, tV_C2D);

            // 判斷是否已超終點
            if (eTheta >= ANGLE_TH)
            { // 超過終點 >>　必須反向行走
                return true;
            }
            return false;
        }

        /// <summary> motor power calculate during Turn mode </summary>
        /// <param name="a_tPathInfo">[IN] path data for navigate </param>
        /// <param name="a_eCarAngle">[IN] direction of car </param>
        /// <returns> power value </returns>
        public static double MotorPower_TurnErrorCal(rtPath_Info a_tPathInfo, double a_eCarAngle)
        {
            double eTheta = 0;
            rtVector tV_Car;        // car current direction
            rtVector tV_NextS2D;    // next src point to destination

            tV_Car.eX = Math.Cos(a_eCarAngle * Math.PI / 180);
            tV_Car.eY = Math.Sin(a_eCarAngle * Math.PI / 180);

            tV_NextS2D.eX = a_tPathInfo.tDest.eX - a_tPathInfo.tSrc.eX;
            tV_NextS2D.eY = a_tPathInfo.tDest.eY - a_tPathInfo.tSrc.eY;

            eTheta = rtVectorOP_2D.GetTheta(tV_Car, tV_NextS2D);

            return eTheta;
        }

        /// <summary> Distance Modify For Path Offset </summary>
        /// <param name="a_tPathInfo">[IN] path data for navigate </param>
        /// <param name="a_tPosition">[IN] Position of car </param>
        /// <param name="a_eDistanceOri">[IN] Original Distance </param>
        /// <returns> modified diatance </returns>
        public double DistanceModifyForPathOffset(rtPath_Info a_tPathInfo, rtVector a_tPosition, double a_eDistanceOri)
        {
            double eDistance = 0, eTheta = 0, eDistanceD2C = 0, eDistanceProject = 0;

            rtVector tVectorD2S = new rtVector();
            rtVector tVetorD2C = new rtVector();
            rtVector tVetorProject = new rtVector();

            tVetorD2C = rtVectorOP_2D.GetVector(a_tPathInfo.tDest, a_tPosition);
            tVectorD2S = rtVectorOP_2D.GetVector(a_tPathInfo.tDest, a_tPathInfo.tSrc);

            eTheta = rtVectorOP_2D.GetTheta(tVetorD2C, tVectorD2S);

            if(Math.Abs(eTheta) < 0.001)
            {   // 夾角過小代表在路徑上 >> 不處理
                eDistance = a_eDistanceOri;
            }
            else
            {   // 投影至路徑向量上看長度
                tVetorProject = rtVectorOP_2D.VectorProject(tVetorD2C, tVectorD2S);

                eDistanceProject = rtVectorOP_2D.GetLength(tVetorProject);
                eDistanceD2C = rtVectorOP_2D.GetLength(tVetorD2C);
                eDistance = a_eDistanceOri - eDistanceD2C + eDistanceProject;
            }
            return eDistance;
        }


        /// <summary> control during car direction aligment </summary>
        /// <param name="a_atPathInfo">[IN] path data for navigate </param>
        /// <param name="a_tCarData">[IN] data of car </param>
        public void Motor_CtrlNavigateAligment(rtPath_Info[] a_atPathInfo, rtCarData a_tCarData)
        {
            bool bAlignment = false;
            double eErrorCurrent = 0, eTargetAngle = 0;
            int lPathNodeIndex = 0;
            rtVector tV_S2D_Next = new rtVector();
            rtVector tV_Aligment = new rtVector();
            rtPath_Info.rtTurnType tTurnTypeNext;

            lPathNodeIndex = tMotorData.lPathNodeIndex;
            tTurnTypeNext = (rtPath_Info.rtTurnType)a_atPathInfo[lPathNodeIndex + 1].ucTurnType;
            tV_S2D_Next = rtVectorOP_2D.GetVector(a_atPathInfo[lPathNodeIndex+1].tSrc, a_atPathInfo[lPathNodeIndex+1].tDest);
            tMotorData.bBackWard = BackWardVerify(a_atPathInfo[lPathNodeIndex+1], a_tCarData.eAngle);

            if(tTurnTypeNext == rtPath_Info.rtTurnType.ARRIVE)
            {   // 下段是要取放貨 >> 一定要正走
                tV_Aligment = tV_S2D_Next;
            }
            else if(tTurnTypeNext == rtPath_Info.rtTurnType.PARK)
            {   // 下段是要停車 >> 一定要反走走
                tV_Aligment = rtVectorOP_2D.VectorMultiple(tV_S2D_Next, -1);
            }
            else
            {   // 下段非停車或取放貨 >> 依照方便度自行決定正反走
                tV_Aligment = (tMotorData.bBackWard) ? rtVectorOP_2D.VectorMultiple(tV_S2D_Next, -1) : tV_S2D_Next;
            }
            
            eTargetAngle = rtVectorOP_2D.Vector2Angle(tV_Aligment);

            // 用車身方向與目標方向的夾角當誤差
            eErrorCurrent = Math.Abs(rtAngleDiff.GetAngleDiff(tV_Aligment, a_tCarData.eAngle));
          
            bAlignment = CarAngleAlignment(eTargetAngle, a_tCarData);

            if (bAlignment)
            {
                a_atPathInfo[lPathNodeIndex].ucStatus = (byte)rtPath_Info.rtStatus.DONE;
                tMotorData.lPathNodeIndex++;
            }
            tMotorData.ePathError = 0;
            tMotorData.eDistance2Dest = eErrorCurrent;
        }

        /// <summary> power control during Smooth Turn mode </summary>
        /// <param name="a_atPathInfo">[IN] path data for navigate </param>
        /// <param name="a_tCarData">[IN] data of car </param>
        public void MotorPower_CtrlNavigateSmoothTurn(rtPath_Info[] a_atPathInfo, rtCarData a_tCarData)
        {   // 轉彎狀態
            bool bOutFlag = false;
            double eErrorCurrent = 0;

            tMotorData.bBackWard = BackWardVerify(a_atPathInfo[tMotorData.lPathNodeIndex+1], a_tCarData.eAngle);

            // 用車身與下一段路徑的夾角當power誤差
            eErrorCurrent = MotorPower_TurnErrorCal(a_atPathInfo[tMotorData.lPathNodeIndex + 1], a_tCarData.eAngle);
            tMotorData.lMotorPower = TURN_POWER;

            if (tMotorData.bBackWard)
            {   // 反向走 >> 所以車身向量會差180度 & power會取負號
                eErrorCurrent = 180 - eErrorCurrent;
                tMotorData.lMotorPower = -tMotorData.lMotorPower;
            }

            // 判斷是否已經走完轉彎的扇形區
            if (tMotorData.bBackWard)
            {
                bOutFlag = FinishSmoothTurnCheck(
                    tMotorData.lPathNodeIndex, tMotorCfg.lRotationDistance,
                    a_atPathInfo, a_tCarData.tMotorPosition, tMotorData.tTurnCenter);
            }
            else
            {
                bOutFlag = FinishSmoothTurnCheck(
                    tMotorData.lPathNodeIndex, tMotorCfg.lRotationDistance,
                    a_atPathInfo, a_tCarData.tPosition, tMotorData.tTurnCenter);
            }
            if (eErrorCurrent < THETA_ERROR_TURN || bOutFlag == true)
            {
                a_atPathInfo[tMotorData.lPathNodeIndex].ucStatus = (byte)rtPath_Info.rtStatus.DONE;
                tMotorData.lPathNodeIndex++;
            }
        }

        /// <summary> power control during Straight mode </summary>
        /// <param name="a_atPathInfo">[IN] path data for navigate </param>
        /// <param name="a_tCarData">[IN] data of car </param>
        public void MotorPower_CtrlNavigateStraight(rtPath_Info[] a_atPathInfo, rtCarData a_tCarData)
        {   // 直走狀態
            double eErrorCurrent = 0, eDistanceMotor2Dest = 0, eDistanceTurn = 0;

            tMotorData.bFinishFlag = false;
            tMotorData.bOverDest = false;
            tMotorData.bBackWard = BackWardVerify(a_atPathInfo[tMotorData.lPathNodeIndex], a_tCarData.eAngle );
            tMotorData.bOverDest = OverDestination(a_atPathInfo, a_tCarData.tPosition, tMotorData.lPathNodeIndex);
            if (tMotorData.bOverDest == true)
            {   // 判斷超過終點
                if (a_atPathInfo[tMotorData.lPathNodeIndex].ucTurnType == (byte)rtPath_Info.rtTurnType.ARRIVE)
                {   // 到達最終目的地 >> 停車
                    tMotorData.StopCar();
                    a_atPathInfo[tMotorData.lPathNodeIndex].ucStatus = (byte)rtPath_Info.rtStatus.DONE;
                    tMotorData.bFinishFlag = true;
                    Console.WriteLine("到達最終目的地 >> 停車");
                }
                else if (a_atPathInfo[tMotorData.lPathNodeIndex].ucTurnType == (byte)rtPath_Info.rtTurnType.PARK)
                {   // 到達最終目的地 >> 停車
                    tMotorData.StopCar();
                    a_atPathInfo[tMotorData.lPathNodeIndex].ucStatus = (byte)rtPath_Info.rtStatus.DONE;
                    tMotorData.bFinishFlag = true;
                    Console.WriteLine("到達最終目的地 >> 停進車位");
                }
                else
                { // 趕快進入下一段 (要不要先轉正再說)
                    a_atPathInfo[tMotorData.lPathNodeIndex].ucStatus = (byte)rtPath_Info.rtStatus.DONE;
                    tMotorData.lPathNodeIndex++;

                    // 將旋轉半徑、中心等資料清空
                    tMotorData.ClearTurnData();
                }
            }
            else
            {   // 尚未超過終點
                // 算出跟終點距離
                eErrorCurrent = Distance2PathDestCal(a_atPathInfo, a_tCarData.tPosition, tMotorData.lPathNodeIndex);

                eDistanceMotor2Dest = rtVectorOP_2D.GetDistance(a_atPathInfo[tMotorData.lPathNodeIndex].tDest, a_tCarData.tMotorPosition);

                // Motor power = function(Error)
                tMotorData.lMotorPower = (int)(tMotorCfg.tPID_PowerCoe.eKp * eErrorCurrent) + MIN_POWER;

                if (tMotorData.bBackWard)
                {   // 反向走
                    tMotorData.lMotorPower = -tMotorData.lMotorPower;
                }

                eDistanceTurn = (tMotorData.bBackWard) ? eDistanceMotor2Dest : eErrorCurrent;
                switch (a_atPathInfo[tMotorData.lPathNodeIndex].ucTurnType)
                {
                    case (byte)rtPath_Info.rtTurnType.SIMPLE:
                        if (eErrorCurrent < DISTANCE_ERROR_SIMPLE)
                        {
                            a_atPathInfo[tMotorData.lPathNodeIndex].ucStatus = (byte)rtPath_Info.rtStatus.TURN;
                        }
                        break;
                    case (byte)rtPath_Info.rtTurnType.SMOOTH:
                        if (eDistanceTurn < rtMotor_Cfg.RADIUS_SMOOTH)
                        {
                            // 算出旋轉半徑 & 中心座標
                            RotationCenterAndRadiusCal(a_atPathInfo, tMotorCfg.lRotationDistance, ref tMotorData);

                            a_atPathInfo[tMotorData.lPathNodeIndex].ucStatus = (byte)rtPath_Info.rtStatus.TURN;
                        }
                        break;
                    case (byte)rtPath_Info.rtTurnType.ARRIVE:
                        if (eErrorCurrent < DISTANCE_ERROR_SIMPLE)
                        { // 到達最終目的地
                            tMotorData.StopCar();
                            a_atPathInfo[tMotorData.lPathNodeIndex].ucStatus = (byte)rtPath_Info.rtStatus.DONE;
                            tMotorData.bFinishFlag = true;
                            Console.WriteLine("ARRIVE");
                        }
                        break;
                    case (byte)rtPath_Info.rtTurnType.PARK:
                        if (eErrorCurrent < DISTANCE_ERROR_SIMPLE)
                        { // 到達最終目的地
                            tMotorData.StopCar();
                            a_atPathInfo[tMotorData.lPathNodeIndex].ucStatus = (byte)rtPath_Info.rtStatus.DONE;
                            tMotorData.bFinishFlag = true;
                            Console.WriteLine("ARRIVE");
                        }
                        break;
                    default:
                        // show error msg
                        break;
                }
            }
            tMotorData.eDistance2Dest = eErrorCurrent;
        }


        /// <summary> 誤差為點到路徑的距離: 點可順時鐘轉至路徑為正 否則為負 </summary>
        /// <param name="a_tPathInfo">[IN] path data for navigate </param>
        /// <param name="a_tPosition">[IN] position of car </param>
        /// <returns> distance error </returns>
        public static double PathErrorCal_Straight(rtPath_Info a_tPathInfo, rtVector a_tPosition)
        {
            double eErrorCurrent = 0, eCross = 0;
            rtVector tVS2D, tVlaw, tVproject, tVS2C;

            tVS2C = rtVectorOP_2D.GetVector(a_tPathInfo.tSrc, a_tPosition);
            tVS2D = rtVectorOP_2D.GetVector(a_tPathInfo.tSrc, a_tPathInfo.tDest);

            // 取右側的法向量
            tVlaw.eX = tVS2D.eY;
            tVlaw.eY = -tVS2D.eX;

            // 將向量投影到法向量上
            tVproject = rtVectorOP_2D.VectorProject(tVS2C, tVlaw);

            // 向量長度即為點到路徑的距離
            eErrorCurrent = rtVectorOP_2D.GetLength(tVproject);

            eCross = rtVectorOP_2D.Cross(tVS2C, tVS2D);
            if (eCross < 0)
            {   // 當下座標轉到路徑向量為逆時針 要取負值
                eErrorCurrent = -eErrorCurrent;
            }

            return eErrorCurrent;
        }


        /// <summary> check the direction car should turn </summary>
        /// <param name="a_atPathInfo">[IN] path data for navigate </param>
        /// <param name="a_lPathNodeIndex">[IN] index of current path </param>
        /// <returns> CAR_TURN_RIGHT: need turn right CAR_TURN_LEFT: need turn left  ERROR: error </returns>
        public static int TurnDirectionCal(rtPath_Info[] a_atPathInfo, int a_lPathNodeIndex)
        {
            int lTurnDirection = 0;
            double eAngle = 0, eCross = 0;
            rtVector tVd2sCurrent;
            rtVector tVs2dNext;

            tVd2sCurrent = rtVectorOP_2D.GetVector(a_atPathInfo[a_lPathNodeIndex].tDest, a_atPathInfo[a_lPathNodeIndex].tSrc);
            tVs2dNext = rtVectorOP_2D.GetVector(a_atPathInfo[a_lPathNodeIndex+1].tSrc, a_atPathInfo[a_lPathNodeIndex+1].tDest);
            eAngle = rtVectorOP_2D.GetTheta(tVd2sCurrent, tVs2dNext);
            eCross = rtVectorOP_2D.Cross(tVd2sCurrent, tVs2dNext);

            if (eAngle > ANGLE_TH_PATH)
            {
                if (eCross > 0)
                {   // 點在右邊 >> 車子需要向右轉(不管正走或反走)
                    lTurnDirection = (int)rtTurnType_Simple.CAR_TURN_RIGHT;
                }
                else
                {   // 點在左邊 >> 車子需要向左轉(不管正走或反走)
                    lTurnDirection = (int)rtTurnType_Simple.CAR_TURN_LEFT;
                }
            }
            else
            {   // 不可能轉這麼大
                lTurnDirection = (int)rtTurnType_Simple.ERROR;
            }

            return lTurnDirection;
        }

        /// <summary> Calculate Rotation Center And Radius for turn </summary>
        /// <param name="a_atPathInfo">[IN] path data for navigate </param>
        /// <param name="a_lRotationDistance">[IN] Rotation Distance </param>
        /// <param name="a_tMotorData">[INOUT] motor output data </param>
        public static void RotationCenterAndRadiusCal(rtPath_Info[] a_atPathInfo, int a_lRotationDistance, ref rtMotor_Data a_tMotorData)
        {
            int lPathIndex = 0;
            rtVector tVd2sCurrent, tVs2dNext, tVd2sCurrentLaw, tVs2dNextLaw, tTurnStart, tTurnEnd;

            lPathIndex = a_tMotorData.lPathNodeIndex;

            // set current vector
            tVd2sCurrent = rtVectorOP_2D.GetVector(a_atPathInfo[lPathIndex].tDest, a_atPathInfo[lPathIndex].tSrc);

            // 取右側法向量
            tVd2sCurrentLaw.eX = tVd2sCurrent.eY;
            tVd2sCurrentLaw.eY = -tVd2sCurrent.eX;

            // 取轉彎起始點
            tTurnStart = rtVectorOP_2D.ExtendPointAlongVector(a_atPathInfo[lPathIndex].tDest, tVd2sCurrent, a_lRotationDistance);

            // set next vector
            tVs2dNext = rtVectorOP_2D.GetVector(a_atPathInfo[lPathIndex + 1].tSrc, a_atPathInfo[lPathIndex + 1].tDest);

            // 取右側法向量
            tVs2dNextLaw.eX = tVs2dNext.eY;
            tVs2dNextLaw.eY = -tVs2dNext.eX;

            // 取轉彎結束點
            tTurnEnd = rtVectorOP_2D.ExtendPointAlongVector(a_atPathInfo[lPathIndex + 1].tSrc, tVs2dNext, a_lRotationDistance);

            // 取兩條線交點當旋轉中心座標
            a_tMotorData.tTurnCenter = rtVectorOP_2D.FindMeetPoint(tTurnStart, tVd2sCurrentLaw, tTurnEnd, tVs2dNextLaw);

            // 計算旋轉半徑 (轉彎路徑用)
            a_tMotorData.lTurnRadius = (int)Math.Round(rtVectorOP_2D.GetDistance(tTurnStart, a_tMotorData.tTurnCenter));
        }

        /// <summary> Check if car Finish Smooth Turn </summary>
        /// <param name="a_lPathIndex">[IN] index of current path </param>
        /// <param name="a_lRotationDistance">[IN] Rotation Distance </param>
        /// <param name="a_atPathInfo">[IN] path data for navigate </param>
        /// <param name="a_tCarPostition">[IN] car position </param>
        /// <param name="a_tRotateCenter">[IN] Rotate Center </param>
        public static bool FinishSmoothTurnCheck(
            int a_lPathIndex, int a_lRotationDistance,
            rtPath_Info[] a_atPathInfo, rtVector a_tCarPostition, rtVector a_tRotateCenter)
        {
            bool bResult = false;
            double eThetaBoundaty = 0, eThetaCurrent = 0;
            rtVector tTurnStart, tTurnEnd, tVd2sCurrent, tVs2dNext;
            rtVector tCenter2SrcTurn, tCenter2DestTurn, tCenter2Current;

            // set current vector
            tVd2sCurrent = rtVectorOP_2D.GetVector(a_atPathInfo[a_lPathIndex].tDest, a_atPathInfo[a_lPathIndex].tSrc);

            // 取轉彎起始點
            tTurnStart = rtVectorOP_2D.ExtendPointAlongVector(a_atPathInfo[a_lPathIndex].tDest, tVd2sCurrent, a_lRotationDistance);

            // set next vector
            tVs2dNext = rtVectorOP_2D.GetVector(a_atPathInfo[a_lPathIndex + 1].tSrc, a_atPathInfo[a_lPathIndex + 1].tDest);

            // 取轉彎結束點
            tTurnEnd = rtVectorOP_2D.ExtendPointAlongVector(a_atPathInfo[a_lPathIndex + 1].tSrc, tVs2dNext, a_lRotationDistance);

            // 以下計算是否超出扇形區域
            tCenter2SrcTurn = rtVectorOP_2D.GetVector(a_tRotateCenter, tTurnStart);
            tCenter2DestTurn = rtVectorOP_2D.GetVector(a_tRotateCenter, tTurnEnd);
            tCenter2Current = rtVectorOP_2D.GetVector(a_tRotateCenter, a_tCarPostition);
            eThetaBoundaty = rtVectorOP_2D.GetTheta(tCenter2DestTurn, tCenter2SrcTurn);
            eThetaCurrent = rtVectorOP_2D.GetTheta(tCenter2Current, tCenter2SrcTurn);

            bResult = (eThetaCurrent > eThetaBoundaty) ? true : false;
            return bResult;
        }


        /// <summary> calculate error of turn  </summary>
        /// <param name="a_tPathInfo">[IN] path data for navigate </param>
        /// <param name="a_tPosition">[IN] car position </param>
        /// <param name="a_tTurnCenter">[IN] turn Center </param>
        /// <param name="a_lTurnRadius">[IN] turn Radius </param>
        /// <param name="a_lTurnDirection">[IN] turn Direction </param>
        /// <returns> error of turn </returns>
        public static double MotorAngle_TurnErrorCal(
            rtPath_Info a_tPathInfo, rtVector a_tPosition,
            rtVector a_tTurnCenter, int a_lTurnRadius, int a_lTurnDirection)
        {
            double eErrorCurrent = 0, eDistance = 0;

            if(a_tPathInfo.ucTurnType == (byte)rtPath_Info.rtTurnType.SMOOTH)
            {
                eDistance = rtVectorOP_2D.GetDistance(a_tPosition, a_tTurnCenter);
                eErrorCurrent = eDistance - a_lTurnRadius; // 可能會有錯誤 如果在內側非扇型區域 >> TBD
            }
            else
            {
                return 0;
            }

            switch (a_lTurnDirection)
            {
                case (int)rtTurnType_Simple.CAR_TURN_RIGHT:
                    // inverse
                    eErrorCurrent = -eErrorCurrent;
                    break;
                default:
                    // Do nothing
                    break;
            }

            return eErrorCurrent;
        }

        /// <summary> calculate path error of turn  </summary>
        /// <param name="a_tPathInfo">[IN] path data for navigate </param>
        /// <param name="a_tPosition">[IN] car position </param>
        /// <param name="a_tTurnCenter">[IN] turn Center </param>
        /// <param name="a_lTurnRadius">[IN] turn Radius </param>
        /// <returns> path error of turn </returns>
        public static double PathErrorCal_Turn(
            rtPath_Info a_tPathInfo, rtVector a_tPosition,
            rtVector a_tTurnCenter, int a_lTurnRadius)
        {
            double eErrorCurrent = 0, eDistance = 0, eCross = 0;
            rtVector tVs2d = new rtVector();
            rtVector tVs2center = new rtVector();

            tVs2d = rtVectorOP_2D.GetVector(a_tPathInfo.tSrc, a_tPathInfo.tDest);
            tVs2center = rtVectorOP_2D.GetVector(a_tPathInfo.tSrc, a_tTurnCenter);
            eCross = rtVectorOP_2D.Cross(tVs2center, tVs2d);
            eDistance = rtVectorOP_2D.GetDistance(a_tPosition, a_tTurnCenter);

            if (a_tPathInfo.ucTurnType == (byte)rtPath_Info.rtTurnType.SMOOTH)
            {
                if(eCross < 0)
                {   // 向左轉 >> 正的部分圓弧路徑外側
                    eErrorCurrent = eDistance - a_lTurnRadius;
                }
                else
                {   // 向右轉 >> 正的部分圓弧路徑內側
                    eErrorCurrent = a_lTurnRadius - eDistance;
                }
            }
            else
            {
                return 0;
            }
            return eErrorCurrent;
        }

        /// <summary> calculate Car Center Speed  </summary>
        /// <param name="a_tCarData">[IN] data for car </param>
        /// <returns> Car Center Speed </returns>
        public static double CarCenterSpeedCal(rtCarData a_tCarData)
        {
            double eSpeed = 0, eTheta = 0, eT = 0;
            double eLength_C2M = 0; // 兩輪中心到後馬達的距離 
            double eLength_C2O = 0; // 兩輪中心到旋轉中心的距離 = 旋轉半徑
            double eLength_R2O = 0; // 右輪中心到旋轉中心的距離 
            double eLength_L2O = 0; // 右輪中心到旋轉中心的距離
            rtVector tV_Car, tVlaw, tRotateCenter;

            eTheta = Math.Abs(a_tCarData.eWheelAngle);
            eLength_C2M = rtVectorOP_2D.GetDistance(a_tCarData.tPosition, a_tCarData.tMotorPosition);
            eLength_C2O = Math.Tan((90 - eTheta) * Math.PI / 180) * eLength_C2M;

            tV_Car.eX = Math.Cos(a_tCarData.eAngle * Math.PI / 180);
            tV_Car.eY = Math.Sin(a_tCarData.eAngle * Math.PI / 180);

            // 取右側的法向量
            tVlaw.eX = tV_Car.eY;
            tVlaw.eY = -tV_Car.eX;

            eT = Math.Sqrt(eLength_C2O * eLength_C2O / (tVlaw.eX * tVlaw.eX + tVlaw.eY * tVlaw.eY));
            if (a_tCarData.eWheelAngle >= 0)
            {
                eT = -eT;
            }

            tRotateCenter.eX = a_tCarData.tPosition.eX + tVlaw.eX * eT;
            tRotateCenter.eY = a_tCarData.tPosition.eY + tVlaw.eY * eT;

            eLength_R2O = rtVectorOP_2D.GetDistance(a_tCarData.tCarTirepositionR, tRotateCenter);
            eLength_L2O = rtVectorOP_2D.GetDistance(a_tCarData.tCarTirepositionL, tRotateCenter);

            if (eTheta > ANGLE_TH_MOTION_PREDICT)
            {
                if (a_tCarData.eWheelAngle < 0)
                { // 車子往右轉 輪子角度為負
                    eSpeed = Math.Abs(a_tCarData.eCarTireSpeedLeft) * eLength_C2O / eLength_L2O;
                }
                else
                { // 車子往左轉 輪子角度為正
                    eSpeed = Math.Abs(a_tCarData.eCarTireSpeedRight) * eLength_C2O / eLength_R2O;
                }
            }
            else
            {   // 當作直行
                eSpeed = (a_tCarData.eCarTireSpeedLeft + a_tCarData.eCarTireSpeedRight) / 2;
            }

            return eSpeed;
        }

        /// <summary> Motion Predict of coordinate  </summary>
        /// <param name="a_tCarData">[IN] data for car </param>
        /// <param name="a_CMotorInfo">[IN] motor control class </param>
        /// <param name="a_eDeltaTime">[IN] the time interval </param>
        /// <param name="a_tCarCoordinate">[OUT] predicted coordinate </param>
        /// <param name="a_eCarAngle">[IN] car direction </param>
        public static void Motion_Predict(rtCarData a_tCarData, rtMotorCtrl a_CMotorInfo, double a_eDeltaTime, out rtVector a_tCarCoordinate, out double a_eCarAngle)
        {
            a_eCarAngle = 0;
            a_tCarCoordinate = new rtVector();
            a_tCarCoordinate = Coordinate_Predict(a_tCarData.tPosition, a_tCarData, a_CMotorInfo, a_eDeltaTime);
            a_eCarAngle = a_tCarData.eAngle + a_CMotorInfo.tMotorData.eDeltaAngle;
        }

        /// <summary> Motion Predict by coordinate rotation on 2D plan  </summary>
        /// <param name="a_CurrentPos">[IN] data for car </param>
        /// <param name="a_tCarData">[IN] car data </param>
        /// <param name="a_CMotorInfo">[IN] motor control class </param>
        /// <param name="a_eDeltaTime">[IN] the time interval </param>
        /// <returns> predicted coordinate </returns>
        public static rtVector Coordinate_Predict(rtVector a_CurrentPos, rtCarData a_tCarData, rtMotorCtrl a_CMotorInfo, double a_eDeltaTime)
        {
            double eDistance = 0, eAngle = 0, eTheta = 0, eSpeed = 0, eT = 0, ePhi = 0, ePhiRad = 0;
            double eLength_C2M = 0; // 兩輪中心到後馬達的距離 
            double eLength_C2O = 0; // 兩輪中心到旋轉中心的距離 = 旋轉半徑
            double eLength_R2O = 0; // 右輪中心到旋轉中心的距離 
            double eLength_L2O = 0; // 右輪中心到旋轉中心的距離
            rtVector tNextPosition = new rtVector();
            rtVector tV_Car, tVlaw, tRotateCenter;

            eAngle = a_tCarData.eAngle;
            tV_Car.eX = Math.Cos(eAngle * Math.PI / 180);
            tV_Car.eY = Math.Sin(eAngle * Math.PI / 180);

            // 取右側的法向量
            tVlaw.eX = tV_Car.eY;
            tVlaw.eY = -tV_Car.eX;

            eTheta = Math.Abs(a_tCarData.eWheelAngle);

            eLength_C2M = rtVectorOP_2D.GetDistance(a_tCarData.tPosition, a_tCarData.tMotorPosition);
            eLength_C2O = Math.Tan((90 - eTheta) * Math.PI / 180) * eLength_C2M;

            if (eTheta > ANGLE_TH_MOTION_PREDICT)
            { // 用車模型預測 (對圓心旋轉)
                eT = Math.Sqrt(eLength_C2O * eLength_C2O / (tVlaw.eX * tVlaw.eX + tVlaw.eY * tVlaw.eY));
                if (a_tCarData.eWheelAngle >= 0)
                {
                    eT = -eT;
                }

                tRotateCenter.eX = a_tCarData.tPosition.eX + tVlaw.eX * eT;
                tRotateCenter.eY = a_tCarData.tPosition.eY + tVlaw.eY * eT;

                a_CMotorInfo.tMotorData.tPredictRotationCenter.eX = tRotateCenter.eX;
                a_CMotorInfo.tMotorData.tPredictRotationCenter.eY = tRotateCenter.eY;

                eLength_R2O = rtVectorOP_2D.GetDistance(a_tCarData.tCarTirepositionR, tRotateCenter);
                eLength_L2O = rtVectorOP_2D.GetDistance(a_tCarData.tCarTirepositionL, tRotateCenter);

                if (a_tCarData.eCarTireSpeedLeft > a_tCarData.eCarTireSpeedRight || a_tCarData.eWheelAngle < 0)
                { // 往右轉
                    eSpeed = Math.Abs(a_tCarData.eCarTireSpeedLeft) * eLength_C2O / eLength_L2O;
                }
                else
                { // 往左轉
                    eSpeed = Math.Abs(a_tCarData.eCarTireSpeedRight) * eLength_C2O / eLength_R2O;
                }

                eDistance = eSpeed * a_eDeltaTime; // distance = V x T = 所旋轉的弧長

                ePhi = eDistance / eLength_C2O; // 這裡單位是徑度 >> 旋轉角度 = 弧長 / 旋轉半徑

                if (eT >= 0)
                { // 旋轉中心在右邊 >> 旋轉角度要取負值
                    ePhi = -ePhi;
                }

                if (a_CMotorInfo.tMotorData.lMotorPower < 0)
                { // 馬達反轉 角度也要取負號
                    ePhi = -ePhi;
                }
                ePhiRad = ePhi * 180 / Math.PI;

                tNextPosition = rtVectorOP_2D.Rotate(a_CurrentPos, tRotateCenter, ePhi);
            }
            else
            { // 直行模式
                ePhi = 0;
                ePhiRad = ePhi * 180 / Math.PI;

                eSpeed = (a_tCarData.eCarTireSpeedLeft + a_tCarData.eCarTireSpeedRight) / 2;
                eDistance = eSpeed * (1 / FREQUENCY); // distance = V x T
                if (a_CMotorInfo.tMotorData.lMotorPower >= 0)
                {
                    tNextPosition.eX = a_CurrentPos.eX + eDistance * tV_Car.eX;
                    tNextPosition.eY = a_CurrentPos.eY + eDistance * tV_Car.eY;
                }
                else
                {
                    tNextPosition.eX = a_CurrentPos.eX - eDistance * tV_Car.eX;
                    tNextPosition.eY = a_CurrentPos.eY - eDistance * tV_Car.eY;
                }
            }

            a_CMotorInfo.tMotorData.eDeltaAngle = ePhiRad;

            return tNextPosition;
        }


        /// <summary> Target Angle Calculation  </summary>
        /// <param name="a_tCarData">[IN] car data </param>
        /// <param name="a_lTurnRadius">[IN] Turn Radius </param>
        /// <returns> Target Angle of car </returns>
        public static double TargetAngle_Cal(rtCarData a_tCarData, int a_lTurnRadius)
        {
            double eTargetAngle = 0, eLengthM2C = 0, eTanTheta = 0;

            eLengthM2C = rtVectorOP_2D.GetDistance(a_tCarData.tMotorPosition, a_tCarData.tPosition);
            eTanTheta = a_lTurnRadius / eLengthM2C;
            eTargetAngle = Math.Atan(eTanTheta);
            eTargetAngle = 90 - (eTargetAngle * 180 / Math.PI);

            return eTargetAngle;
        }

        /// <summary> Reset Car Alignment Flag  </summary>
        public void ResetCarAlignmentFlag()
        {
            bAlignmentCarAngleMatch = false;
        }


        /// <summary> Car direction Alignment  </summary>
        /// <param name="a_eTargetAngle">[IN] Target Angle of car </param>
        /// <param name="a_tCarData">[IN] car data </param>
        /// <returns> true: finish false: not yet </returns>
        public bool CarAngleAlignment(double a_eTargetAngle, rtCarData a_tCarData)
        {
            bool bMatched = false;
            double eAngleError = 0;
            double eAngleDelay = 0; // 確認輪胎有轉到90度

            eAngleError = rtAngleDiff.GetAngleDiff(a_eTargetAngle, a_tCarData.eAngle);
            if (Math.Abs(eAngleError) <= ANGLE_MATCH_TH || bAlignmentCarAngleMatch)
            {
                bAlignmentCarAngleMatch = true; // 代表已轉正 >>正在迴正輪胎
                tMotorData.lMotorPower = 0;
                tMotorData.lMotorAngle = 0;

                // 車輪轉回0度才結束
                if (Math.Abs(a_tCarData.eWheelAngle) < ANGLE_MATCH_TH)
                {
                    bAlignmentCarAngleMatch = false;    // 結束時要復原這個旗標 or not
                    bMatched = true;
                }
                else
                {
                    bMatched = false;
                }
            }
            else
            {
                if(eAngleError > 0)
                {
                    tMotorData.lMotorAngle = ANGLE_ROTATION;
                }
                else
                {
                    tMotorData.lMotorAngle = -ANGLE_ROTATION;
                }
                eAngleDelay = tMotorData.lMotorAngle - a_tCarData.eWheelAngle;
                if(Math.Abs(eAngleDelay) < ANGLE_MATCH_TH)
                {
                    if (Math.Abs(eAngleError) > ALIGNMENT_SPEED_ANGLE_TH)
                    {
                        tMotorData.lMotorPower = TURN_POWER;
                    }
                    else
                    {
                        tMotorData.lMotorPower = MIN_POWER;
                    }   
                }
                else
                {
                    tMotorData.lMotorPower = 0;
                }
                bMatched = false;
            }
            return bMatched;
        }


        /// <summary> Path Angle Offset Calculation  </summary>
        /// <param name="a_eDistance">[IN] distance to the path </param>
        /// <param name="a_tPID_ThetaOffsetCoe">[IN] PID Coe </param>
        /// <returns> angle Offset </returns>
        public static double PathAngleOffsetCal(double a_eDistance, rtPID_Coefficient a_tPID_ThetaOffsetCoe)
        {
            double eThetaOffset = 0;

            if (Math.Abs(a_eDistance) > PATH_ERROR_LIMT)
            {   // 超過距離限制 最多跟路徑差70度
                eThetaOffset = (a_eDistance > 0) ? MAX_ANGLE_OFFSET_PATH : -MAX_ANGLE_OFFSET_PATH;
            }
            else
            {   // 按系數計算
                eThetaOffset = a_eDistance * a_tPID_ThetaOffsetCoe.eKp;
            }

            if (Math.Abs(eThetaOffset) > MAX_ANGLE_OFFSET_PATH)
            {   // 超過距離限制 最多跟路徑差70度
                eThetaOffset = (eThetaOffset > 0) ? MAX_ANGLE_OFFSET_PATH : -MAX_ANGLE_OFFSET_PATH;
            }

            return eThetaOffset;
        }


        /// <summary> Motor Angle Calculation: find out how to control wheel direction  </summary>
        /// <param name="a_eDeltaCarAngle">[IN] Car Angle offset to wanted direction </param>
        /// <param name="a_eCarSpeed">[IN] Car Speed </param>
        /// <param name="a_tPID_MotorAngleCoe">[IN] PID Coe of dorection control </param>
        /// <returns> Motor Angle </returns>
        public static double MotorAngleCal(double a_eDeltaCarAngle, double a_eCarSpeed, rtPID_Coefficient a_tPID_MotorAngleCoe)
        {   // 目前不考慮 車速的因素
            double MotorAngle = 0;

            if (a_eDeltaCarAngle > DELTA_CAR_ANGLE_LIMIT)
            {   // 超過角度限制 最多轉70度
                MotorAngle = (a_eDeltaCarAngle > 0) ? MAX_ANGLE_OFFSET_MOTOR : -MAX_ANGLE_OFFSET_MOTOR;
            }
            else
            {   // 按系數計算
                MotorAngle = a_eDeltaCarAngle * a_tPID_MotorAngleCoe.eKp;
            }
            return MotorAngle;
        }

        /// <summary> Target Angle Offset Modify by car speed  </summary>
        /// <param name="a_eDistanceEroor">[IN] diatance to the path </param>
        /// <param name="a_eCenterSpeed">[IN] Car Speed </param>
        /// <param name="a_eTargetAngleOffset">[IN] Target Angle Offset </param>
        /// <returns> modified Target Angle Offset </returns>
        public static double TargetAngleOffsetModify(double a_eDistanceEroor, double a_eCenterSpeed, double a_eTargetAngleOffset)
        {
            double eAbsDistanceEroor = 0, eLength = 0, eLengthModify = 0, eAbsTargetAngleOffset = 0, eModifiedAngleOffset = 0;

            if (Math.Abs(a_eCenterSpeed) < BASE_SPEED || a_eCenterSpeed==0)
            {   // 比基礎速度小 or 車速為0 >> 不用改
                return a_eTargetAngleOffset;
            }

            eAbsDistanceEroor = Math.Abs(a_eDistanceEroor);
            eAbsTargetAngleOffset = Math.Abs(a_eTargetAngleOffset);

            if (eAbsTargetAngleOffset < ANGLE_MATCH_TH || eAbsDistanceEroor==0)
            {   // 1. 角度太小就不修正 避免三角函數算錯 2. eAbsDistanceEroor = 0 也不修正 >>　浪費時間
                return a_eTargetAngleOffset;
            }
            eLength = eAbsDistanceEroor / (Math.Sin(eAbsTargetAngleOffset * Math.PI / 180));
            eLengthModify = Math.Abs(a_eCenterSpeed) / BASE_SPEED * eLength;

#if rtAGV_DEBUG_PRINT
            Console.WriteLine("eLength:" + eLength.ToString());
            Console.WriteLine("eLengthModify:" + eLengthModify.ToString());
#endif

            if (eLengthModify == 0)
            {   // 速度 = 0 就不更改
                return a_eTargetAngleOffset;
            }
            eModifiedAngleOffset = Math.Asin(eAbsDistanceEroor / eLengthModify);
            eModifiedAngleOffset = eModifiedAngleOffset * 180 / Math.PI;
            eModifiedAngleOffset = (a_eTargetAngleOffset < 0) ? -eModifiedAngleOffset : eModifiedAngleOffset;

            return eModifiedAngleOffset;
        }

        /// <summary> Path Offset Calculate  </summary>
        /// <param name="a_atPathInfo">[IN] information of path </param>
        /// <param name="a_eDestDirection">[IN] Destination Direction </param>
        public void PathOffsetCal(rtPath_Info[] a_atPathInfo, double a_eDestDirection)
        {
            int lPathIndex = 0;
            double eTmp = 0, eDeltaTmp = 0;
            rtVector tDestVector = new rtVector();
            rtVector tPathVector = new rtVector();

            lPathIndex = tMotorData.lPathNodeIndex;
            tPathVector.eX = a_atPathInfo[lPathIndex].tDest.eX - a_atPathInfo[lPathIndex].tSrc.eX;
            tPathVector.eY = a_atPathInfo[lPathIndex].tDest.eY - a_atPathInfo[lPathIndex].tSrc.eY;
            tDestVector = rtVectorOP_2D.Angle2Vector(a_eDestDirection);
            eDeltaTmp = rtVectorOP_2D.GetTheta(tPathVector, tDestVector);
            if (Link2DestCheck(a_atPathInfo, lPathIndex) && eDeltaTmp > DELTA_ANGLE_TH)
            {   // need offset
                eTmp = rtVectorOP_2D.Cross(tPathVector, tDestVector);

                if (eTmp > 0)
                {   // 車子要往左轉 (不管前進或到退走) >> offset 為負
                    tMotorData.lNavigateOffset = DEFAULT_PATH_OFFSET;
                }
                else
                {   // 車子要往右轉 (不管前進或到退走) >> offset 為正
                    tMotorData.lNavigateOffset = -DEFAULT_PATH_OFFSET;
                }
            }
            else
            {   // do not need offset
                tMotorData.lNavigateOffset = 0;
            }
        }

        /// <summary> Determine Path Vector in Turn Mode  </summary>
        /// <param name="a_tPosition">[IN] car position </param>
        /// <param name="a_tTurnCenter">[IN] Turn Center </param>
        /// <param name="a_lTurnDirection">[IN] Turn Direction </param>
        /// <returns> Path Vector </returns>
        public rtVector PathVectorDetermine_TurnMode(rtVector a_tPosition, rtVector a_tTurnCenter, int a_lTurnDirection)
        {
            rtVector tPathVector = new rtVector();
            rtVector tVcenter2current = new rtVector();

            tVcenter2current = rtVectorOP_2D.GetVector(a_tTurnCenter, a_tPosition);
            switch (a_lTurnDirection)
            {
                case (int)rtTurnType_Simple.CAR_TURN_RIGHT: // 取右側的法向量 為路徑切線向量
                    tPathVector.eX = tVcenter2current.eY;
                    tPathVector.eY = -tVcenter2current.eX;
                    break;
                case (int)rtTurnType_Simple.CAR_TURN_LEFT:  // 取左側的法向量 為路徑切線向量
                    tPathVector.eX = -tVcenter2current.eY;
                    tPathVector.eY = tVcenter2current.eX;
                    break;
                default:    // 錯誤
                    tPathVector.Init();
                    break;
            }
            return tPathVector;
        }

        /// <summary> Determine Motor Angle during Smooth Turn  </summary>
        /// <param name="a_atPathInfo">[IN] information of path </param>
        /// <param name="a_tCarData">[IN] car data </param>
        public void MotorAngle_CtrlNavigateSmoothTurn(rtPath_Info[] a_atPathInfo, rtCarData a_tCarData)
        {
            double eDistance = 0, eThetaError = 0, eCarAngle = 0, eMotorAngleOffset = 0, eMototAngleTmp = 0;
            int lPathIndex = 0;
            rtVector tPathVector = new rtVector();
            rtVector tPostion = new rtVector();

            // 倒退走: 以驅動車輪為路徑偏差基準    往前走: 以兩導輪中心為路徑偏差基準
            tPostion = (tMotorData.bBackWard) ? a_tCarData.tMotorPosition : a_tCarData.tPosition;

            lPathIndex = tMotorData.lPathNodeIndex;
            eCarAngle = a_tCarData.eAngle;
#if rtAGV_BACK_MODE
            if (tMotorData.bBackWard)
            {   // 倒退走 >> 在某些情況下 要用車尾方向來計算
                eCarAngle = (eCarAngle > 180) ? eCarAngle - 180 : eCarAngle + 180;
            }
#endif
            tPathVector = rtVectorOP_2D.GetVector(a_atPathInfo[lPathIndex].tSrc, a_atPathInfo[lPathIndex].tDest);

            //  算出車子要往左轉還是右轉
            tMotorData.lTurnDirection = TurnDirectionCal(a_atPathInfo, tMotorData.lPathNodeIndex);

            eDistance = PathErrorCal_Turn(a_atPathInfo[lPathIndex], tPostion, tMotorData.tTurnCenter, tMotorData.lTurnRadius);
            tPathVector = PathVectorDetermine_TurnMode(tPostion, tMotorData.tTurnCenter, tMotorData.lTurnDirection);

            // 用算出的旋轉半徑得知車輪至少要轉幾度 >> 目前僅在正走時才需要 Offset 所以不需考慮反走的種種情況
            if (tMotorData.bBackWard == false)
            {
                eMotorAngleOffset = TargetAngle_Cal(a_tCarData, tMotorData.lTurnRadius);
                eMotorAngleOffset = eMotorAngleOffset * tMotorData.lTurnDirection;
            }
      
            tMotorData.lTargetAngle = eMotorAngleOffset;

            // 算出車身角度和路徑角度偏差多少 (這裡目前僅為了 LOG)
            eThetaError = rtAngleDiff.GetAngleDiff(tPathVector, eCarAngle);
            tMotorData.Debug_ePathThetaError = eThetaError;

            // 考慮靠左 or 靠右的 offset
            tMotorData.ePathError = eDistance + tMotorData.lNavigateOffset;

            eMototAngleTmp = DedtermineMotorAngleByPathError(tPathVector, a_tCarData);
            tMotorData.lMotorAngle = (int)(Math.Round(eMototAngleTmp));

            // boundary
            if (Math.Abs(tMotorData.lMotorAngle) > MAX_ANGLE_OFFSET_MOTOR)
            {
                tMotorData.lMotorAngle = (tMotorData.lMotorAngle < 0) ? -MAX_ANGLE_OFFSET_MOTOR : MAX_ANGLE_OFFSET_MOTOR;
            }
        }

        /// <summary> Determine Motor Angle Straight mode  </summary>
        /// <param name="a_atPathInfo">[IN] information of path </param>
        /// <param name="a_tCarData">[IN] car data </param>
        public void MotorAngle_CtrlNavigateStraight(rtPath_Info[] a_atPathInfo, rtCarData a_tCarData)
        {
            double eDistance = 0, eThetaError = 0, eCarAngle = 0, eMotorAngleOffset = 0, eMototAngleTmp = 0;
            int lPathIndex = 0;
            rtVector tPathVector = new rtVector();
            rtVector tPostion = new rtVector();

            // 倒退走: 以驅動車輪為路徑偏差基準    往前走: 以兩導輪中心為路徑偏差基準
            tPostion = (tMotorData.bBackWard) ? a_tCarData.tMotorPosition : a_tCarData.tPosition;

            lPathIndex = tMotorData.lPathNodeIndex;
            eCarAngle = a_tCarData.eAngle;
#if rtAGV_BACK_MODE
            if (tMotorData.bBackWard)
            {   // 倒退走 >> 在某些情況下 要用車尾方向來計算
                eCarAngle = (eCarAngle > 180) ? eCarAngle - 180 : eCarAngle + 180;
            }
#endif
            tPathVector = rtVectorOP_2D.GetVector(a_atPathInfo[lPathIndex].tSrc, a_atPathInfo[lPathIndex].tDest);
            eDistance = PathErrorCal_Straight(a_atPathInfo[lPathIndex], tPostion);
            tMotorData.lTargetAngle = eMotorAngleOffset;

            // 算出車身角度和路徑角度偏差多少 (這裡目前僅為了 LOG)
            eThetaError = rtAngleDiff.GetAngleDiff(tPathVector, eCarAngle);
            tMotorData.Debug_ePathThetaError = eThetaError;

            // 考慮靠左 or 靠右的 offset
            tMotorData.ePathError = eDistance + tMotorData.lNavigateOffset;

            eMototAngleTmp = DedtermineMotorAngleByPathError(tPathVector, a_tCarData);
            tMotorData.lMotorAngle = (int)(Math.Round(eMototAngleTmp));

            // boundary
            if (Math.Abs(tMotorData.lMotorAngle) > MAX_ANGLE_OFFSET_MOTOR)
            {
                tMotorData.lMotorAngle = (tMotorData.lMotorAngle < 0) ? -MAX_ANGLE_OFFSET_MOTOR : MAX_ANGLE_OFFSET_MOTOR;
            }
        }

        /// <summary> Determine Motor Angle By Path Error  </summary>
        /// <param name="a_tPathVector">[IN] Path Vector </param>
        /// <param name="a_tCarData">[IN] car data </param>
        /// <returns> Motor Angle </returns>
        public double DedtermineMotorAngleByPathError(rtVector a_tPathVector, rtCarData a_tCarData)
        {
            double eMototAngle = 0, eTargetCarAngleOffset = 0, eCarCenterSpeed = 0, eDeltaCarAngle = 0, eCarAngle = 0;
            rtVector tTargetCarVector = new rtVector();

#if rtAGV_BACK_MODE
            eCarAngle = a_tCarData.eAngle;
            if (tMotorData.bBackWard)
            {   // 倒退走 >> 在某些情況下 要用車尾方向來計算
                eCarAngle = (eCarAngle > 180) ? eCarAngle - 180 : eCarAngle + 180;
            }
#endif
            // 算出要跟路徑的夾角
            eTargetCarAngleOffset = PathAngleOffsetCal(tMotorData.ePathError, tMotorCfg.tPID_ThetaOffsetCoe);

            tMotorData.Debug_TargetAngleOffset1 = eTargetCarAngleOffset;

            // 算出兩輪中心的速度 (以考慮正走反走的狀況: 車速的正負號)
            eCarCenterSpeed = CarCenterSpeedCal(a_tCarData);
            tMotorData.Debug_CenterSpeed = eCarCenterSpeed;

#if rtAGV_DEBUG_OFFSET_MODIFY
            // 根據車速調整跟路徑的夾角
            eTargetCarAngleOffset = TargetAngleOffsetModify(tMotorData.ePathError, eCarCenterSpeed, eTargetCarAngleOffset);
#endif
            tMotorData.Debug_TargetAngleOffset2 = eTargetCarAngleOffset;

            // 算出目標車身角度 (vector format)
            tTargetCarVector = rtVectorOP_2D.Rotate(a_tPathVector, new rtVector(0, 0), eTargetCarAngleOffset * Math.PI / 180);

            // 算出目標車身角度與當下車身角度的差距
            eDeltaCarAngle = rtAngleDiff.GetAngleDiff(tTargetCarVector, eCarAngle);
#if rtAGV_BACK_MODE
            if (tMotorData.bBackWard)
            {   // 倒退走 >> 同樣角度差距下 正走跟反走 車輪要打的方向相反
                eDeltaCarAngle = -eDeltaCarAngle;
            }
#endif
            tMotorData.Debug_eDeltaCarAngle = eDeltaCarAngle;

            // 用角度差距算出 適當的車輪馬達轉角
            eMototAngle = MotorAngleCal(eDeltaCarAngle, 0, tMotorCfg.tPID_MotorAngleCoe);

            // 加上之前的角度 offset
            eMototAngle = eMototAngle + tMotorData.lTargetAngle;

            return eMototAngle;
        }
    }

    /// <summary> struct of Fork Control Data  </summary>
    public struct rtForkCtrl_Data
    {
        public byte ucStatus;

        public int height;

        public int distanceDepth;

        public bool bEnable;

        /// <summary> init struct of Fork Control Data  </summary>
        public void Init()
        {
            ucStatus = (byte)rtForkCtrl.ForkStatus.NULL;
            height = 0;
            distanceDepth = 0;
            bEnable = false;
        }
    }

    /// <summary> core class of Fork Control  </summary>
    public class rtForkCtrl
    {
        /// <summary> 堆高機貨叉狀態宣告 </summary>
		/** \brief    */
        public enum ForkStatus { NULL, ALIMENT, SET_HEIGHT, FORTH, BACKWARD, PICKUP, PICKDOWN, RESET_HEIGHT, FINISH, ERROR, SET_DEPTH, RESET_DEPTH, ALIMENT_FORTH };

		/** \brief 堆高機貨叉動作模式   */
        public enum ForkActionMode { LOAD = 0,UNLOAD = 1};

		/** \brief Define: 判斷是否到達要的高度跟深度   */
        public const double FORK_MATCH_TH = 2;

		/** \brief Define: 貨叉最大深度   */
        public const int FORK_MAX_DEPTH = 4500;

		/** \brief Define: 貨叉舉起高度   */
        public const int FORK_PICKUP_HEIGHT = 10;

		/** \brief Fork Control Data   */
        public rtForkCtrl_Data tForkData;

        /// <summary> construct function  </summary>
        public rtForkCtrl()
        {
            tForkData.Init();
        }

        public static void LOADTest(ref rtForkCtrl_Data tForkData)
        {
            tForkData.height = 125;
            tForkData.distanceDepth = FORK_MAX_DEPTH;
            tForkData.bEnable = true;
        }

        public static void UNLOADTest(ref rtForkCtrl_Data tForkData)
        {
            tForkData.height = 125;
            tForkData.distanceDepth = FORK_MAX_DEPTH;
            tForkData.bEnable = true;
        }

     }
}