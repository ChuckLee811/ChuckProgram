using System;

namespace AlgorithmTool
{
    /// <summary> the struct of 2D Vector </summary>
    public struct rtVector
    {
        public double eX;
        public double eY;

        /// <summary> reset coordinate </summary>
        public void Init()
        {
            eX = 0;
            eY = 0;
        }

        /// <summary> Constructor </summary>
        public rtVector(double a_eX, double a_eY)
        {
            eX = a_eX;
            eY = a_eY;
        }

        /// <summary> set coordinate </summary>
        public void Set(double a_eX, double a_eY)
        {
            eX = a_eX;
            eY = a_eY;
        }

    }

    /// <summary> the struct of Path Infomation </summary>
    public struct rtPath_Info
    {
        public enum rtStatus { STRAIGHT = 1, TURN = 2, DONE = 0 };

        public enum rtTurnType { SIMPLE = 0, SMOOTH = 1, ARRIVE = 2, PARK = 3 };

		/** \brief source position   */
        public rtVector tSrc;

		/** \brief destination position   */
        public rtVector tDest;

		/** \brief status of this segment   */
        public byte ucStatus;

		/** \brief turn ytpe of this segment   */
        public byte ucTurnType;

        /// <summary> init path with length </summary>
        /// <param name="a_lPathLength">[IN] Path's Length</param>
        /// <returns> Path struct </returns>
        public static rtPath_Info[] InitSet(int a_lPathLength)
        {
            int lCnt = 0;
            rtPath_Info[] atPathInfo = new rtPath_Info[a_lPathLength];

            for(lCnt = 0; lCnt< a_lPathLength - 1; lCnt++)
            {
                atPathInfo[lCnt].tSrc.Init();
                atPathInfo[lCnt].tDest.Init();
                atPathInfo[lCnt].ucStatus = (byte)rtStatus.STRAIGHT;
                atPathInfo[lCnt].ucTurnType = (byte)rtTurnType.SIMPLE;
            }

            if (a_lPathLength != 0)
            {
                atPathInfo[lCnt].tSrc.Init();
                atPathInfo[lCnt].tDest.Init();
                atPathInfo[lCnt].ucStatus = (byte)rtStatus.STRAIGHT;
                atPathInfo[lCnt].ucTurnType = (byte)rtTurnType.ARRIVE;
            }
            return atPathInfo;
        }
    }

    /// <summary> the struct of car current Infomation </summary>
    public struct rtCarData
    {
		/** \brief car position   */
        public rtVector tPosition;

		/** \brief Motor position   */
        public rtVector tMotorPosition;

		/** \brief car left Tire position   */
        public rtVector tCarTirepositionL;

		/** \brief car right Tire position   */
        public rtVector tCarTirepositionR;

		/** \brief car angle (direction)   */
        public double eAngle;

		/** \brief car left Tire speed   */
        public double eCarTireSpeedLeft;

		/** \brief car right Tire speed   */
        public double eCarTireSpeedRight;

		/** \brief Motor(or wheel) angle (direction)   */
        public double eWheelAngle;

        /// <summary> initail car Infomation </summary>
        public void Init()
        {
            tPosition.Init();
            tMotorPosition.Init();
            tCarTirepositionL.Init();
            tCarTirepositionR.Init();
            eAngle = 0;
            eCarTireSpeedLeft = 0;
            eCarTireSpeedRight = 0;
            eWheelAngle = 0;
        }
    }

    /// <summary> the struct of ROI </summary>
    public struct ROI
    {
		/** \brief Start X   */
        public int lStartX;

		/** \brief Start Y   */
        public int lStartY;

		/** \brief width   */
        public int lwidth;

		/** \brief height   */
        public int lheight;
    }

    /// <summary> the struct of Node, including region and node index </summary>
    public struct NodeId
    {
		/** \brief region index   */
        public int lRegion;

		/** \brief node index   */
        public int lIndex;
    }

    /// <summary> the struct of Warehouse position, including region, direction and node index </summary>
    public struct WarehousPos
    {
		/** \brief region index   */
        public int lRegion;

		/** \brief node index   */
        public int lIndex;

		/** \brief Warehousing Direction: Radius   */
        public double eDirection;
    }

