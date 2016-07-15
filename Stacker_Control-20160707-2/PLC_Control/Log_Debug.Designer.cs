namespace PLC_Control
{
    partial class Log_Debug
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnLogBack = new System.Windows.Forms.Button();
            this.lblDataIndex = new System.Windows.Forms.Label();
            this.groupBox_Log = new System.Windows.Forms.GroupBox();
            this.dataGridView_Log = new System.Windows.Forms.DataGridView();
            this.btnMachine = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtDataIndex = new System.Windows.Forms.TextBox();
            this.btnReadData = new System.Windows.Forms.Button();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.label_LogMotorAngle = new System.Windows.Forms.Label();
            this.label_LogPower = new System.Windows.Forms.Label();
            this.groupBox_OperationalResults = new System.Windows.Forms.GroupBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.groupBox_Log.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Log)).BeginInit();
            this.groupBox_OperationalResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLogBack
            // 
            this.btnLogBack.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnLogBack.Location = new System.Drawing.Point(694, 462);
            this.btnLogBack.Name = "btnLogBack";
            this.btnLogBack.Size = new System.Drawing.Size(89, 38);
            this.btnLogBack.TabIndex = 0;
            this.btnLogBack.Text = "返回";
            this.btnLogBack.UseVisualStyleBackColor = true;
            this.btnLogBack.Click += new System.EventHandler(this.btnLogBack_Click);
            // 
            // lblDataIndex
            // 
            this.lblDataIndex.AutoSize = true;
            this.lblDataIndex.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblDataIndex.Location = new System.Drawing.Point(38, 114);
            this.lblDataIndex.Name = "lblDataIndex";
            this.lblDataIndex.Size = new System.Drawing.Size(123, 19);
            this.lblDataIndex.TabIndex = 9;
            this.lblDataIndex.Text = "請輸入列數：";
            // 
            // groupBox_Log
            // 
            this.groupBox_Log.Controls.Add(this.dataGridView_Log);
            this.groupBox_Log.Location = new System.Drawing.Point(3, 295);
            this.groupBox_Log.Name = "groupBox_Log";
            this.groupBox_Log.Size = new System.Drawing.Size(790, 166);
            this.groupBox_Log.TabIndex = 8;
            this.groupBox_Log.TabStop = false;
            this.groupBox_Log.Text = "讀取到的資料";
            // 
            // dataGridView_Log
            // 
            this.dataGridView_Log.AllowUserToAddRows = false;
            this.dataGridView_Log.AllowUserToDeleteRows = false;
            this.dataGridView_Log.AllowUserToResizeRows = false;
            this.dataGridView_Log.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_Log.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView_Log.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView_Log.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_Log.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView_Log.ColumnHeadersHeight = 30;
            this.dataGridView_Log.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView_Log.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.btnMachine,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.gridTotalTime,
            this.gridAvearageTotal,
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6});
            this.dataGridView_Log.Location = new System.Drawing.Point(5, 20);
            this.dataGridView_Log.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView_Log.MultiSelect = false;
            this.dataGridView_Log.Name = "dataGridView_Log";
            this.dataGridView_Log.ReadOnly = true;
            this.dataGridView_Log.RowHeadersVisible = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dataGridView_Log.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView_Log.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dataGridView_Log.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.Transparent;
            this.dataGridView_Log.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dataGridView_Log.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.dataGridView_Log.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridView_Log.RowTemplate.Height = 50;
            this.dataGridView_Log.RowTemplate.ReadOnly = true;
            this.dataGridView_Log.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_Log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView_Log.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_Log.Size = new System.Drawing.Size(780, 139);
            this.dataGridView_Log.TabIndex = 42;
            // 
            // btnMachine
            // 
            this.btnMachine.FillWeight = 40F;
            this.btnMachine.HeaderText = "Index";
            this.btnMachine.Name = "btnMachine";
            this.btnMachine.ReadOnly = true;
            this.btnMachine.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnMachine.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.FillWeight = 60F;
            this.dataGridViewTextBoxColumn1.HeaderText = "P_X";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.FillWeight = 60F;
            this.dataGridViewTextBoxColumn2.HeaderText = "P_Y";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.FillWeight = 70F;
            this.dataGridViewTextBoxColumn3.HeaderText = "Angle";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // gridTotalTime
            // 
            this.gridTotalTime.FillWeight = 70F;
            this.gridTotalTime.HeaderText = "R_X";
            this.gridTotalTime.Name = "gridTotalTime";
            this.gridTotalTime.ReadOnly = true;
            // 
            // gridAvearageTotal
            // 
            this.gridAvearageTotal.FillWeight = 70F;
            this.gridAvearageTotal.HeaderText = "R_Y";
            this.gridAvearageTotal.Name = "gridAvearageTotal";
            this.gridAvearageTotal.ReadOnly = true;
            // 
            // Column1
            // 
            this.Column1.FillWeight = 70F;
            this.Column1.HeaderText = "L_X";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.FillWeight = 90F;
            this.Column2.HeaderText = "L_Y";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.FillWeight = 40F;
            this.Column3.HeaderText = "S_L";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.FillWeight = 40F;
            this.Column4.HeaderText = "S_R";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column5
            // 
            this.Column5.FillWeight = 80F;
            this.Column5.HeaderText = "Src";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Dst";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            // 
            // txtDataIndex
            // 
            this.txtDataIndex.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtDataIndex.Location = new System.Drawing.Point(37, 138);
            this.txtDataIndex.Name = "txtDataIndex";
            this.txtDataIndex.Size = new System.Drawing.Size(100, 30);
            this.txtDataIndex.TabIndex = 7;
            // 
            // btnReadData
            // 
            this.btnReadData.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnReadData.Location = new System.Drawing.Point(652, 239);
            this.btnReadData.Name = "btnReadData";
            this.btnReadData.Size = new System.Drawing.Size(121, 50);
            this.btnReadData.TabIndex = 6;
            this.btnReadData.Text = "讀取資料並計算結果";
            this.btnReadData.UseVisualStyleBackColor = true;
            this.btnReadData.Click += new System.EventHandler(this.btnReadData_Click);
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnOpenFile.Location = new System.Drawing.Point(37, 41);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(121, 29);
            this.btnOpenFile.TabIndex = 5;
            this.btnOpenFile.Text = "開啟檔案";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // label_LogMotorAngle
            // 
            this.label_LogMotorAngle.AccessibleRole = System.Windows.Forms.AccessibleRole.SplitButton;
            this.label_LogMotorAngle.AutoSize = true;
            this.label_LogMotorAngle.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_LogMotorAngle.Location = new System.Drawing.Point(12, 62);
            this.label_LogMotorAngle.Name = "label_LogMotorAngle";
            this.label_LogMotorAngle.Size = new System.Drawing.Size(144, 24);
            this.label_LogMotorAngle.TabIndex = 10;
            this.label_LogMotorAngle.Text = "MotorAngle：";
            // 
            // label_LogPower
            // 
            this.label_LogPower.AccessibleRole = System.Windows.Forms.AccessibleRole.SplitButton;
            this.label_LogPower.AutoSize = true;
            this.label_LogPower.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_LogPower.Location = new System.Drawing.Point(64, 30);
            this.label_LogPower.Name = "label_LogPower";
            this.label_LogPower.Size = new System.Drawing.Size(92, 24);
            this.label_LogPower.TabIndex = 11;
            this.label_LogPower.Text = "Power：";
            // 
            // groupBox_OperationalResults
            // 
            this.groupBox_OperationalResults.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox_OperationalResults.Controls.Add(this.label_LogPower);
            this.groupBox_OperationalResults.Controls.Add(this.label_LogMotorAngle);
            this.groupBox_OperationalResults.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox_OperationalResults.Location = new System.Drawing.Point(201, 195);
            this.groupBox_OperationalResults.Name = "groupBox_OperationalResults";
            this.groupBox_OperationalResults.Size = new System.Drawing.Size(356, 94);
            this.groupBox_OperationalResults.TabIndex = 13;
            this.groupBox_OperationalResults.TabStop = false;
            this.groupBox_OperationalResults.Text = "運算結果";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::PLC_Control.Properties.Resources.forklift__1_;
            this.pictureBox2.Location = new System.Drawing.Point(201, 17);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(572, 162);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 44;
            this.pictureBox2.TabStop = false;
            // 
            // Log_Debug
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.groupBox_OperationalResults);
            this.Controls.Add(this.lblDataIndex);
            this.Controls.Add(this.groupBox_Log);
            this.Controls.Add(this.txtDataIndex);
            this.Controls.Add(this.btnReadData);
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(this.btnLogBack);
            this.Name = "Log_Debug";
            this.Size = new System.Drawing.Size(789, 504);
            this.groupBox_Log.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Log)).EndInit();
            this.groupBox_OperationalResults.ResumeLayout(false);
            this.groupBox_OperationalResults.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLogBack;
        private System.Windows.Forms.Label lblDataIndex;
        private System.Windows.Forms.GroupBox groupBox_Log;
        private System.Windows.Forms.DataGridView dataGridView_Log;
        private System.Windows.Forms.TextBox txtDataIndex;
        private System.Windows.Forms.Button btnReadData;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn btnMachine;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.Label label_LogMotorAngle;
        private System.Windows.Forms.Label label_LogPower;
        private System.Windows.Forms.GroupBox groupBox_OperationalResults;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}
