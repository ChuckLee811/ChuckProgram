// #define rtAGV_NAVIGATE_MULTI_REGION
#define rtAGV_DEBUG_PRINT_TABLE

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AlgorithmTool
{
    /// <summary> core class of Path Planning  </summary>
    public class rtPathPlanning
    {
		/** \brief Define: 最大路徑結點數   */
        public const int MAX_PATH_NODE_NUMBER = 100;

		/** \brief Define: 空資料   */
        public const int EMPTY_DATA = 99999;

        /// <summary> Path Planning for AGV navigation </summary>
        /// <param name="a_tMap">[IN] map for navigation </param>
        /// <param name="a_atWarehousingInfo">[IN] Warehouse Information </param>
        /// <param name="a_atRegionCfg">[IN] region configure </param>
        /// <param name="a_atPathInfo">[INOUT] Path data </param>
        /// <param name="a_tCarInfo">[IN] car data </param>
        /// <param name="a_tDestData">[IN] Destination data </param>
        /// <param name="a_atObstacle">[IN] Obstacle data </param>
        public static void rtAGV_PathPlanning(
            rtAGV_MAP a_tMap, rtWarehousingInfo[][] a_atWarehousingInfo, ROI[] a_atRegionCfg, 
            ref rtPath_Info[] a_atPathInfo, ref rtCarData a_tCarInfo, rtWarehousingInfo a_tDestData, ROI[] a_atObstacle)
        {
            NodeId tNodeId;
            int lNodeNum = 0, lPathLength = 0;
            int lRegionIndex = 0;

            int[] alPathofRegion = new int[0];

            //搜尋最靠近車體位置的節點
            tNodeId = rtAGV_FindCurrentNode(a_tMap.atNodeLocal[lRegionIndex], a_tCarInfo);
 

#if rtAGV_NAVIGATE_MULTI_REGION
            if (a_tDestData.tNodeId.lRegion == tNodeId.lRegion)
            {   // different region
                if(alPathofRegion.Length == 0)
                {
                    // show error
                }

                while (lRegionIndex < alPathofRegion.Length)
                {
                    FindPathNode2NextRegion();
                    lRegionIndex++;
                }
            }
#endif
            // already in same region
            lRegionIndex = a_tDestData.tNodeId.lRegion;
            lNodeNum = a_tMap.alNodeNumLocal[lRegionIndex];
            rtAGV_FindPathNode2Dest(
                tNodeId.lIndex, a_tDestData.tNodeId.lIndex,
                a_tMap.atNodeLocal[lRegionIndex], a_tMap.alPathTableLocal[lRegionIndex], lNodeNum, ref lPathLength, ref a_atPathInfo);
        }


        /// <summary> 搜尋最靠近車體位置的節點 </summary>
        /// <param name="a_atNodeLocal">[IN] node list </param>
        /// <param name="a_tCarInfo">[IN] car information </param>
        /// <returns> The closest node </returns>
        public static NodeId rtAGV_FindCurrentNode(rtAGV_MAP_node[] a_atNodeLocal, rtCarData a_tCarInfo)
        {
            NodeId tNodeId = new NodeId();
            double MinDistance = EMPTY_DATA;
            for (int i = 0; i < a_atNodeLocal.Length; i++)
            {
                double EachDistance = rtVectorOP_2D.GetDistance(a_atNodeLocal[i].tCoordinate, a_tCarInfo.tPosition);
                if (EachDistance <= MinDistance)
                {
                    MinDistance = EachDistance;
                    tNodeId = a_atNodeLocal[i].tNodeId;
                }
            }
            return tNodeId;
        }

        /// <summary> Find Node list of Path to Destination </summary>
        /// <param name="a_lSrc">[IN] source node </param>
        /// <param name="a_lDst">[IN] Destination node </param>
        /// <param name="a_atNodeLocal">[IN] node information in current region </param>
        /// <param name="a_alMapCurrent">[IN] look up table of node in current region </param>
        /// <param name="a_lNodeNum">[IN] node number in current region </param>
        /// <param name="a_lPathLength">[INOUT] wanted path size </param>
        /// <param name="a_atPathInfo">[INOUT] wanted path </param>
        public static void rtAGV_FindPathNode2Dest(int a_lSrc, int a_lDst, rtAGV_MAP_node[] a_atNodeLocal, int[] a_alMapCurrent, int a_lNodeNum, ref int a_lPathLength, ref rtPath_Info[] a_atPathInfo)
        {
            int lCnt = 0;
            int[] alPathResult = new int[0];

            alPathResult = FindPathofNode(a_lSrc, a_lDst, a_alMapCurrent, a_lNodeNum, ref a_lPathLength);

            a_lPathLength--;    // 路徑段數比節點數少1

            a_atPathInfo = rtPath_Info.InitSet(a_lPathLength);
            
            for (lCnt = 0; lCnt < a_lPathLength; lCnt++)
            {
                a_atPathInfo[lCnt].tSrc.eX = a_atNodeLocal[alPathResult[lCnt]].tCoordinate.eX;
                a_atPathInfo[lCnt].tSrc.eY = a_atNodeLocal[alPathResult[lCnt]].tCoordinate.eY;

                a_atPathInfo[lCnt].tDest.eX = a_atNodeLocal[alPathResult[lCnt+1]].tCoordinate.eX;
                a_atPathInfo[lCnt].tDest.eY = a_atNodeLocal[alPathResult[lCnt+1]].tCoordinate.eY;
            }
        }

        public static void FindPathNode2NextRegion()
        {

        }

        public static NodeId FindPathofRegion()
        {
            NodeId tNodeId = new NodeId();

            return tNodeId;
        }

        /// <summary> Merge 2 Path </summary>
        /// <param name="a_lIndexS2D">[IN] Index source to Destination </param>
        /// <param name="a_lIndexS2C">[IN] Index source to current </param>
        /// <param name="a_lIndexC2D">[IN] Index current to Destination </param>
        /// <param name="a_alNodeListNum">[IN] node number in each path </param>
        /// <param name="a_lNodeNum">[IN] node number if current region </param>
        /// <param name="a_alNodeListTmp">[INOUT] tmp node list of wanted path </param>
        static void MergePath(int a_lIndexS2D, int a_lIndexS2C, int a_lIndexC2D, int[] a_alNodeListNum, int a_lNodeNum, ref int[][] a_alNodeListTmp)
        {
            int lCntTmp = 0, lCntNodeList = 0;
            int lNodeListLimit = 0;
            int lNodeNumS2C = 0, lNodeNumC2D = 0, lNodeNumS2D = 0;

            lNodeListLimit = a_lNodeNum;    // 路徑節點最大數目 = 節點數
            lNodeNumS2C = a_alNodeListNum[a_lIndexS2C];
            lNodeNumC2D = a_alNodeListNum[a_lIndexC2D];
            lNodeNumS2D = a_alNodeListNum[a_lIndexS2D];
       
            // 將 Src to Mid 中的List 放進 Path
            for (lCntTmp = 0; lCntTmp < lNodeNumS2C; lCntTmp++)
            {
                a_alNodeListTmp[a_lIndexS2D][lCntTmp] = a_alNodeListTmp[a_lIndexS2C][lCntTmp];
            }

            // 將 Mid to Dest 中的List 放進 Path 但Mid要扣除 >>　lCntNodeList = 1 開始 是為了 扣除中介點
            for (lCntTmp = lNodeNumS2C, lCntNodeList = 1; lCntTmp < lNodeNumS2C + lNodeNumC2D -1; lCntTmp++, lCntNodeList++)
            {
                if (lCntTmp > lNodeListLimit)
                {   // lCntTmp 要小於上限 >> lCntTmp 頂多從 0 ~ lNodeListLimit-1
                    continue; // error
                }
                else
                {
                    a_alNodeListTmp[a_lIndexS2D][lCntTmp] = a_alNodeListTmp[a_lIndexC2D][lCntNodeList];
                }
            }

            // 將 Path list 剩下的元素設為 EMPTY_DATA
            for (lCntTmp = lNodeNumS2C + lNodeNumC2D - 1; lCntTmp < lNodeListLimit; lCntTmp++)
            {
                a_alNodeListTmp[a_lIndexS2D][lCntTmp] = EMPTY_DATA;
            }
        }

        /// <summary> Find shortest PATH </summary>
        /// <param name="a_lSrc">[IN] source node </param>
        /// <param name="a_lDst">[IN] Destination node </param>
        /// <param name="a_alMapCurrent">[IN] look up table of node in current region </param>
        /// <param name="a_lNodeNum">[IN] node number in current region </param>
        /// <param name="a_lPathLength">[INOUT] wanted path size </param>
        /// <returns> node list </returns>
        public static int[] FindPathofNode(int a_lSrc, int a_lDst, int[] a_alMapCurrent, int a_lNodeNum, ref int a_lPathLength)
        {
            int[] alPath = new int[0];              // 存路徑節點用
            int[] alDisMapTmp = new int[0];         // 暫存的地圖 >> 不修改輸入的地圖
            int[][] alNodeListTmp = new int[0][];   // 存所有路線最短路徑經過的節點
            int[] alNodeListNum = new int[0];       // 存所有路線的節點個數
            int lCntTmp = 0, lCntMid = 0, lCntSrc = 0, lCntDest = 0, lIndexS2D = 0, lIndexS2C = 0, lIndexC2D = 0;
            int lCntTmp_1 = 0, lCntTmp_2 = 0;
            int lNodeListLimit = 0;

            lNodeListLimit = a_lNodeNum ;    // 路徑節點最大數目 = 節點數

            if (a_lSrc == a_lDst)
            {   // 起終點相同 >> 直接回傳當下節點
                a_lPathLength = 1;
                alPath = new int[1];
                alPath[0] = a_lSrc;
                return alPath;
            }

            // Clone Map to tmp buffer
            alDisMapTmp = (int[])a_alMapCurrent.Clone();

            // create path's node list of map
            alNodeListTmp = new int[alDisMapTmp.Length][];
            for(lCntTmp = 0; lCntTmp < alDisMapTmp.Length; lCntTmp++)
            {
                alNodeListTmp[lCntTmp] = new int[lNodeListLimit]; // 宣告最大節點數
                lCntTmp_1 = lCntTmp / a_lNodeNum;   // y
                lCntTmp_2 = lCntTmp % a_lNodeNum;   // x
                alNodeListTmp[lCntTmp][0] = lCntTmp_1;
                alNodeListTmp[lCntTmp][1] = lCntTmp_2;
            }

            // create array for node list number
            alNodeListNum = new int[alDisMapTmp.Length];
            for (lCntTmp = 0; lCntTmp < alNodeListNum.Length; lCntTmp++)
            {   // 初始化每條路徑經過結點數是2 >>　因為一條線至少兩個點
                alNodeListNum[lCntTmp] = 2;
            }

            // Update buffer map
            for (lCntMid = 0; lCntMid < a_lNodeNum; lCntMid++)
            {
                for (lCntSrc = 0; lCntSrc < a_lNodeNum; lCntSrc++)
                {
                    for (lCntDest = 0; lCntDest < a_lNodeNum; lCntDest++)
                    {
                        lIndexS2D = lCntSrc * a_lNodeNum + lCntDest;
                        lIndexS2C = lCntSrc * a_lNodeNum + lCntMid;
                        lIndexC2D = lCntMid * a_lNodeNum + lCntDest;
                        if ((alDisMapTmp[lIndexS2C] + alDisMapTmp[lIndexC2D]) < alDisMapTmp[lIndexS2D])
                        {
                            // 更新路徑距離
                            alDisMapTmp[lIndexS2D] = alDisMapTmp[lIndexS2C] + alDisMapTmp[lIndexC2D];

                            // 合併兩路徑
                            MergePath(lIndexS2D, lIndexS2C, lIndexC2D, alNodeListNum, a_lNodeNum, ref alNodeListTmp);

                            //  更新路徑經過的節點數
                            alNodeListNum[lIndexS2D] = alNodeListNum[lIndexS2C] + alNodeListNum[lIndexC2D] - 1; // -1 是因為中介點重複算到
                        }
                    }
                }
            }

#if rtAGV_DEBUG_PRINT_TABLE
            Test_OutputTable("./FinalTable.txt", alDisMapTmp);
#endif

            lIndexS2D = a_lSrc * a_lNodeNum + a_lDst;
            a_lPathLength = alNodeListNum[lIndexS2D];
            alPath = new int[a_lPathLength];

            // Output list of path to alPath
            for (lCntTmp = 0; lCntTmp < a_lPathLength; lCntTmp++)
            {
                alPath[lCntTmp] = alNodeListTmp[lIndexS2D][lCntTmp];
            }

            return alPath;
        }

        public static void Test_FindPathofNode()
        {
          /*  int[] TableArray = new int[] 
            {   0, 2, EMPTY_DATA, 1, EMPTY_DATA ,
                2, 0, 3, 2, EMPTY_DATA ,
                EMPTY_DATA, 3, 0, EMPTY_DATA, 1 ,
                EMPTY_DATA, 2, EMPTY_DATA, 0, 1 ,
                EMPTY_DATA, EMPTY_DATA, 1, 1, 0
            };*/

            int[] TableArray = new int[] 
            {   0, 4200, 12280, EMPTY_DATA, EMPTY_DATA ,
                4200, 0, 8680, EMPTY_DATA, 755 ,
                12280, 8680, 0, 755, EMPTY_DATA ,
                EMPTY_DATA, EMPTY_DATA, 755, 0, EMPTY_DATA ,
                EMPTY_DATA, 755, EMPTY_DATA, EMPTY_DATA, 0
            };

            int lNodeNum = 0, lSrc = 0, lDst = 0, lPathLength = 0, lCnt = 0;
            int[] alPathResult = new int[0];

            Test_OutputTable("./InitTable.txt", TableArray);

            lNodeNum = 5;
            lSrc = 3;
            lDst = 4;

            alPathResult = FindPathofNode(lSrc, lDst, TableArray, lNodeNum, ref lPathLength);

            Console.Out.NewLine = "\n";
            for (lCnt = 0; lCnt< alPathResult.Length; lCnt++)
            {
                Console.WriteLine(alPathResult[lCnt] + ">>");
            }

            ////
            Test_OutputPath("./Result.txt", alPathResult);
            ////

        }

        public static void Test_OutputPath(string a_sFile, int[] a_alPath)
        {
            int lCnt = 0;

            //  文件寫入
            FileStream fs = new FileStream(a_sFile, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            //  開始寫入
            // 
            for (lCnt = 0; lCnt < a_alPath.Length; lCnt++)
            {
                if (lCnt == a_alPath.Length - 1)
                {
                    sw.Write(a_alPath[lCnt] + "\n");
                }
                else
                {
                    sw.Write(a_alPath[lCnt] + " << ");
                }
            }

            //清空暫存
            sw.Flush();

            //關閉檔案
            sw.Close();
            fs.Close();
            ////
        }

        public static void Test_OutputTable(string a_sFile, int[] a_alTable)
        {
            int lNodeNum = 0, lCnt = 0;

            lNodeNum = (int)Math.Sqrt(a_alTable.Length);


            //  文件寫入
            FileStream fs = new FileStream(a_sFile, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            //  開始寫入
            // 
            for (lCnt = 0; lCnt < a_alTable.Length; lCnt++)
            {
                if(lCnt % lNodeNum == lNodeNum-1)
                {
                    sw.Write(a_alTable[lCnt] + "\n");
                }
                else
                {
                    sw.Write(a_alTable[lCnt] + "\t\t");
                }
            }

            //清空暫存
            sw.Flush();

            //關閉檔案
            sw.Close();
            fs.Close();
            ////
        }

        public static void LoadInitTable(string TableFileName, ref int[] TableArray)
        {
            //讀取權重檔並寫入陣列
            string[] lines = System.IO.File.ReadAllLines(TableFileName);
            int PointsCount = lines.Length;
            TableArray = new int[PointsCount * PointsCount];
            int Count = 0;
            foreach (string line in lines)
            {
                string[] EachWeight = line.Split('\t');
                for (int i = 0; i < EachWeight.Length; i++)
                {
                    if (EachWeight[i] == "") continue;
                    TableArray[Count] = Convert.ToInt32(EachWeight[i]);
                    Count++;
                }
            }
        }

        public static void LoadAllPoints(string PointsFileName, ref rtVector[] PointsArray)
        {
            //讀取節點並寫入陣列
            string[] lines = System.IO.File.ReadAllLines(PointsFileName);
            int PointsCount = lines.Length;
            PointsArray = new rtVector[PointsCount];
            int Count = 0;
            foreach (string line in lines)
            {
                string[] EachPoint = line.Split(',');
                for (int i = 0; i < EachPoint.Length; i++)
                {
                    if (EachPoint[i] == "") continue;
                    if (EachPoint.Length == 2)
                    {
                        PointsArray[Count].eX = Convert.ToInt32(EachPoint[0]);
                        PointsArray[Count].eY = Convert.ToInt32(EachPoint[1]);
                    }
                }
                Count++;
            }
        }

        public static void CreateWeightingTable(string PointsFileName, string OutTableFileName)
        {
            string[] lines = System.IO.File.ReadAllLines(PointsFileName);
            int PointsCount = lines.Length;
            int[] TableArray = new int[PointsCount * PointsCount];
            rtVector[] AllPoint = new rtVector[PointsCount];
            int Count = 0;

            //從檔案讀取所有座標點資料
            foreach (string line in lines)
            {
                string[] EachPoint = line.Split(',');
                AllPoint[Count].eX = Convert.ToInt32(EachPoint[0]);
                AllPoint[Count].eY = Convert.ToInt32(EachPoint[1]);
                Count++;
            }

            //計算所有點之間的距離
            for (int i = 0; i < PointsCount; i++)
            {
                for (int j = i; j < PointsCount; j++)
                {
                    //自己
                    if (i == j) TableArray[i * PointsCount + j] = 0;
                    else
                    {
                        int Distance = (int)rtVectorOP_2D.GetDistance(AllPoint[i], AllPoint[j]);

                        TableArray[i * PointsCount + j] = Distance;
                        TableArray[j * PointsCount + i] = Distance;
                    }
                }
            }

            //  文件寫入
            FileStream fs = new FileStream(OutTableFileName, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            //  開始寫入
            // 
            int lCnt;
            for (lCnt = 0; lCnt < TableArray.Length; lCnt++)
            {
                if (lCnt % PointsCount == PointsCount - 1)
                {
                    sw.Write(TableArray[lCnt] + "\n");
                }
                else
                {
                    sw.Write(TableArray[lCnt] + "\t\t");
                }
            }

            //清空暫存
            sw.Flush();

            //關閉檔案
            sw.Close();
            fs.Close();
            ////
        }
    }
}