    /// <summary> the struct of Warehouse detail information </summary>
    public struct rtWarehousingInfo
    {
		/** \brief Node Id: 代表他連結到MAP中哪一個結點   */
        public NodeId tNodeId;

		/** \brief Warehousing position   */
        public rtVector tCoordinate;

		/** \brief Warehousing Height   */
        public double eHeight;

		/** \brief Warehousing Direction: Radius   */
        public double eDirection;

        public double DistanceDepth;
    }

    /// <summary> Node struct of AGV MAP </summary>
    public struct rtAGV_MAP_node
    {
		/** \brief Node Id   */
        public NodeId tNodeId;

		/** \brief node position   */
        public rtVector tCoordinate;

		/** \brief Link of region: length > 0 表示此點可以通往其它 region   */
        public int[] alLinkofRegion;

		/** \brief index of linked region   */
        public int[] alIndexfRegion;
    }

    /// <summary> Struct of AGV MAP </summary>
    public struct rtAGV_MAP
    {
		/** \brief Region number: 區域個數   */
        public int alRegionNum;

		/** \brief node number in local: 每一區節點個數，array長度代表區域個數   */
        public int[] alNodeNumLocal;

		/** \brief node in global 區域節點   */
        public rtAGV_MAP_node[] atNodeGlobal;

		/** \brief node in local 每一區的節點   */
        public rtAGV_MAP_node[][] atNodeLocal;

		/** \brief Map table in global : 區域間的路徑權重表 算最短路徑用   */
        public int[] alPathTableGlobal;

		/** \brief Map table in local : 每區的路徑權重表 算最短路徑用   */
        public int[][] alPathTableLocal;

        /// <summary> Initial function </summary>
        public void Init()
        {
            alRegionNum = 0;
            alNodeNumLocal = new int[alRegionNum];
            atNodeGlobal = new rtAGV_MAP_node[alRegionNum];
            atNodeLocal = new rtAGV_MAP_node[alRegionNum][];
            alPathTableGlobal = new int[alRegionNum];
            alPathTableLocal = new int[alRegionNum][];
        }
    }

    /// <summary> 用各種不同的方式求角度的差距 </summary>
    public class rtAngleDiff
    {
        /// <summary> 求向量a_tIn 要順時針轉幾度才會到 向量a_tTarget </summary>
        public static double GetAngleDiff(rtVector a_tTarget, rtVector a_tIn)
        {
            double eTheta = 0, eCross = 0;

            eTheta = rtVectorOP_2D.GetTheta(a_tTarget, a_tIn);
            eCross = rtVectorOP_2D.Cross(a_tIn, a_tTarget);
            eTheta = (eCross < 0) ? -eTheta : eTheta;

            return eTheta; 
        }

        /// <summary> 求角度a_tIn 要順時針轉幾度才會到 角度a_tTarget </summary>
        public static double GetAngleDiff(double a_eTarget, double a_eIn)
        {
            double eTheta = 0, eCross = 0;
            rtVector tTarget = new rtVector();
            rtVector tIn = new rtVector();

            tTarget = rtVectorOP_2D.Angle2Vector(a_eTarget);
            tIn = rtVectorOP_2D.Angle2Vector(a_eIn);
            eTheta = rtVectorOP_2D.GetTheta(tTarget, tIn);
            eCross = rtVectorOP_2D.Cross(tIn, tTarget);
            eTheta = (eCross < 0) ? -eTheta : eTheta;

            return eTheta;
        }

        /// <summary> 求角度a_tIn 要順時針轉幾度才會到 向量a_tTarget </summary>
        public static double GetAngleDiff(rtVector a_tTarget, double a_eIn)
        {
            double eTheta = 0, eCross = 0;
            rtVector tIn = new rtVector();

            tIn = rtVectorOP_2D.Angle2Vector(a_eIn);
            eTheta = rtVectorOP_2D.GetTheta(a_tTarget, tIn);
            eCross = rtVectorOP_2D.Cross(tIn, a_tTarget);
            eTheta = (eCross < 0) ? -eTheta : eTheta;

            return eTheta;
        }

