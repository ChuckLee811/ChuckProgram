using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//使用演算法須宣告
using AlgorithmTool;

namespace PLC_Control
{
    public partial class Text_Debug : UserControl
    {
        public Panel ShowTextDebugPanel;

        /** \brief是否要開預測紀錄*/
        public static rtAGV_Control DeliverData;
        
        public Text_Debug()
        {
            InitializeComponent();
            textBox_PositionX.Text = "2383";
            textBox_PositionY.Text = "-1686";
            textBox_Angle.Text = "1";
            textBox_CarTireSpeedLeft.Text = "0";
            textBox_CarTireSpeedRight.Text = "0";
            textBox_CarTirepositionR_X.Text = "2393";
            textBox_CarTirepositionR_Y.Text = "-2285";
            textBox_CarTirepositionL_X.Text = "2373";
            textBox_CarTirepositionL_Y.Text = "-1087";
            textBox_MotorPosition_X.Text = "884";
            textBox_MotorPosition_Y.Text = "-1712";
            textBox_WheelAngle.Text = "-0.308823529411765";
            textBox_TurnType.Text = "2";
            textBox_Src_X.Text = "2250";
            textBox_Src_Y.Text = "-1550";
            textBox_Dest_X.Text = "17080";
            textBox_Dest_Y.Text = "-1550";
        }
        
        private void btnTextBack_Click(object sender, EventArgs e)
        {
            ShowTextDebugPanel.Visible = false;
        }

        private void btnCalculationResults_Click(object sender, EventArgs e)
        {
            if (textBox_PositionX.Text == "" || textBox_PositionY.Text == "" || textBox_Angle.Text == ""
             || textBox_CarTireSpeedLeft.Text == "" || textBox_CarTireSpeedRight.Text == "" || textBox_CarTirepositionR_X.Text == ""
             || textBox_CarTirepositionR_Y.Text == "" || textBox_CarTirepositionL_X.Text == "" || textBox_CarTirepositionL_Y.Text == ""
             || textBox_MotorPosition_X.Text == "" || textBox_MotorPosition_Y.Text == "" || textBox_WheelAngle.Text == ""
             || textBox_Src_X.Text == "" || textBox_Src_Y.Text == "" || textBox_Dest_X.Text == "" || textBox_Dest_Y.Text == "")
            {
                MessageBox.Show("請輸入數值");
                return;
            }
            if (textBox_PositionX.Text == "")
            {
                return;
            }

            //int Select_Index = 1;
            DeliverData = new rtAGV_Control();
            dataGridView_Text.Rows.Clear();
            //更新車體資訊
            DeliverData.tAGV_Data.tCarInfo.tPosition.eX = Convert.ToDouble(textBox_PositionX.Text);
            DeliverData.tAGV_Data.tCarInfo.tPosition.eY = Convert.ToDouble(textBox_PositionY.Text);
            DeliverData.tAGV_Data.tCarInfo.eAngle = Convert.ToDouble(textBox_Angle.Text);

            DeliverData.tAGV_Data.tCarInfo.eCarTireSpeedLeft = Convert.ToDouble(textBox_CarTireSpeedLeft.Text);
            DeliverData.tAGV_Data.tCarInfo.eCarTireSpeedRight = Convert.ToDouble(textBox_CarTireSpeedRight.Text);

            DeliverData.tAGV_Data.tCarInfo.tCarTirepositionR.eX = Convert.ToDouble(textBox_CarTirepositionR_X.Text);
            DeliverData.tAGV_Data.tCarInfo.tCarTirepositionR.eY = Convert.ToDouble(textBox_CarTirepositionR_Y.Text);
            DeliverData.tAGV_Data.tCarInfo.tCarTirepositionL.eX = Convert.ToDouble(textBox_CarTirepositionL_X.Text);
            DeliverData.tAGV_Data.tCarInfo.tCarTirepositionL.eY = Convert.ToDouble(textBox_CarTirepositionL_Y.Text);
            DeliverData.tAGV_Data.tCarInfo.tMotorPosition.eX = Convert.ToDouble(textBox_MotorPosition_X.Text);
            DeliverData.tAGV_Data.tCarInfo.tMotorPosition.eY = Convert.ToDouble(textBox_MotorPosition_Y.Text);

            //更新車Sensor資訊
            DeliverData.tAGV_Data.tCarInfo.eWheelAngle = Convert.ToDouble(textBox_WheelAngle.Text);

            //取得Path資訊
            DeliverData.tAGV_Data.atPathInfo = new rtPath_Info[1];
            DeliverData.tAGV_Data.atPathInfo[0].tSrc.eX = Convert.ToDouble(textBox_Src_X.Text);
            DeliverData.tAGV_Data.atPathInfo[0].tSrc.eY = Convert.ToDouble(textBox_Src_Y.Text);
            DeliverData.tAGV_Data.atPathInfo[0].tDest.eX = Convert.ToDouble(textBox_Dest_X.Text);
            DeliverData.tAGV_Data.atPathInfo[0].tDest.eY = Convert.ToDouble(textBox_Dest_Y.Text);
            DeliverData.tAGV_Data.atPathInfo[0].ucTurnType = Convert.ToByte(textBox_TurnType.Text);
            DeliverData.tAGV_Data.atPathInfo[0].ucStatus = 1;

            object[] obj = new object[11] { DeliverData.tAGV_Data.tCarInfo.tPosition.eX,  DeliverData.tAGV_Data.tCarInfo.tPosition.eY, 
                        DeliverData.tAGV_Data.tCarInfo.eAngle,   DeliverData.tAGV_Data.tCarInfo.tCarTirepositionR.eX,   DeliverData.tAGV_Data.tCarInfo.tCarTirepositionR.eY, 
                        DeliverData.tAGV_Data.tCarInfo.tCarTirepositionL.eX,  DeliverData.tAGV_Data.tCarInfo.tCarTirepositionL.eY,  DeliverData.tAGV_Data.tCarInfo.eCarTireSpeedLeft,
             DeliverData.tAGV_Data.tCarInfo.eCarTireSpeedRight,  DeliverData.tAGV_Data.atPathInfo[0].tSrc.eX+"/"+ DeliverData.tAGV_Data.atPathInfo[0].tSrc.eY, 
             DeliverData.tAGV_Data.atPathInfo[0].tDest.eX + "/" +  DeliverData.tAGV_Data.atPathInfo[0].tDest.eY};

            DataGridViewRow dgvr = new DataGridViewRow();
            dgvr.CreateCells(dataGridView_Text, obj);
            dgvr.Height = 35;
            dgvr.DefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView_Text.Rows.Add(dgvr);

            DeliverData.rtAGV_MotorCtrl(ref DeliverData.tAGV_Data.atPathInfo, 0, true);

            //Console.WriteLine("Power:" + DeliverData.tAGV_Data.CMotor.tMotorData.lMotorPower.ToString());
            //Console.WriteLine("lMotorAngle:" + DeliverData.tAGV_Data.CMotor.tMotorData.lMotorAngle.ToString());

            label_TextPower.Text = "Power：" + DeliverData.tAGV_Data.CMotor.tMotorData.lMotorPower.ToString();
            label_TextMotorAngle.Text = "MotorAngle：" + DeliverData.tAGV_Data.CMotor.tMotorData.lMotorAngle.ToString();
        }
    }
}
