namespace PLC_Control
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            this.btnSendR = new System.Windows.Forms.Button();
            this.btnSendL = new System.Windows.Forms.Button();
            this.btnDirectionBack = new System.Windows.Forms.Button();
            this.btnDirectionFront = new System.Windows.Forms.Button();
            this.btnAccelerator = new System.Windows.Forms.Button();
            this.timer_Rotation = new System.Windows.Forms.Timer(this.components);
            this.timer_system = new System.Windows.Forms.Timer(this.components);
            this.btnOrigin = new System.Windows.Forms.Button();
            this.btnCANMode = new System.Windows.Forms.Button();
            this.btnEmergencyStop = new System.Windows.Forms.Button();
            this.btnMoveFront = new System.Windows.Forms.Button();
            this.brnMoveBack = new System.Windows.Forms.Button();
            this.btnRelaxMotor = new System.Windows.Forms.Button();
            this.btnSetTimeStamp = new System.Windows.Forms.Button();
            this.btnClibratOrigin = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridTotalTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridAvearageTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnDoCmd = new System.Windows.Forms.Button();
            this.lblDstPosition = new System.Windows.Forms.Label();
            this.lblDstRegion = new System.Windows.Forms.Label();
            this.lblSrcPosition = new System.Windows.Forms.Label();
            this.lblSrcRegion = new System.Windows.Forms.Label();
            this.lblAgvID = new System.Windows.Forms.Label();
            this.txtDstPosition = new System.Windows.Forms.TextBox();
            this.txtDstRegion = new System.Windows.Forms.TextBox();
            this.txtSrcPosition = new System.Windows.Forms.TextBox();
            this.txtSrcRegion = new System.Windows.Forms.TextBox();
            this.txtAgvID = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtReceiveServer = new System.Windows.Forms.TextBox();
            this.btnStartServer = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnContinue = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.lbNAV_IP = new System.Windows.Forms.Label();
            this.btn_Mode_Navigation = new System.Windows.Forms.Button();
            this.btnNAVDisConnect = new System.Windows.Forms.Button();
            this.txtNAVIP = new System.Windows.Forms.TextBox();
            this.btnNAVConnect = new System.Windows.Forms.Button();
            this.txtNAVPort = new System.Windows.Forms.TextBox();
            this.lbNAVPort = new System.Windows.Forms.Label();
            this.btnContinueLocation = new System.Windows.Forms.Button();
            this.txtNavReceive = new System.Windows.Forms.TextBox();
            this.tabLidar3 = new System.Windows.Forms.TabPage();
            this.OpenGLCtrl = new SharpGL.OpenGLControl();
            this.btnOpenLidarFile = new System.Windows.Forms.Button();
            this.btnLidarDataOut = new System.Windows.Forms.Button();
            this.btnLidarRecode = new System.Windows.Forms.Button();
            this.btnResetView = new System.Windows.Forms.Button();
            this.btnStartLidar = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtFrontBackValue = new System.Windows.Forms.TextBox();
            this.btnSetForkLeftRight = new System.Windows.Forms.Button();
            this.btnDoFrontBack = new System.Windows.Forms.Button();
            this.txtForkValue = new System.Windows.Forms.TextBox();
            this.txtRotationValue = new System.Windows.Forms.TextBox();
            this.btnSetHeight = new System.Windows.Forms.Button();
            this.txtSetHeight = new System.Windows.Forms.TextBox();
            this.btnRotation = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.btnSetUserLevel = new System.Windows.Forms.Button();
            this.btn_Mode_LandMark = new System.Windows.Forms.Button();
            this.btn_Mode_Mapping = new System.Windows.Forms.Button();
            this.btnSaveTxt = new System.Windows.Forms.Button();
            this.btn_Mode_Standby = new System.Windows.Forms.Button();
            this.btn_Mode_PowerDown = new System.Windows.Forms.Button();
            this.btnArduino = new System.Windows.Forms.Button();
            this.txtArduinoPortNumber = new System.Windows.Forms.TextBox();
            this.lblArduinoPort = new System.Windows.Forms.Label();
            this.lblArduinoBaudRate = new System.Windows.Forms.Label();
            this.btnClearList = new System.Windows.Forms.Button();
            this.pictureBox_Angle = new System.Windows.Forms.PictureBox();
            this.txtArduinoBaudRate = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtWPortNumber = new System.Windows.Forms.TextBox();
            this.lblWPortNumber = new System.Windows.Forms.Label();
            this.lblWBaudRate = new System.Windows.Forms.Label();
            this.txtWBaudRate = new System.Windows.Forms.TextBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnOpenCan = new System.Windows.Forms.Button();
            this.btnCanConnect = new System.Windows.Forms.Button();
            this.btnBrakes = new System.Windows.Forms.Button();
            this.btnClutch = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnSloping = new System.Windows.Forms.Button();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnOblique = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnFront = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.label_Path = new System.Windows.Forms.Label();
            this.label_PLCConnect = new System.Windows.Forms.Label();
            this.btnDisConnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelCANStatus = new System.Windows.Forms.Label();
            this.axDBCommManager_Detector = new AxDATABUILDERAXLibLB.AxDBCommManager();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.menuStrip_System = new System.Windows.Forms.MenuStrip();
            this.功能ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.主要控制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.離線除錯ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.log檔除錯ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.文字輸入除錯ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.說明ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.關於ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelSetLocation = new System.Windows.Forms.Panel();
            this.panelDebug = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.tabPage4.SuspendLayout();
            this.groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox7.SuspendLayout();
            this.tabLidar3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OpenGLCtrl)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Angle)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axDBCommManager_Detector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.tabControl.SuspendLayout();
            this.menuStrip_System.SuspendLayout();
            this.SuspendLayout();
            // 
            // monthCalendar1
            // 
            this.monthCalendar1.Location = new System.Drawing.Point(0, 0);
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.TabIndex = 0;
            // 
            // btnSendR
            // 
            this.btnSendR.Location = new System.Drawing.Point(627, 51);
            this.btnSendR.Name = "btnSendR";
            this.btnSendR.Size = new System.Drawing.Size(68, 36);
            this.btnSendR.TabIndex = 3;
            this.btnSendR.Text = " 逆時針";
            this.btnSendR.UseVisualStyleBackColor = true;
            this.btnSendR.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnSendR_MouseDown);
            this.btnSendR.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnSendR_MouseUp);
            // 
            // btnSendL
            // 
            this.btnSendL.Location = new System.Drawing.Point(706, 51);
            this.btnSendL.Name = "btnSendL";
            this.btnSendL.Size = new System.Drawing.Size(68, 36);
            this.btnSendL.TabIndex = 4;
            this.btnSendL.Text = "順時針";
            this.btnSendL.UseVisualStyleBackColor = true;
            this.btnSendL.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnSendL_MouseDown);
            this.btnSendL.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnSendL_MouseUp);
            // 
            // btnDirectionBack
            // 
            this.btnDirectionBack.Location = new System.Drawing.Point(795, 113);
            this.btnDirectionBack.Margin = new System.Windows.Forms.Padding(2);
            this.btnDirectionBack.Name = "btnDirectionBack";
            this.btnDirectionBack.Size = new System.Drawing.Size(70, 33);
            this.btnDirectionBack.TabIndex = 15;
            this.btnDirectionBack.Text = "後";
            this.btnDirectionBack.UseVisualStyleBackColor = true;
            this.btnDirectionBack.Click += new System.EventHandler(this.btnDirectionBack_Click);
            // 
            // btnDirectionFront
            // 
            this.btnDirectionFront.Location = new System.Drawing.Point(795, 71);
            this.btnDirectionFront.Margin = new System.Windows.Forms.Padding(2);
            this.btnDirectionFront.Name = "btnDirectionFront";
            this.btnDirectionFront.Size = new System.Drawing.Size(70, 34);
            this.btnDirectionFront.TabIndex = 15;
            this.btnDirectionFront.Text = "前";
            this.btnDirectionFront.UseVisualStyleBackColor = true;
            this.btnDirectionFront.Click += new System.EventHandler(this.btnDirectionFront_Click);
            // 
            // btnAccelerator
            // 
            this.btnAccelerator.Location = new System.Drawing.Point(794, 28);
            this.btnAccelerator.Margin = new System.Windows.Forms.Padding(2);
            this.btnAccelerator.Name = "btnAccelerator";
            this.btnAccelerator.Size = new System.Drawing.Size(71, 34);
            this.btnAccelerator.TabIndex = 4;
            this.btnAccelerator.Text = "油門";
            this.btnAccelerator.UseVisualStyleBackColor = true;
            this.btnAccelerator.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAccelerator_MouseDown);
            this.btnAccelerator.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAccelerator_MouseUp);
            // 
            // timer_Rotation
            // 
            this.timer_Rotation.Enabled = true;
            this.timer_Rotation.Interval = 150;
            this.timer_Rotation.Tick += new System.EventHandler(this.timer_Rotation_Tick);
            // 
            // timer_system
            // 
            this.timer_system.Interval = 1000;
            this.timer_system.Tick += new System.EventHandler(this.timer_system_Tick);
            // 
            // btnOrigin
            // 
            this.btnOrigin.Location = new System.Drawing.Point(627, 94);
            this.btnOrigin.Name = "btnOrigin";
            this.btnOrigin.Size = new System.Drawing.Size(68, 36);
            this.btnOrigin.TabIndex = 9;
            this.btnOrigin.Text = "原點復歸";
            this.btnOrigin.UseVisualStyleBackColor = true;
            this.btnOrigin.Click += new System.EventHandler(this.btnOrigin_Click);
            // 
            // btnCANMode
            // 
            this.btnCANMode.Location = new System.Drawing.Point(796, 170);
            this.btnCANMode.Name = "btnCANMode";
            this.btnCANMode.Size = new System.Drawing.Size(69, 56);
            this.btnCANMode.TabIndex = 10;
            this.btnCANMode.Text = "手動模式";
            this.btnCANMode.UseVisualStyleBackColor = true;
            this.btnCANMode.Click += new System.EventHandler(this.btnCANMode_Click);
            // 
            // btnEmergencyStop
            // 
            this.btnEmergencyStop.Location = new System.Drawing.Point(627, 247);
            this.btnEmergencyStop.Name = "btnEmergencyStop";
            this.btnEmergencyStop.Size = new System.Drawing.Size(146, 179);
            this.btnEmergencyStop.TabIndex = 17;
            this.btnEmergencyStop.Text = "緊急停止";
            this.btnEmergencyStop.UseVisualStyleBackColor = true;
            this.btnEmergencyStop.Click += new System.EventHandler(this.btnEmergencyStop_Click);
            // 
            // btnMoveFront
            // 
            this.btnMoveFront.Location = new System.Drawing.Point(627, 135);
            this.btnMoveFront.Name = "btnMoveFront";
            this.btnMoveFront.Size = new System.Drawing.Size(67, 56);
            this.btnMoveFront.TabIndex = 18;
            this.btnMoveFront.Text = "前進";
            this.btnMoveFront.UseVisualStyleBackColor = true;
            this.btnMoveFront.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnMoveFront_MouseDown);
            this.btnMoveFront.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnMoveFront_MouseUp);
            // 
            // brnMoveBack
            // 
            this.brnMoveBack.Location = new System.Drawing.Point(706, 134);
            this.brnMoveBack.Name = "brnMoveBack";
            this.brnMoveBack.Size = new System.Drawing.Size(67, 56);
            this.brnMoveBack.TabIndex = 19;
            this.brnMoveBack.Text = "後退";
            this.brnMoveBack.UseVisualStyleBackColor = true;
            this.brnMoveBack.MouseDown += new System.Windows.Forms.MouseEventHandler(this.brnMoveBack_MouseDown);
            this.brnMoveBack.MouseUp += new System.Windows.Forms.MouseEventHandler(this.brnMoveBack_MouseUp);
            // 
            // btnRelaxMotor
            // 
            this.btnRelaxMotor.Location = new System.Drawing.Point(706, 93);
            this.btnRelaxMotor.Margin = new System.Windows.Forms.Padding(2);
            this.btnRelaxMotor.Name = "btnRelaxMotor";
            this.btnRelaxMotor.Size = new System.Drawing.Size(68, 36);
            this.btnRelaxMotor.TabIndex = 23;
            this.btnRelaxMotor.Text = "放掉馬達";
            this.btnRelaxMotor.UseVisualStyleBackColor = true;
            this.btnRelaxMotor.Click += new System.EventHandler(this.btnRelaxMotor_Click);
            // 
            // btnSetTimeStamp
            // 
            this.btnSetTimeStamp.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSetTimeStamp.Location = new System.Drawing.Point(796, 237);
            this.btnSetTimeStamp.Name = "btnSetTimeStamp";
            this.btnSetTimeStamp.Size = new System.Drawing.Size(69, 50);
            this.btnSetTimeStamp.TabIndex = 47;
            this.btnSetTimeStamp.Text = "開始TimeStamp";
            this.btnSetTimeStamp.UseVisualStyleBackColor = true;
            // 
            // btnClibratOrigin
            // 
            this.btnClibratOrigin.Location = new System.Drawing.Point(627, 197);
            this.btnClibratOrigin.Margin = new System.Windows.Forms.Padding(2);
            this.btnClibratOrigin.Name = "btnClibratOrigin";
            this.btnClibratOrigin.Size = new System.Drawing.Size(146, 45);
            this.btnClibratOrigin.TabIndex = 24;
            this.btnClibratOrigin.Text = "原點校正";
            this.btnClibratOrigin.UseVisualStyleBackColor = true;
            this.btnClibratOrigin.Click += new System.EventHandler(this.btnClibratOrigin_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.AllowUserToResizeRows = false;
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView2.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView2.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView2.ColumnHeadersHeight = 30;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.gridTotalTime,
            this.gridAvearageTotal,
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5});
            this.dataGridView2.Location = new System.Drawing.Point(10, 429);
            this.dataGridView2.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView2.MultiSelect = false;
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.RowHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.LightSalmon;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dataGridView2.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView2.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dataGridView2.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.Transparent;
            this.dataGridView2.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dataGridView2.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.HotPink;
            this.dataGridView2.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridView2.RowTemplate.Height = 50;
            this.dataGridView2.RowTemplate.ReadOnly = true;
            this.dataGridView2.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(763, 95);
            this.dataGridView2.TabIndex = 48;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.FillWeight = 60F;
            this.dataGridViewTextBoxColumn1.HeaderText = "X";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.FillWeight = 60F;
            this.dataGridViewTextBoxColumn2.HeaderText = "Y";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.FillWeight = 55F;
            this.dataGridViewTextBoxColumn3.HeaderText = "Angle";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // gridTotalTime
            // 
            this.gridTotalTime.FillWeight = 55F;
            this.gridTotalTime.HeaderText = "FHeight";
            this.gridTotalTime.Name = "gridTotalTime";
            this.gridTotalTime.ReadOnly = true;
            // 
            // gridAvearageTotal
            // 
            this.gridAvearageTotal.FillWeight = 70F;
            this.gridAvearageTotal.HeaderText = "AGVStatus";
            this.gridAvearageTotal.Name = "gridAvearageTotal";
            this.gridAvearageTotal.ReadOnly = true;
            // 
            // Column1
            // 
            this.Column1.FillWeight = 50F;
            this.Column1.HeaderText = "FStatus";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.FillWeight = 60F;
            this.Column2.HeaderText = "SpeedL";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.FillWeight = 60F;
            this.Column3.HeaderText = "SpeedR";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.FillWeight = 50F;
            this.Column4.HeaderText = "WAngle";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column5
            // 
            this.Column5.FillWeight = 50F;
            this.Column5.HeaderText = "Power";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.groupBox9);
            this.tabPage4.Controls.Add(this.groupBox7);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage4.Size = new System.Drawing.Size(600, 368);
            this.tabPage4.TabIndex = 6;
            this.tabPage4.Text = "NAV-350";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.button3);
            this.groupBox9.Controls.Add(this.button4);
            this.groupBox9.Controls.Add(this.button2);
            this.groupBox9.Controls.Add(this.button1);
            this.groupBox9.Controls.Add(this.btnDoCmd);
            this.groupBox9.Controls.Add(this.lblDstPosition);
            this.groupBox9.Controls.Add(this.lblDstRegion);
            this.groupBox9.Controls.Add(this.lblSrcPosition);
            this.groupBox9.Controls.Add(this.lblSrcRegion);
            this.groupBox9.Controls.Add(this.lblAgvID);
            this.groupBox9.Controls.Add(this.txtDstPosition);
            this.groupBox9.Controls.Add(this.txtDstRegion);
            this.groupBox9.Controls.Add(this.txtSrcPosition);
            this.groupBox9.Controls.Add(this.txtSrcRegion);
            this.groupBox9.Controls.Add(this.txtAgvID);
            this.groupBox9.Controls.Add(this.pictureBox1);
            this.groupBox9.Controls.Add(this.txtReceiveServer);
            this.groupBox9.Controls.Add(this.btnStartServer);
            this.groupBox9.Controls.Add(this.btnPause);
            this.groupBox9.Controls.Add(this.btnContinue);
            this.groupBox9.Location = new System.Drawing.Point(240, 10);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(355, 353);
            this.groupBox9.TabIndex = 28;
            this.groupBox9.TabStop = false;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(263, 293);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(80, 52);
            this.button3.TabIndex = 80;
            this.button3.Text = "RunDown4";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(263, 231);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(80, 52);
            this.button4.TabIndex = 79;
            this.button4.Text = "RunDown3";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click_1);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(166, 293);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 52);
            this.button2.TabIndex = 78;
            this.button2.Text = "RunDown2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(166, 231);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 52);
            this.button1.TabIndex = 77;
            this.button1.Text = "RunDown1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnDoCmd
            // 
            this.btnDoCmd.Location = new System.Drawing.Point(6, 180);
            this.btnDoCmd.Name = "btnDoCmd";
            this.btnDoCmd.Size = new System.Drawing.Size(65, 45);
            this.btnDoCmd.TabIndex = 76;
            this.btnDoCmd.Text = "執行";
            this.btnDoCmd.UseVisualStyleBackColor = true;
            this.btnDoCmd.Click += new System.EventHandler(this.btnDoCmd_Click);
            // 
            // lblDstPosition
            // 
            this.lblDstPosition.AutoSize = true;
            this.lblDstPosition.Location = new System.Drawing.Point(88, 187);
            this.lblDstPosition.Name = "lblDstPosition";
            this.lblDstPosition.Size = new System.Drawing.Size(57, 12);
            this.lblDstPosition.TabIndex = 75;
            this.lblDstPosition.Text = "DstPosition";
            // 
            // lblDstRegion
            // 
            this.lblDstRegion.AutoSize = true;
            this.lblDstRegion.Location = new System.Drawing.Point(91, 145);
            this.lblDstRegion.Name = "lblDstRegion";
            this.lblDstRegion.Size = new System.Drawing.Size(54, 12);
            this.lblDstRegion.TabIndex = 74;
            this.lblDstRegion.Text = "DstRegion";
            // 
            // lblSrcPosition
            // 
            this.lblSrcPosition.AutoSize = true;
            this.lblSrcPosition.Location = new System.Drawing.Point(88, 104);
            this.lblSrcPosition.Name = "lblSrcPosition";
            this.lblSrcPosition.Size = new System.Drawing.Size(57, 12);
            this.lblSrcPosition.TabIndex = 73;
            this.lblSrcPosition.Text = "SrcPosition";
            // 
            // lblSrcRegion
            // 
            this.lblSrcRegion.AutoSize = true;
            this.lblSrcRegion.Location = new System.Drawing.Point(90, 62);
            this.lblSrcRegion.Name = "lblSrcRegion";
            this.lblSrcRegion.Size = new System.Drawing.Size(54, 12);
            this.lblSrcRegion.TabIndex = 72;
            this.lblSrcRegion.Text = "SrcRegion";
            // 
            // lblAgvID
            // 
            this.lblAgvID.AutoSize = true;
            this.lblAgvID.Location = new System.Drawing.Point(99, 16);
            this.lblAgvID.Name = "lblAgvID";
            this.lblAgvID.Size = new System.Drawing.Size(37, 12);
            this.lblAgvID.TabIndex = 71;
            this.lblAgvID.Text = "AgvID";
            // 
            // txtDstPosition
            // 
            this.txtDstPosition.Location = new System.Drawing.Point(80, 202);
            this.txtDstPosition.Name = "txtDstPosition";
            this.txtDstPosition.Size = new System.Drawing.Size(71, 22);
            this.txtDstPosition.TabIndex = 70;
            this.txtDstPosition.Text = "0";
            // 
            // txtDstRegion
            // 
            this.txtDstRegion.Location = new System.Drawing.Point(80, 159);
            this.txtDstRegion.Name = "txtDstRegion";
            this.txtDstRegion.Size = new System.Drawing.Size(71, 22);
            this.txtDstRegion.TabIndex = 69;
            this.txtDstRegion.Text = "0";
            // 
            // txtSrcPosition
            // 
            this.txtSrcPosition.Location = new System.Drawing.Point(80, 118);
            this.txtSrcPosition.Name = "txtSrcPosition";
            this.txtSrcPosition.Size = new System.Drawing.Size(71, 22);
            this.txtSrcPosition.TabIndex = 68;
            this.txtSrcPosition.Text = "0";
            // 
            // txtSrcRegion
            // 
            this.txtSrcRegion.Location = new System.Drawing.Point(80, 76);
            this.txtSrcRegion.Name = "txtSrcRegion";
            this.txtSrcRegion.Size = new System.Drawing.Size(71, 22);
            this.txtSrcRegion.TabIndex = 67;
            this.txtSrcRegion.Text = "0";
            // 
            // txtAgvID
            // 
            this.txtAgvID.Location = new System.Drawing.Point(80, 34);
            this.txtAgvID.Name = "txtAgvID";
            this.txtAgvID.Size = new System.Drawing.Size(71, 22);
            this.txtAgvID.TabIndex = 66;
            this.txtAgvID.Text = "0";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::PLC_Control.Properties.Resources.forklift;
            this.pictureBox1.Location = new System.Drawing.Point(154, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(195, 212);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 64;
            this.pictureBox1.TabStop = false;
            // 
            // txtReceiveServer
            // 
            this.txtReceiveServer.Location = new System.Drawing.Point(6, 231);
            this.txtReceiveServer.Multiline = true;
            this.txtReceiveServer.Name = "txtReceiveServer";
            this.txtReceiveServer.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtReceiveServer.Size = new System.Drawing.Size(145, 114);
            this.txtReceiveServer.TabIndex = 63;
            // 
            // btnStartServer
            // 
            this.btnStartServer.Location = new System.Drawing.Point(6, 30);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(65, 45);
            this.btnStartServer.TabIndex = 62;
            this.btnStartServer.Text = "Server連線";
            this.btnStartServer.UseVisualStyleBackColor = true;
            this.btnStartServer.Click += new System.EventHandler(this.button4_Click);
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(6, 131);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(65, 45);
            this.btnPause.TabIndex = 60;
            this.btnPause.Text = "暫停";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnContinue
            // 
            this.btnContinue.Location = new System.Drawing.Point(5, 81);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(65, 45);
            this.btnContinue.TabIndex = 59;
            this.btnContinue.Text = "繼續";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.lbNAV_IP);
            this.groupBox7.Controls.Add(this.btn_Mode_Navigation);
            this.groupBox7.Controls.Add(this.btnNAVDisConnect);
            this.groupBox7.Controls.Add(this.txtNAVIP);
            this.groupBox7.Controls.Add(this.btnNAVConnect);
            this.groupBox7.Controls.Add(this.txtNAVPort);
            this.groupBox7.Controls.Add(this.lbNAVPort);
            this.groupBox7.Controls.Add(this.btnContinueLocation);
            this.groupBox7.Controls.Add(this.txtNavReceive);
            this.groupBox7.Location = new System.Drawing.Point(7, 10);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(227, 352);
            this.groupBox7.TabIndex = 26;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "NAV-350設定";
            // 
            // lbNAV_IP
            // 
            this.lbNAV_IP.AutoSize = true;
            this.lbNAV_IP.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbNAV_IP.Location = new System.Drawing.Point(22, 31);
            this.lbNAV_IP.Name = "lbNAV_IP";
            this.lbNAV_IP.Size = new System.Drawing.Size(24, 15);
            this.lbNAV_IP.TabIndex = 18;
            this.lbNAV_IP.Text = "IP:";
            // 
            // btn_Mode_Navigation
            // 
            this.btn_Mode_Navigation.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Mode_Navigation.Location = new System.Drawing.Point(17, 124);
            this.btn_Mode_Navigation.Name = "btn_Mode_Navigation";
            this.btn_Mode_Navigation.Size = new System.Drawing.Size(93, 29);
            this.btn_Mode_Navigation.TabIndex = 24;
            this.btn_Mode_Navigation.Text = "Navigation";
            this.btn_Mode_Navigation.UseVisualStyleBackColor = true;
            this.btn_Mode_Navigation.Click += new System.EventHandler(this.btn_Mode_Navigation_Click);
            // 
            // btnNAVDisConnect
            // 
            this.btnNAVDisConnect.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnNAVDisConnect.Location = new System.Drawing.Point(120, 91);
            this.btnNAVDisConnect.Name = "btnNAVDisConnect";
            this.btnNAVDisConnect.Size = new System.Drawing.Size(93, 29);
            this.btnNAVDisConnect.TabIndex = 22;
            this.btnNAVDisConnect.Text = "斷線";
            this.btnNAVDisConnect.UseVisualStyleBackColor = true;
            this.btnNAVDisConnect.Click += new System.EventHandler(this.btnNAVDisConnect_Click);
            // 
            // txtNAVIP
            // 
            this.txtNAVIP.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtNAVIP.Location = new System.Drawing.Point(53, 27);
            this.txtNAVIP.Name = "txtNAVIP";
            this.txtNAVIP.Size = new System.Drawing.Size(160, 25);
            this.txtNAVIP.TabIndex = 17;
            this.txtNAVIP.Text = "169.254.144.10";
            // 
            // btnNAVConnect
            // 
            this.btnNAVConnect.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnNAVConnect.Location = new System.Drawing.Point(17, 91);
            this.btnNAVConnect.Name = "btnNAVConnect";
            this.btnNAVConnect.Size = new System.Drawing.Size(93, 29);
            this.btnNAVConnect.TabIndex = 21;
            this.btnNAVConnect.Text = "連線";
            this.btnNAVConnect.UseVisualStyleBackColor = true;
            this.btnNAVConnect.Click += new System.EventHandler(this.btnNAVConnect_Click);
            // 
            // txtNAVPort
            // 
            this.txtNAVPort.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtNAVPort.Location = new System.Drawing.Point(52, 60);
            this.txtNAVPort.Name = "txtNAVPort";
            this.txtNAVPort.Size = new System.Drawing.Size(161, 25);
            this.txtNAVPort.TabIndex = 19;
            this.txtNAVPort.Text = "2112";
            // 
            // lbNAVPort
            // 
            this.lbNAVPort.AutoSize = true;
            this.lbNAVPort.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbNAVPort.Location = new System.Drawing.Point(14, 64);
            this.lbNAVPort.Name = "lbNAVPort";
            this.lbNAVPort.Size = new System.Drawing.Size(35, 15);
            this.lbNAVPort.TabIndex = 20;
            this.lbNAVPort.Text = "Port:";
            // 
            // btnContinueLocation
            // 
            this.btnContinueLocation.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnContinueLocation.Location = new System.Drawing.Point(120, 124);
            this.btnContinueLocation.Name = "btnContinueLocation";
            this.btnContinueLocation.Size = new System.Drawing.Size(93, 29);
            this.btnContinueLocation.TabIndex = 27;
            this.btnContinueLocation.Text = "連續座標";
            this.btnContinueLocation.UseVisualStyleBackColor = true;
            this.btnContinueLocation.Click += new System.EventHandler(this.btnContinueLocation_Click);
            // 
            // txtNavReceive
            // 
            this.txtNavReceive.Location = new System.Drawing.Point(17, 162);
            this.txtNavReceive.Multiline = true;
            this.txtNavReceive.Name = "txtNavReceive";
            this.txtNavReceive.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNavReceive.Size = new System.Drawing.Size(196, 184);
            this.txtNavReceive.TabIndex = 34;
            // 
            // tabLidar3
            // 
            this.tabLidar3.Controls.Add(this.OpenGLCtrl);
            this.tabLidar3.Controls.Add(this.btnOpenLidarFile);
            this.tabLidar3.Controls.Add(this.btnLidarDataOut);
            this.tabLidar3.Controls.Add(this.btnLidarRecode);
            this.tabLidar3.Controls.Add(this.btnResetView);
            this.tabLidar3.Controls.Add(this.btnStartLidar);
            this.tabLidar3.Location = new System.Drawing.Point(4, 22);
            this.tabLidar3.Margin = new System.Windows.Forms.Padding(2);
            this.tabLidar3.Name = "tabLidar3";
            this.tabLidar3.Padding = new System.Windows.Forms.Padding(2);
            this.tabLidar3.Size = new System.Drawing.Size(600, 368);
            this.tabLidar3.TabIndex = 5;
            this.tabLidar3.Text = "Lidar";
            this.tabLidar3.UseVisualStyleBackColor = true;
            // 
            // OpenGLCtrl
            // 
            this.OpenGLCtrl.BitDepth = 24;
            this.OpenGLCtrl.DrawFPS = false;
            this.OpenGLCtrl.FrameRate = 20;
            this.OpenGLCtrl.Location = new System.Drawing.Point(5, 29);
            this.OpenGLCtrl.Name = "OpenGLCtrl";
            this.OpenGLCtrl.RenderContextType = SharpGL.RenderContextType.DIBSection;
            this.OpenGLCtrl.Size = new System.Drawing.Size(590, 336);
            this.OpenGLCtrl.TabIndex = 98;
            this.OpenGLCtrl.OpenGLDraw += new System.Windows.Forms.PaintEventHandler(this.OpenGLCtrl_OpenGLDraw);
            this.OpenGLCtrl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OpenGLCtrl_MouseDown);
            this.OpenGLCtrl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OpenGLCtrl_MouseUp);
            this.OpenGLCtrl.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.OpenGLCtrl_MouseWheel);
            // 
            // btnOpenLidarFile
            // 
            this.btnOpenLidarFile.Location = new System.Drawing.Point(525, 5);
            this.btnOpenLidarFile.Margin = new System.Windows.Forms.Padding(2);
            this.btnOpenLidarFile.Name = "btnOpenLidarFile";
            this.btnOpenLidarFile.Size = new System.Drawing.Size(71, 19);
            this.btnOpenLidarFile.TabIndex = 84;
            this.btnOpenLidarFile.Text = "開啟檔案";
            this.btnOpenLidarFile.UseVisualStyleBackColor = true;
            this.btnOpenLidarFile.Click += new System.EventHandler(this.btnOpenLidarFile_Click);
            // 
            // btnLidarDataOut
            // 
            this.btnLidarDataOut.Location = new System.Drawing.Point(284, 4);
            this.btnLidarDataOut.Margin = new System.Windows.Forms.Padding(2);
            this.btnLidarDataOut.Name = "btnLidarDataOut";
            this.btnLidarDataOut.Size = new System.Drawing.Size(71, 19);
            this.btnLidarDataOut.TabIndex = 82;
            this.btnLidarDataOut.Text = "資料輸出";
            this.btnLidarDataOut.UseVisualStyleBackColor = true;
            this.btnLidarDataOut.Click += new System.EventHandler(this.btnLidarDataOut_Click);
            // 
            // btnLidarRecode
            // 
            this.btnLidarRecode.Location = new System.Drawing.Point(209, 4);
            this.btnLidarRecode.Margin = new System.Windows.Forms.Padding(2);
            this.btnLidarRecode.Name = "btnLidarRecode";
            this.btnLidarRecode.Size = new System.Drawing.Size(71, 19);
            this.btnLidarRecode.TabIndex = 81;
            this.btnLidarRecode.Text = "開始儲存";
            this.btnLidarRecode.UseVisualStyleBackColor = true;
            this.btnLidarRecode.Click += new System.EventHandler(this.btnLidarRecode_Click);
            // 
            // btnResetView
            // 
            this.btnResetView.Location = new System.Drawing.Point(79, 4);
            this.btnResetView.Margin = new System.Windows.Forms.Padding(2);
            this.btnResetView.Name = "btnResetView";
            this.btnResetView.Size = new System.Drawing.Size(69, 20);
            this.btnResetView.TabIndex = 80;
            this.btnResetView.Text = "重置視角";
            this.btnResetView.UseVisualStyleBackColor = true;
            this.btnResetView.Click += new System.EventHandler(this.btnResetView_Click);
            // 
            // btnStartLidar
            // 
            this.btnStartLidar.Location = new System.Drawing.Point(5, 5);
            this.btnStartLidar.Margin = new System.Windows.Forms.Padding(2);
            this.btnStartLidar.Name = "btnStartLidar";
            this.btnStartLidar.Size = new System.Drawing.Size(71, 19);
            this.btnStartLidar.TabIndex = 79;
            this.btnStartLidar.Text = "開啟光達";
            this.btnStartLidar.UseVisualStyleBackColor = true;
            this.btnStartLidar.Click += new System.EventHandler(this.btnStartLidar_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.groupBox6);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(600, 368);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Setting";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtFrontBackValue);
            this.groupBox3.Controls.Add(this.btnSetForkLeftRight);
            this.groupBox3.Controls.Add(this.btnDoFrontBack);
            this.groupBox3.Controls.Add(this.txtForkValue);
            this.groupBox3.Controls.Add(this.txtRotationValue);
            this.groupBox3.Controls.Add(this.btnSetHeight);
            this.groupBox3.Controls.Add(this.txtSetHeight);
            this.groupBox3.Controls.Add(this.btnRotation);
            this.groupBox3.Location = new System.Drawing.Point(30, 151);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(254, 209);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "一般控制";
            // 
            // txtFrontBackValue
            // 
            this.txtFrontBackValue.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtFrontBackValue.Location = new System.Drawing.Point(119, 146);
            this.txtFrontBackValue.Margin = new System.Windows.Forms.Padding(2);
            this.txtFrontBackValue.Name = "txtFrontBackValue";
            this.txtFrontBackValue.Size = new System.Drawing.Size(120, 27);
            this.txtFrontBackValue.TabIndex = 43;
            this.txtFrontBackValue.Text = "0";
            // 
            // btnSetForkLeftRight
            // 
            this.btnSetForkLeftRight.Location = new System.Drawing.Point(13, 37);
            this.btnSetForkLeftRight.Margin = new System.Windows.Forms.Padding(2);
            this.btnSetForkLeftRight.Name = "btnSetForkLeftRight";
            this.btnSetForkLeftRight.Size = new System.Drawing.Size(91, 26);
            this.btnSetForkLeftRight.TabIndex = 37;
            this.btnSetForkLeftRight.Text = "左右設定";
            this.btnSetForkLeftRight.UseVisualStyleBackColor = true;
            this.btnSetForkLeftRight.Click += new System.EventHandler(this.btnSetForkLeftRight_Click);
            // 
            // btnDoFrontBack
            // 
            this.btnDoFrontBack.Location = new System.Drawing.Point(13, 144);
            this.btnDoFrontBack.Margin = new System.Windows.Forms.Padding(2);
            this.btnDoFrontBack.Name = "btnDoFrontBack";
            this.btnDoFrontBack.Size = new System.Drawing.Size(91, 26);
            this.btnDoFrontBack.TabIndex = 42;
            this.btnDoFrontBack.Text = "前進後退";
            this.btnDoFrontBack.UseVisualStyleBackColor = true;
            this.btnDoFrontBack.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnDoFrontBack_MouseDown);
            this.btnDoFrontBack.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnDoFrontBack_MouseUp);
            // 
            // txtForkValue
            // 
            this.txtForkValue.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtForkValue.Location = new System.Drawing.Point(119, 37);
            this.txtForkValue.Margin = new System.Windows.Forms.Padding(2);
            this.txtForkValue.Name = "txtForkValue";
            this.txtForkValue.Size = new System.Drawing.Size(120, 27);
            this.txtForkValue.TabIndex = 36;
            this.txtForkValue.Text = "13";
            // 
            // txtRotationValue
            // 
            this.txtRotationValue.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtRotationValue.Location = new System.Drawing.Point(119, 110);
            this.txtRotationValue.Margin = new System.Windows.Forms.Padding(2);
            this.txtRotationValue.Name = "txtRotationValue";
            this.txtRotationValue.Size = new System.Drawing.Size(120, 27);
            this.txtRotationValue.TabIndex = 41;
            this.txtRotationValue.Text = "0";
            // 
            // btnSetHeight
            // 
            this.btnSetHeight.Location = new System.Drawing.Point(13, 74);
            this.btnSetHeight.Margin = new System.Windows.Forms.Padding(2);
            this.btnSetHeight.Name = "btnSetHeight";
            this.btnSetHeight.Size = new System.Drawing.Size(91, 26);
            this.btnSetHeight.TabIndex = 38;
            this.btnSetHeight.Text = "高度設定";
            this.btnSetHeight.UseVisualStyleBackColor = true;
            this.btnSetHeight.Click += new System.EventHandler(this.btnSetHeight_Click);
            // 
            // txtSetHeight
            // 
            this.txtSetHeight.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtSetHeight.Location = new System.Drawing.Point(119, 74);
            this.txtSetHeight.Margin = new System.Windows.Forms.Padding(2);
            this.txtSetHeight.Name = "txtSetHeight";
            this.txtSetHeight.Size = new System.Drawing.Size(120, 27);
            this.txtSetHeight.TabIndex = 39;
            this.txtSetHeight.Text = "52";
            // 
            // btnRotation
            // 
            this.btnRotation.Location = new System.Drawing.Point(13, 110);
            this.btnRotation.Margin = new System.Windows.Forms.Padding(2);
            this.btnRotation.Name = "btnRotation";
            this.btnRotation.Size = new System.Drawing.Size(91, 26);
            this.btnRotation.TabIndex = 40;
            this.btnRotation.Text = "轉向角度";
            this.btnRotation.UseVisualStyleBackColor = true;
            this.btnRotation.Click += new System.EventHandler(this.btnRotation_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.groupBox8);
            this.groupBox6.Controls.Add(this.btnArduino);
            this.groupBox6.Controls.Add(this.txtArduinoPortNumber);
            this.groupBox6.Controls.Add(this.lblArduinoPort);
            this.groupBox6.Controls.Add(this.lblArduinoBaudRate);
            this.groupBox6.Controls.Add(this.btnClearList);
            this.groupBox6.Controls.Add(this.pictureBox_Angle);
            this.groupBox6.Controls.Add(this.txtArduinoBaudRate);
            this.groupBox6.Location = new System.Drawing.Point(290, 7);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(273, 353);
            this.groupBox6.TabIndex = 11;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Arduino設定";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.btnSetUserLevel);
            this.groupBox8.Controls.Add(this.btn_Mode_LandMark);
            this.groupBox8.Controls.Add(this.btn_Mode_Mapping);
            this.groupBox8.Controls.Add(this.btnSaveTxt);
            this.groupBox8.Controls.Add(this.btn_Mode_Standby);
            this.groupBox8.Controls.Add(this.btn_Mode_PowerDown);
            this.groupBox8.Location = new System.Drawing.Point(12, 154);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(248, 212);
            this.groupBox8.TabIndex = 27;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "模式切換";
            // 
            // btnSetUserLevel
            // 
            this.btnSetUserLevel.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSetUserLevel.Location = new System.Drawing.Point(144, 140);
            this.btnSetUserLevel.Name = "btnSetUserLevel";
            this.btnSetUserLevel.Size = new System.Drawing.Size(80, 45);
            this.btnSetUserLevel.TabIndex = 25;
            this.btnSetUserLevel.Text = "UserLevel";
            this.btnSetUserLevel.UseVisualStyleBackColor = true;
            this.btnSetUserLevel.Click += new System.EventHandler(this.btnSetUserLevel_Click);
            // 
            // btn_Mode_LandMark
            // 
            this.btn_Mode_LandMark.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Mode_LandMark.Location = new System.Drawing.Point(144, 79);
            this.btn_Mode_LandMark.Name = "btn_Mode_LandMark";
            this.btn_Mode_LandMark.Size = new System.Drawing.Size(80, 50);
            this.btn_Mode_LandMark.TabIndex = 23;
            this.btn_Mode_LandMark.Text = "Landmark Detection";
            this.btn_Mode_LandMark.UseVisualStyleBackColor = true;
            this.btn_Mode_LandMark.Click += new System.EventHandler(this.btn_Mode_LandMark_Click);
            // 
            // btn_Mode_Mapping
            // 
            this.btn_Mode_Mapping.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Mode_Mapping.Location = new System.Drawing.Point(20, 81);
            this.btn_Mode_Mapping.Name = "btn_Mode_Mapping";
            this.btn_Mode_Mapping.Size = new System.Drawing.Size(80, 50);
            this.btn_Mode_Mapping.TabIndex = 22;
            this.btn_Mode_Mapping.Text = "Mapping";
            this.btn_Mode_Mapping.UseVisualStyleBackColor = true;
            this.btn_Mode_Mapping.Click += new System.EventHandler(this.btn_Mode_Mapping_Click);
            // 
            // btnSaveTxt
            // 
            this.btnSaveTxt.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSaveTxt.Location = new System.Drawing.Point(19, 140);
            this.btnSaveTxt.Name = "btnSaveTxt";
            this.btnSaveTxt.Size = new System.Drawing.Size(79, 45);
            this.btnSaveTxt.TabIndex = 43;
            this.btnSaveTxt.Text = "存入txt";
            this.btnSaveTxt.UseVisualStyleBackColor = true;
            this.btnSaveTxt.Click += new System.EventHandler(this.btnSaveTxt_Click);
            // 
            // btn_Mode_Standby
            // 
            this.btn_Mode_Standby.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Mode_Standby.Location = new System.Drawing.Point(144, 19);
            this.btn_Mode_Standby.Name = "btn_Mode_Standby";
            this.btn_Mode_Standby.Size = new System.Drawing.Size(80, 50);
            this.btn_Mode_Standby.TabIndex = 21;
            this.btn_Mode_Standby.Text = "Standby";
            this.btn_Mode_Standby.UseVisualStyleBackColor = true;
            this.btn_Mode_Standby.Click += new System.EventHandler(this.btn_Mode_Standby_Click);
            // 
            // btn_Mode_PowerDown
            // 
            this.btn_Mode_PowerDown.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Mode_PowerDown.Location = new System.Drawing.Point(19, 20);
            this.btn_Mode_PowerDown.Name = "btn_Mode_PowerDown";
            this.btn_Mode_PowerDown.Size = new System.Drawing.Size(80, 50);
            this.btn_Mode_PowerDown.TabIndex = 17;
            this.btn_Mode_PowerDown.Text = "Power Down";
            this.btn_Mode_PowerDown.UseVisualStyleBackColor = true;
            this.btn_Mode_PowerDown.Click += new System.EventHandler(this.btn_Mode_PowerDown_Click);
            // 
            // btnArduino
            // 
            this.btnArduino.Location = new System.Drawing.Point(31, 108);
            this.btnArduino.Name = "btnArduino";
            this.btnArduino.Size = new System.Drawing.Size(79, 45);
            this.btnArduino.TabIndex = 17;
            this.btnArduino.Text = "OpenArduino";
            this.btnArduino.UseVisualStyleBackColor = true;
            this.btnArduino.Click += new System.EventHandler(this.btnArduino_Click);
            // 
            // txtArduinoPortNumber
            // 
            this.txtArduinoPortNumber.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtArduinoPortNumber.Location = new System.Drawing.Point(119, 22);
            this.txtArduinoPortNumber.Name = "txtArduinoPortNumber";
            this.txtArduinoPortNumber.Size = new System.Drawing.Size(120, 27);
            this.txtArduinoPortNumber.TabIndex = 16;
            this.txtArduinoPortNumber.Text = "COM11";
            // 
            // lblArduinoPort
            // 
            this.lblArduinoPort.AutoSize = true;
            this.lblArduinoPort.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblArduinoPort.Location = new System.Drawing.Point(17, 25);
            this.lblArduinoPort.Name = "lblArduinoPort";
            this.lblArduinoPort.Size = new System.Drawing.Size(96, 16);
            this.lblArduinoPort.TabIndex = 15;
            this.lblArduinoPort.Text = "PortNumber : ";
            // 
            // lblArduinoBaudRate
            // 
            this.lblArduinoBaudRate.AutoSize = true;
            this.lblArduinoBaudRate.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblArduinoBaudRate.Location = new System.Drawing.Point(17, 71);
            this.lblArduinoBaudRate.Name = "lblArduinoBaudRate";
            this.lblArduinoBaudRate.Size = new System.Drawing.Size(81, 16);
            this.lblArduinoBaudRate.TabIndex = 17;
            this.lblArduinoBaudRate.Text = "BaudRate : ";
            // 
            // btnClearList
            // 
            this.btnClearList.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnClearList.Location = new System.Drawing.Point(156, 108);
            this.btnClearList.Name = "btnClearList";
            this.btnClearList.Size = new System.Drawing.Size(80, 45);
            this.btnClearList.TabIndex = 42;
            this.btnClearList.Text = "清空List";
            this.btnClearList.UseVisualStyleBackColor = true;
            this.btnClearList.Click += new System.EventHandler(this.btnClearList_Click);
            // 
            // pictureBox_Angle
            // 
            this.pictureBox_Angle.Location = new System.Drawing.Point(248, 70);
            this.pictureBox_Angle.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox_Angle.Name = "pictureBox_Angle";
            this.pictureBox_Angle.Size = new System.Drawing.Size(20, 17);
            this.pictureBox_Angle.TabIndex = 16;
            this.pictureBox_Angle.TabStop = false;
            // 
            // txtArduinoBaudRate
            // 
            this.txtArduinoBaudRate.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtArduinoBaudRate.Location = new System.Drawing.Point(119, 68);
            this.txtArduinoBaudRate.Name = "txtArduinoBaudRate";
            this.txtArduinoBaudRate.Size = new System.Drawing.Size(120, 27);
            this.txtArduinoBaudRate.TabIndex = 18;
            this.txtArduinoBaudRate.Text = "9600";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtWPortNumber);
            this.groupBox4.Controls.Add(this.lblWPortNumber);
            this.groupBox4.Controls.Add(this.lblWBaudRate);
            this.groupBox4.Controls.Add(this.txtWBaudRate);
            this.groupBox4.Location = new System.Drawing.Point(30, 7);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(254, 138);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "重量感測器設定";
            // 
            // txtWPortNumber
            // 
            this.txtWPortNumber.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtWPortNumber.Location = new System.Drawing.Point(119, 40);
            this.txtWPortNumber.Name = "txtWPortNumber";
            this.txtWPortNumber.Size = new System.Drawing.Size(120, 27);
            this.txtWPortNumber.TabIndex = 16;
            this.txtWPortNumber.Text = "COM8";
            // 
            // lblWPortNumber
            // 
            this.lblWPortNumber.AutoSize = true;
            this.lblWPortNumber.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblWPortNumber.Location = new System.Drawing.Point(17, 43);
            this.lblWPortNumber.Name = "lblWPortNumber";
            this.lblWPortNumber.Size = new System.Drawing.Size(96, 16);
            this.lblWPortNumber.TabIndex = 15;
            this.lblWPortNumber.Text = "PortNumber : ";
            // 
            // lblWBaudRate
            // 
            this.lblWBaudRate.AutoSize = true;
            this.lblWBaudRate.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblWBaudRate.Location = new System.Drawing.Point(17, 90);
            this.lblWBaudRate.Name = "lblWBaudRate";
            this.lblWBaudRate.Size = new System.Drawing.Size(81, 16);
            this.lblWBaudRate.TabIndex = 17;
            this.lblWBaudRate.Text = "BaudRate : ";
            // 
            // txtWBaudRate
            // 
            this.txtWBaudRate.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtWBaudRate.Location = new System.Drawing.Point(119, 86);
            this.txtWBaudRate.Name = "txtWBaudRate";
            this.txtWBaudRate.Size = new System.Drawing.Size(120, 27);
            this.txtWBaudRate.TabIndex = 18;
            this.txtWBaudRate.Text = "115200";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(600, 368);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Control";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnOpenCan);
            this.groupBox1.Controls.Add(this.btnCanConnect);
            this.groupBox1.Controls.Add(this.btnBrakes);
            this.groupBox1.Controls.Add(this.btnClutch);
            this.groupBox1.Controls.Add(this.groupBox5);
            this.groupBox1.Controls.Add(this.label_Path);
            this.groupBox1.Controls.Add(this.label_PLCConnect);
            this.groupBox1.Controls.Add(this.btnDisConnect);
            this.groupBox1.Controls.Add(this.btnConnect);
            this.groupBox1.Location = new System.Drawing.Point(15, 166);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(570, 197);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "操控按鈕";
            // 
            // btnOpenCan
            // 
            this.btnOpenCan.Location = new System.Drawing.Point(143, 24);
            this.btnOpenCan.Name = "btnOpenCan";
            this.btnOpenCan.Size = new System.Drawing.Size(114, 36);
            this.btnOpenCan.TabIndex = 16;
            this.btnOpenCan.Text = "開啟CAN";
            this.btnOpenCan.UseVisualStyleBackColor = true;
            this.btnOpenCan.Click += new System.EventHandler(this.btnOpenCan_Click);
            // 
            // btnCanConnect
            // 
            this.btnCanConnect.Location = new System.Drawing.Point(17, 24);
            this.btnCanConnect.Name = "btnCanConnect";
            this.btnCanConnect.Size = new System.Drawing.Size(114, 36);
            this.btnCanConnect.TabIndex = 15;
            this.btnCanConnect.Text = "CAN連線";
            this.btnCanConnect.UseVisualStyleBackColor = true;
            this.btnCanConnect.Click += new System.EventHandler(this.btnCanConnect_Click);
            // 
            // btnBrakes
            // 
            this.btnBrakes.Location = new System.Drawing.Point(143, 150);
            this.btnBrakes.Margin = new System.Windows.Forms.Padding(2);
            this.btnBrakes.Name = "btnBrakes";
            this.btnBrakes.Size = new System.Drawing.Size(114, 34);
            this.btnBrakes.TabIndex = 14;
            this.btnBrakes.Text = "煞車";
            this.btnBrakes.UseVisualStyleBackColor = true;
            this.btnBrakes.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnBrakes_MouseDown);
            this.btnBrakes.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnBrakes_MouseUp);
            // 
            // btnClutch
            // 
            this.btnClutch.Location = new System.Drawing.Point(17, 150);
            this.btnClutch.Margin = new System.Windows.Forms.Padding(2);
            this.btnClutch.Name = "btnClutch";
            this.btnClutch.Size = new System.Drawing.Size(114, 34);
            this.btnClutch.TabIndex = 13;
            this.btnClutch.Text = "離合器";
            this.btnClutch.UseVisualStyleBackColor = true;
            this.btnClutch.Click += new System.EventHandler(this.btnClutch_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnSloping);
            this.groupBox5.Controls.Add(this.btnRight);
            this.groupBox5.Controls.Add(this.btnLeft);
            this.groupBox5.Controls.Add(this.btnOblique);
            this.groupBox5.Controls.Add(this.btnBack);
            this.groupBox5.Controls.Add(this.btnFront);
            this.groupBox5.Controls.Add(this.btnDown);
            this.groupBox5.Controls.Add(this.btnUp);
            this.groupBox5.Location = new System.Drawing.Point(271, 18);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox5.Size = new System.Drawing.Size(294, 169);
            this.groupBox5.TabIndex = 12;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "前端操控";
            // 
            // btnSloping
            // 
            this.btnSloping.Location = new System.Drawing.Point(155, 96);
            this.btnSloping.Margin = new System.Windows.Forms.Padding(2);
            this.btnSloping.Name = "btnSloping";
            this.btnSloping.Size = new System.Drawing.Size(63, 50);
            this.btnSloping.TabIndex = 15;
            this.btnSloping.Text = "下傾斜";
            this.btnSloping.UseVisualStyleBackColor = true;
            this.btnSloping.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnSloping_MouseDown);
            this.btnSloping.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnSloping_MouseUp);
            // 
            // btnRight
            // 
            this.btnRight.Location = new System.Drawing.Point(226, 96);
            this.btnRight.Margin = new System.Windows.Forms.Padding(2);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(63, 50);
            this.btnRight.TabIndex = 13;
            this.btnRight.Text = "右";
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnRight_MouseDown);
            this.btnRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnRight_MouseUp);
            // 
            // btnLeft
            // 
            this.btnLeft.Location = new System.Drawing.Point(226, 27);
            this.btnLeft.Margin = new System.Windows.Forms.Padding(2);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(63, 50);
            this.btnLeft.TabIndex = 14;
            this.btnLeft.Text = "左";
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnLeft_MouseDown);
            this.btnLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnLeft_MouseUp);
            // 
            // btnOblique
            // 
            this.btnOblique.Location = new System.Drawing.Point(154, 27);
            this.btnOblique.Margin = new System.Windows.Forms.Padding(2);
            this.btnOblique.Name = "btnOblique";
            this.btnOblique.Size = new System.Drawing.Size(63, 50);
            this.btnOblique.TabIndex = 16;
            this.btnOblique.Text = "上傾斜";
            this.btnOblique.UseVisualStyleBackColor = true;
            this.btnOblique.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnOblique_MouseDown);
            this.btnOblique.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnOblique_MouseUp);
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(81, 96);
            this.btnBack.Margin = new System.Windows.Forms.Padding(2);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(63, 50);
            this.btnBack.TabIndex = 10;
            this.btnBack.Text = "後";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnBack_MouseDown);
            this.btnBack.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnBack_MouseUp);
            // 
            // btnFront
            // 
            this.btnFront.Location = new System.Drawing.Point(81, 27);
            this.btnFront.Margin = new System.Windows.Forms.Padding(2);
            this.btnFront.Name = "btnFront";
            this.btnFront.Size = new System.Drawing.Size(63, 50);
            this.btnFront.TabIndex = 11;
            this.btnFront.Text = "前";
            this.btnFront.UseVisualStyleBackColor = true;
            this.btnFront.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnFront_MouseDown);
            this.btnFront.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnFront_MouseUp);
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(8, 97);
            this.btnDown.Margin = new System.Windows.Forms.Padding(2);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(63, 50);
            this.btnDown.TabIndex = 8;
            this.btnDown.Text = " 下降";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnDown_MouseDown);
            this.btnDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnDown_MouseUp);
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(9, 27);
            this.btnUp.Margin = new System.Windows.Forms.Padding(2);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(63, 50);
            this.btnUp.TabIndex = 9;
            this.btnUp.Text = "上升";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnUp_MouseDown);
            this.btnUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnUp_MouseUp);
            // 
            // label_Path
            // 
            this.label_Path.AutoSize = true;
            this.label_Path.Location = new System.Drawing.Point(19, 134);
            this.label_Path.Name = "label_Path";
            this.label_Path.Size = new System.Drawing.Size(53, 12);
            this.label_Path.TabIndex = 7;
            this.label_Path.Text = "踏板操控";
            // 
            // label_PLCConnect
            // 
            this.label_PLCConnect.AutoSize = true;
            this.label_PLCConnect.Location = new System.Drawing.Point(18, 69);
            this.label_PLCConnect.Name = "label_PLCConnect";
            this.label_PLCConnect.Size = new System.Drawing.Size(50, 12);
            this.label_PLCConnect.TabIndex = 6;
            this.label_PLCConnect.Text = "PLC連線";
            // 
            // btnDisConnect
            // 
            this.btnDisConnect.Location = new System.Drawing.Point(144, 88);
            this.btnDisConnect.Name = "btnDisConnect";
            this.btnDisConnect.Size = new System.Drawing.Size(114, 37);
            this.btnDisConnect.TabIndex = 2;
            this.btnDisConnect.Text = "DisConnect";
            this.btnDisConnect.UseVisualStyleBackColor = true;
            this.btnDisConnect.Click += new System.EventHandler(this.btnDisConnect_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(17, 88);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(114, 37);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelCANStatus);
            this.groupBox2.Controls.Add(this.axDBCommManager_Detector);
            this.groupBox2.Controls.Add(this.pictureBox2);
            this.groupBox2.Location = new System.Drawing.Point(17, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(570, 162);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "監看";
            // 
            // labelCANStatus
            // 
            this.labelCANStatus.AutoSize = true;
            this.labelCANStatus.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labelCANStatus.Location = new System.Drawing.Point(3, 12);
            this.labelCANStatus.Name = "labelCANStatus";
            this.labelCANStatus.Size = new System.Drawing.Size(64, 15);
            this.labelCANStatus.TabIndex = 10;
            this.labelCANStatus.Text = "初始化...";
            // 
            // axDBCommManager_Detector
            // 
            this.axDBCommManager_Detector.Enabled = true;
            this.axDBCommManager_Detector.Location = new System.Drawing.Point(8, 135);
            this.axDBCommManager_Detector.Name = "axDBCommManager_Detector";
            this.axDBCommManager_Detector.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axDBCommManager_Detector.OcxState")));
            this.axDBCommManager_Detector.Size = new System.Drawing.Size(24, 24);
            this.axDBCommManager_Detector.TabIndex = 0;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::PLC_Control.Properties.Resources.forklift__1_;
            this.pictureBox2.Location = new System.Drawing.Point(-2, 0);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(572, 162);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 43;
            this.pictureBox2.TabStop = false;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Controls.Add(this.tabLidar3);
            this.tabControl.Controls.Add(this.tabPage4);
            this.tabControl.Location = new System.Drawing.Point(10, 32);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(608, 394);
            this.tabControl.TabIndex = 8;
            // 
            // menuStrip_System
            // 
            this.menuStrip_System.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.功能ToolStripMenuItem,
            this.說明ToolStripMenuItem});
            this.menuStrip_System.Location = new System.Drawing.Point(0, 0);
            this.menuStrip_System.Name = "menuStrip_System";
            this.menuStrip_System.Size = new System.Drawing.Size(789, 24);
            this.menuStrip_System.TabIndex = 49;
            this.menuStrip_System.Text = "menuStrip1";
            // 
            // 功能ToolStripMenuItem
            // 
            this.功能ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.主要控制ToolStripMenuItem,
            this.離線除錯ToolStripMenuItem});
            this.功能ToolStripMenuItem.Name = "功能ToolStripMenuItem";
            this.功能ToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.功能ToolStripMenuItem.Text = "功能";
            // 
            // 主要控制ToolStripMenuItem
            // 
            this.主要控制ToolStripMenuItem.Name = "主要控制ToolStripMenuItem";
            this.主要控制ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.主要控制ToolStripMenuItem.Text = "主要控制";
            this.主要控制ToolStripMenuItem.Click += new System.EventHandler(this.主要控制ToolStripMenuItem_Click);
            // 
            // 離線除錯ToolStripMenuItem
            // 
            this.離線除錯ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.log檔除錯ToolStripMenuItem,
            this.文字輸入除錯ToolStripMenuItem});
            this.離線除錯ToolStripMenuItem.Name = "離線除錯ToolStripMenuItem";
            this.離線除錯ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.離線除錯ToolStripMenuItem.Text = "離線除錯";
            // 
            // log檔除錯ToolStripMenuItem
            // 
            this.log檔除錯ToolStripMenuItem.Name = "log檔除錯ToolStripMenuItem";
            this.log檔除錯ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.log檔除錯ToolStripMenuItem.Text = "Log檔除錯";
            this.log檔除錯ToolStripMenuItem.Click += new System.EventHandler(this.log檔除錯ToolStripMenuItem_Click);
            // 
            // 文字輸入除錯ToolStripMenuItem
            // 
            this.文字輸入除錯ToolStripMenuItem.Name = "文字輸入除錯ToolStripMenuItem";
            this.文字輸入除錯ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.文字輸入除錯ToolStripMenuItem.Text = "文字輸入除錯";
            this.文字輸入除錯ToolStripMenuItem.Click += new System.EventHandler(this.文字輸入除錯ToolStripMenuItem_Click);
            // 
            // 說明ToolStripMenuItem
            // 
            this.說明ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.關於ToolStripMenuItem});
            this.說明ToolStripMenuItem.Name = "說明ToolStripMenuItem";
            this.說明ToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.說明ToolStripMenuItem.Text = "說明";
            // 
            // 關於ToolStripMenuItem
            // 
            this.關於ToolStripMenuItem.Name = "關於ToolStripMenuItem";
            this.關於ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.關於ToolStripMenuItem.Text = "關於";
            this.關於ToolStripMenuItem.Click += new System.EventHandler(this.關於ToolStripMenuItem_Click);
            // 
            // panelSetLocation
            // 
            this.panelSetLocation.Location = new System.Drawing.Point(0, 29);
            this.panelSetLocation.Name = "panelSetLocation";
            this.panelSetLocation.Size = new System.Drawing.Size(789, 504);
            this.panelSetLocation.TabIndex = 50;
            this.panelSetLocation.Visible = false;
            // 
            // panelDebug
            // 
            this.panelDebug.Location = new System.Drawing.Point(769, 2);
            this.panelDebug.Name = "panelDebug";
            this.panelDebug.Size = new System.Drawing.Size(17, 22);
            this.panelDebug.TabIndex = 51;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 533);
            this.Controls.Add(this.panelDebug);
            this.Controls.Add(this.btnClibratOrigin);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.btnSetTimeStamp);
            this.Controls.Add(this.btnRelaxMotor);
            this.Controls.Add(this.brnMoveBack);
            this.Controls.Add(this.btnMoveFront);
            this.Controls.Add(this.btnEmergencyStop);
            this.Controls.Add(this.btnDirectionBack);
            this.Controls.Add(this.btnCANMode);
            this.Controls.Add(this.btnDirectionFront);
            this.Controls.Add(this.btnOrigin);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnAccelerator);
            this.Controls.Add(this.btnSendL);
            this.Controls.Add(this.btnSendR);
            this.Controls.Add(this.menuStrip_System);
            this.Controls.Add(this.panelSetLocation);
            this.MainMenuStrip = this.menuStrip_System;
            this.Name = "MainForm";
            this.Text = "AGV-Boltun Corporation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.tabLidar3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.OpenGLCtrl)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Angle)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axDBCommManager_Detector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.menuStrip_System.ResumeLayout(false);
            this.menuStrip_System.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MonthCalendar monthCalendar1;
        private System.Windows.Forms.Button btnSendR;
        private System.Windows.Forms.Button btnSendL;
        private System.Windows.Forms.Button btnAccelerator;
        private System.Windows.Forms.Timer timer_Rotation;
        private System.Windows.Forms.Timer timer_system;
        private System.Windows.Forms.Button btnOrigin;
        private System.Windows.Forms.Button btnCANMode;
        private System.Windows.Forms.Button btnDirectionBack;
        private System.Windows.Forms.Button btnDirectionFront;
        private System.Windows.Forms.Button btnEmergencyStop;
        private System.Windows.Forms.Button btnMoveFront;
        private System.Windows.Forms.Button brnMoveBack;
        private System.Windows.Forms.Button btnRelaxMotor;
        private System.Windows.Forms.Button btnClibratOrigin;
        private System.Windows.Forms.Button btnSetTimeStamp;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridTotalTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridAvearageTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnDoCmd;
        private System.Windows.Forms.Label lblDstPosition;
        private System.Windows.Forms.Label lblDstRegion;
        private System.Windows.Forms.Label lblSrcPosition;
        private System.Windows.Forms.Label lblSrcRegion;
        private System.Windows.Forms.Label lblAgvID;
        private System.Windows.Forms.TextBox txtDstPosition;
        private System.Windows.Forms.TextBox txtDstRegion;
        private System.Windows.Forms.TextBox txtSrcPosition;
        private System.Windows.Forms.TextBox txtSrcRegion;
        private System.Windows.Forms.TextBox txtAgvID;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox txtReceiveServer;
        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label lbNAV_IP;
        private System.Windows.Forms.Button btn_Mode_Navigation;
        private System.Windows.Forms.Button btnNAVDisConnect;
        private System.Windows.Forms.TextBox txtNAVIP;
        private System.Windows.Forms.Button btnNAVConnect;
        private System.Windows.Forms.TextBox txtNAVPort;
        private System.Windows.Forms.Label lbNAVPort;
        private System.Windows.Forms.Button btnContinueLocation;
        private System.Windows.Forms.TextBox txtNavReceive;
        private System.Windows.Forms.TabPage tabLidar3;
        private System.Windows.Forms.Button btnOpenLidarFile;
        private System.Windows.Forms.Button btnLidarDataOut;
        private System.Windows.Forms.Button btnLidarRecode;
        private System.Windows.Forms.Button btnResetView;
        private System.Windows.Forms.Button btnStartLidar;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtFrontBackValue;
        private System.Windows.Forms.Button btnSetForkLeftRight;
        private System.Windows.Forms.Button btnDoFrontBack;
        private System.Windows.Forms.TextBox txtForkValue;
        private System.Windows.Forms.TextBox txtRotationValue;
        private System.Windows.Forms.Button btnSetHeight;
        private System.Windows.Forms.TextBox txtSetHeight;
        private System.Windows.Forms.Button btnRotation;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Button btnSetUserLevel;
        private System.Windows.Forms.Button btn_Mode_LandMark;
        private System.Windows.Forms.Button btn_Mode_Mapping;
        private System.Windows.Forms.Button btnSaveTxt;
        private System.Windows.Forms.Button btn_Mode_Standby;
        private System.Windows.Forms.Button btn_Mode_PowerDown;
        private System.Windows.Forms.Button btnArduino;
        private System.Windows.Forms.TextBox txtArduinoPortNumber;
        private System.Windows.Forms.Label lblArduinoPort;
        private System.Windows.Forms.Label lblArduinoBaudRate;
        private System.Windows.Forms.Button btnClearList;
        private System.Windows.Forms.PictureBox pictureBox_Angle;
        private System.Windows.Forms.TextBox txtArduinoBaudRate;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtWPortNumber;
        private System.Windows.Forms.Label lblWPortNumber;
        private System.Windows.Forms.Label lblWBaudRate;
        private System.Windows.Forms.TextBox txtWBaudRate;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnOpenCan;
        private System.Windows.Forms.Button btnCanConnect;
        private System.Windows.Forms.Button btnBrakes;
        private System.Windows.Forms.Button btnClutch;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnSloping;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Button btnOblique;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnFront;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Label label_Path;
        private System.Windows.Forms.Label label_PLCConnect;
        private System.Windows.Forms.Button btnDisConnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label labelCANStatus;
        private AxDATABUILDERAXLibLB.AxDBCommManager axDBCommManager_Detector;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.TabControl tabControl;
        private SharpGL.OpenGLControl OpenGLCtrl;
        private System.Windows.Forms.MenuStrip menuStrip_System;
        private System.Windows.Forms.ToolStripMenuItem 功能ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 主要控制ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 離線除錯ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem log檔除錯ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 文字輸入除錯ToolStripMenuItem;
        private System.Windows.Forms.Panel panelSetLocation;
        public System.Windows.Forms.Panel panelDebug;
        private System.Windows.Forms.ToolStripMenuItem 說明ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 關於ToolStripMenuItem;
    }
}