        /// <summary> 求向量a_tIn 要順時針轉幾度才會到 角度a_tTarget </summary>
        public static double GetAngleDiff(double a_eTarget, rtVector a_tIn)
        {
            double eTheta = 0, eCross = 0;
            rtVector tTarget = new rtVector();

            tTarget = rtVectorOP_2D.Angle2Vector(a_eTarget);
            eTheta = rtVectorOP_2D.GetTheta(tTarget, a_tIn);
            eCross = rtVectorOP_2D.Cross(a_tIn, tTarget);
            eTheta = (eCross < 0) ? -eTheta : eTheta;

            return eTheta;
        }
    }

    /// <summary> 2D Vector operation </summary>
    public class rtVectorOP_2D
    {
        /// <summary> 求某一點對某向量延伸一定長度 </summary>
        public static rtVector ExtendPointAlongVector(rtVector a_tPoint, rtVector a_tDirection, int a_lExtendSize)
        {
            double eT = 0, eSizeVetor = 0;
            rtVector tExtendPoint = new rtVector();

            eSizeVetor = GetLength(a_tDirection);
            eT = a_lExtendSize / eSizeVetor;
            tExtendPoint.eX = a_tPoint.eX + a_tDirection.eX * eT;
            tExtendPoint.eY = a_tPoint.eY + a_tDirection.eY * eT;

            return tExtendPoint;
        }

        /// <summary> 求某向量長度 </summary>
        public static double GetLength(rtVector a_tIn)
        {
            double eOut = 0;
            eOut = Math.Sqrt(a_tIn.eX * a_tIn.eX + a_tIn.eY * a_tIn.eY);
            return eOut;
        }

        /// <summary> 求某向量內積 </summary>
        public static double Dot(rtVector a_tV1, rtVector a_tV2)
        {
            double eOut = 0;

            eOut = a_tV1.eX * a_tV2.eX + a_tV1.eY * a_tV2.eY;
            return eOut;
        }

        /// <summary> 求某向量外積 (a_tV1 * a_tV2) </summary>
        public static double Cross(rtVector a_tV1, rtVector a_tV2)
        {
            double eOut = 0;

            eOut = a_tV1.eX * a_tV2.eY - a_tV1.eY * a_tV2.eX;
            return eOut;
        }

        /// <summary> 求兩向量的夾角 </summary>
        public static double GetTheta(rtVector a_tV1, rtVector a_tV2)
        {
            double eTheta = 0;

            eTheta = Dot(a_tV1, a_tV2);
            eTheta /= GetLength(a_tV1) * GetLength(a_tV2);
            eTheta = Math.Acos(eTheta) * 180.0 / Math.PI;
            return eTheta;
        }

        public static double FindVectorMultipleOfMeetPoint(rtVector a_tSrc1, rtVector a_tV1, rtVector a_tSrc2, rtVector a_tV2)
        {
            double eT1 = 0;

            // 求交點對應的向量係數 for Line 1
            eT1 = a_tSrc2.eX * a_tV2.eY - a_tSrc2.eY * a_tV2.eX - a_tSrc1.eX * a_tV2.eY + a_tSrc1.eY * a_tV2.eX;
            eT1 /= a_tV1.eX * a_tV2.eY - a_tV1.eY * a_tV2.eX;

            return eT1;
        }

        /// <summary> 求兩線段的交點 </summary>
        public static rtVector FindMeetPoint(rtVector a_tSrc1, rtVector a_tV1, rtVector a_tSrc2, rtVector a_tV2)
        {
            rtVector tMeetPoint = new rtVector();
            double eT1 = 0;

            eT1 = FindVectorMultipleOfMeetPoint(a_tSrc1, a_tV1, a_tSrc2, a_tV2);

            // 求交點(current point 沿著向量與路徑的交點)
            tMeetPoint.eX = a_tSrc1.eX + a_tV1.eX * eT1;
            tMeetPoint.eY = a_tSrc1.eY + a_tV1.eY * eT1;

            return tMeetPoint;
        }

        /// <summary> 求某向量乘上某一純量 </summary>
        public static rtVector VectorMultiple(rtVector a_tSrc, double a_eMultiple)
        {
            rtVector tMultipleVector = new rtVector();

            tMultipleVector.eX = a_tSrc.eX * a_eMultiple;
            tMultipleVector.eY = a_tSrc.eY * a_eMultiple;

            return tMultipleVector;
        }

