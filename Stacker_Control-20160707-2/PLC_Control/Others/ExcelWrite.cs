using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Excel;


namespace Others
{
    public class ExcelWrite
    {
        public static void WriteLog(string FileName, List<string> LogCarInfo, List<string> LogMainData, List<string> LogDetailData)
        {
            // 設定儲存檔名，不用設定副檔名，系統自動判斷 excel 版本，產生 .xls 或 .xlsx 副檔名
            string pathFile = FileName;

            Excel.Application excelApp;
            Excel._Workbook wBook;
            Excel._Worksheet wSheet;
            Excel.Range wRange;

            // 開啟一個新的應用程式
            excelApp = new Excel.Application();

            // 讓Excel文件可見
            excelApp.Visible = true;

            // 停用警告訊息
            excelApp.DisplayAlerts = false;

            // 加入新的活頁簿
            excelApp.Workbooks.Add(Type.Missing);

            // 引用第一個活頁簿
            wBook = excelApp.Workbooks[1];

            // 設定活頁簿焦點
            wBook.Activate();

           /* try
            {*/
                // 引用第一個工作表
                wSheet = (Excel._Worksheet)wBook.Worksheets[1];

                // 命名工作表的名稱
                wSheet.Name = "CarInfo";

                // 設定工作表焦點
                wSheet.Activate();

                excelApp.Cells[1, 1] = "Excel測試";

                excelApp.Cells[1, 1] = "Direction";
                excelApp.Cells[1, 2] = "WheelAngle";
                excelApp.Cells[1, 3] = "Pos_X";
                excelApp.Cells[1, 4] = "Pos_Y";
                excelApp.Cells[1, 5] = "TireR_X";
                excelApp.Cells[1, 6] = "TireR_Y";
                excelApp.Cells[1, 7] = "TireL_X";
                excelApp.Cells[1, 8] = "TireL_Y";
                excelApp.Cells[1, 9] = "Motor_X";
                excelApp.Cells[1, 10] = "Motor_Y";
                excelApp.Cells[1, 11] = "SpeedL";
                excelApp.Cells[1, 12] = "SpeedR";

                // 設定第1列顏色
                wRange = wSheet.Range[wSheet.Cells[1, 1], wSheet.Cells[1, 12]];
                wRange.Select();
                wRange.Font.Color = ColorTranslator.ToOle(Color.White);
                wRange.Interior.Color = ColorTranslator.ToOle(Color.DimGray);

                //寫入Main資料
                for (int i = 0; i < LogCarInfo.Count; i++)
                {
                    string EachLine = LogCarInfo[i];
                    string[] EachData = EachLine.Split(',');
                    for (int j = 0; j < EachData.Length; j++)excelApp.Cells[i+2, j+1] = EachData[j];
                }

                //置中與自動欄寬
                wRange = wSheet.Range[wSheet.Cells[1, 1], wSheet.Cells[LogCarInfo.Count + 1, 12]];
                wRange.Select();
                wRange.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
                wRange.Columns.AutoFit();

                /////////////////////////////////////////////////////
                // 引用第二個工作表
                wSheet = (Excel._Worksheet)wBook.Worksheets[2]; 

                // 命名工作表的名稱
                wSheet.Name = "Main_Data";

                // 設定工作表焦點
                wSheet.Activate();

                excelApp.Cells[1, 1] = "Main_Data";

                // 設定第1列資料

                excelApp.Cells[1, 1] = "AGV_Status";
                excelApp.Cells[1, 2] = "ForkStatus";
                excelApp.Cells[1, 3] = "ErrorAngle";
                excelApp.Cells[1, 4] = "ErrorPower";
                excelApp.Cells[1, 5] = "MotorAngle";
                excelApp.Cells[1, 6] = "MotorPower";
                excelApp.Cells[1, 7] = "FinishFlag";
                excelApp.Cells[1, 8] = "PathIndex";
                excelApp.Cells[1, 9] = "PathStatus";
                excelApp.Cells[1, 10] = "PathSrc";
                excelApp.Cells[1, 11] = "PathDest";
                excelApp.Cells[1, 12] = "DisTime";
                excelApp.Cells[1, 13] = "TurnType";

                // 設定第1列顏色
                wRange = wSheet.Range[wSheet.Cells[1, 1], wSheet.Cells[1, 13]];
                wRange.Select();
                wRange.Font.Color = ColorTranslator.ToOle(Color.White);
                wRange.Interior.Color = ColorTranslator.ToOle(Color.DimGray);

                //寫入Detail資料
                for (int i = 0; i < LogMainData.Count; i++)
                {
                    string EachLine = LogMainData[i];
                    string[] EachData = EachLine.Split(',');
                    for (int j = 0; j < EachData.Length; j++) excelApp.Cells[i + 2, j + 1] = EachData[j];
                }

                //置中與自動欄寬
                wRange = wSheet.Range[wSheet.Cells[1, 1], wSheet.Cells[LogMainData.Count + 1, 13]];
                wRange.Select();
                wRange.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
                wRange.Columns.AutoFit();

                if (GlobalVar.isPredictLog)
                {
                    // 引用第二個工作表
                    wSheet = (Excel._Worksheet)wBook.Worksheets[3];

                    // 命名工作表的名稱
                    wSheet.Name = "Detail_Data";

                    // 設定工作表焦點
                    wSheet.Activate();

                    excelApp.Cells[1, 1] = "Excel測試";

                    // 設定第1列資料
                    excelApp.Cells[1, 1] = "eThetaError";
                    excelApp.Cells[1, 2] = "TargetAngleOffset1";
                    excelApp.Cells[1, 3] = "TargetAngleOffset2";
                    excelApp.Cells[1, 4] = "CenterSpeed";
                    excelApp.Cells[1, 5] = "eDeltaCarAngle";
                    excelApp.Cells[1, 6] = "Car-Debug_bOverDestFlag";
                    excelApp.Cells[1, 7] = "MotorData-BackFlag";
                    excelApp.Cells[1, 8] = "NavigateOffset";

                    // 設定第1列顏色
                    wRange = wSheet.Range[wSheet.Cells[1, 1], wSheet.Cells[1, 8]];
                    wRange.Select();
                    wRange.Font.Color = ColorTranslator.ToOle(Color.White);
                    wRange.Interior.Color = ColorTranslator.ToOle(Color.DimGray);

                    //寫入Detail資料
                    for (int i = 0; i < LogDetailData.Count; i++)
                    {
                        string EachLine = LogDetailData[i];
                        string[] EachData = EachLine.Split(',');
                        for (int j = 0; j < EachData.Length; j++)excelApp.Cells[i + 2, j + 1] = EachData[j];
                    }

                    //置中與自動欄寬
                    wRange = wSheet.Range[wSheet.Cells[1, 1], wSheet.Cells[LogDetailData.Count + 1, 8]];
                    wRange.Select();
                    wRange.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    wRange.Columns.AutoFit();
                }
                try
                {
                    //另存活頁簿
                    wBook.SaveAs(pathFile, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    Console.WriteLine("儲存文件於 " + Environment.NewLine + pathFile);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("儲存檔案出錯，檔案可能正在使用" + Environment.NewLine + ex.Message);
                }
            //}
           /* catch (Exception ex)
            {
                Console.WriteLine("產生報表時出錯！" + Environment.NewLine + ex.Message);
            }*/

            //關閉活頁簿
            wBook.Close(false, Type.Missing, Type.Missing);

            //關閉Excel
            excelApp.Quit();

            //釋放Excel資源
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            wBook = null;
            wSheet = null;
            wRange = null;
            excelApp = null;
            GC.Collect();
            Console.Read();
        }

    }
}