        /// <summary> 求某向量對另一向量的投影 </summary>
        public static rtVector VectorProject(rtVector a_tSrc, rtVector a_tBase)
        {
            rtVector tProjectedVector = new rtVector();
            double eMultiple = 0;
            double eDistanceBase = 0;

            eDistanceBase = GetLength(a_tBase);

            eMultiple = Dot(a_tSrc, a_tBase) / (eDistanceBase * eDistanceBase);

            tProjectedVector = VectorMultiple(a_tBase, eMultiple);

            return tProjectedVector;
        }

        /// <summary> 求某一點對某中心做旋轉的結果 </summary>
        public static rtVector Rotate(rtVector a_tPoint, rtVector a_tCenter, double a_eTheta)
        {   // 角度單位是徑度 甭轉換
            rtVector tResult = new rtVector();
            rtVector tTmp, tTmp1;

            tTmp.eX = a_tPoint.eX - a_tCenter.eX;
            tTmp.eY = a_tPoint.eY - a_tCenter.eY;

            tTmp1.eX = Math.Cos(a_eTheta) * tTmp.eX - Math.Sin(a_eTheta) * tTmp.eY;
            tTmp1.eY = Math.Sin(a_eTheta) * tTmp.eX + Math.Cos(a_eTheta) * tTmp.eY;

            tResult.eX = tTmp1.eX + a_tCenter.eX;
            tResult.eY = tTmp1.eY + a_tCenter.eY;

            return tResult;
        }

        public static double GetDistance(rtVector a_tP1, rtVector a_tP2)
        {
            double eDistance = 0;
            double eGapX = 0, eGapY = 0;

            eGapX = a_tP2.eX - a_tP1.eX;
            eGapY = a_tP2.eY - a_tP1.eY;
            eDistance = Math.Sqrt(eGapX * eGapX + eGapY * eGapY);

            return eDistance;
        }

        /// <summary> 求兩點構成的向量 </summary>
        public static rtVector GetVector(rtVector a_tP_Src, rtVector a_tP_Dest)
        {
            rtVector tVector = new rtVector();

            tVector.eX = a_tP_Dest.eX - a_tP_Src.eX;
            tVector.eY = a_tP_Dest.eY - a_tP_Src.eY;

            return tVector;
        }

        /// <summary> 給角度回傳對應的單位向量 </summary>
        public static rtVector Angle2Vector(double a_eAngle)
        {   // 回傳單位向量
            rtVector tVector = new rtVector();

            tVector.eX = Math.Cos(a_eAngle * Math.PI / 180);
            tVector.eY = Math.Sin(a_eAngle * Math.PI / 180);

            return tVector;
        }

        /// <summary> 給向量回傳對應角度 0~360度 </summary>
        public static double Vector2Angle(rtVector a_tVector)
        {
            double eAngle = 0;

            if (a_tVector.eX == 0)
            {
                if (a_tVector.eY > 0)
                {
                    eAngle = 90;
                }
                else if (a_tVector.eY < 0)
                {
                    eAngle = -90;
                }
                else
                {
                    // show error msg
                    eAngle = 0;
                }
            }
            else
            {
                eAngle = Math.Atan(a_tVector.eY / a_tVector.eX) * 180 / Math.PI;

                if (eAngle > 0)
                {
                    if (a_tVector.eX < 0)
                    {
                        eAngle += 180;
                    }
                }
                else
                {
                    if (a_tVector.eX < 0)
                    {
                        eAngle += 180;
                    }
                    else
                    {
                        eAngle += 360;
                    }
                }
            }

            return eAngle;
        }

        /// <summary> 求角度差距:  a_tV_Src轉到a_tV_Target 是順時針角度為正 否則為負 </summary>
        public static double GetTheta_Difference(rtVector a_tV_Src, rtVector a_tV_Target)
        {
            double eTheta = 0, eCross = 0;
            rtVector tCenter = new rtVector();

            tCenter.Init();
            eTheta = GetTheta(a_tV_Src, a_tV_Target);
            eCross = Cross(a_tV_Src, a_tV_Target);
            if (eCross < 0)
            {
                return -eTheta;
            }
            else
            {
                return eTheta;
            }
        }

    }
}