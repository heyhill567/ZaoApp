namespace ZaoApp
{
	partial class MainForm
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.btnNoiseStart = new System.Windows.Forms.Button();
			this.cbmNoiseType = new System.Windows.Forms.ComboBox();
			this.btnNoiseStop = new System.Windows.Forms.Button();
			this.cmbJkDevices = new System.Windows.Forms.ComboBox();
			this.btnJkStop = new System.Windows.Forms.Button();
			this.btnJkStart = new System.Windows.Forms.Button();
			this.gbNoiseMaker = new System.Windows.Forms.GroupBox();
			this.rbSuiji = new System.Windows.Forms.RadioButton();
			this.rbPeizhi = new System.Windows.Forms.RadioButton();
			this.cbxNoiseTiming = new System.Windows.Forms.CheckBox();
			this.gbJk = new System.Windows.Forms.GroupBox();
			this.lblStatus = new System.Windows.Forms.Label();
			this.txtJkLog = new System.Windows.Forms.TextBox();
			this.txtPeriodStart = new System.Windows.Forms.TextBox();
			this.txtPeriodEnd = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.gbNoiseMaker.SuspendLayout();
			this.gbJk.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnNoiseStart
			// 
			this.btnNoiseStart.Location = new System.Drawing.Point(364, 24);
			this.btnNoiseStart.Name = "btnNoiseStart";
			this.btnNoiseStart.Size = new System.Drawing.Size(166, 23);
			this.btnNoiseStart.TabIndex = 0;
			this.btnNoiseStart.Text = "开始";
			this.btnNoiseStart.UseVisualStyleBackColor = true;
			this.btnNoiseStart.Click += new System.EventHandler(this.btnNoiseStart_Click);
			// 
			// cbmNoiseType
			// 
			this.cbmNoiseType.FormattingEnabled = true;
			this.cbmNoiseType.Items.AddRange(new object[] {
            "重度鼾声",
            "电锯型鼾声",
            "低频"});
			this.cbmNoiseType.Location = new System.Drawing.Point(18, 24);
			this.cbmNoiseType.Name = "cbmNoiseType";
			this.cbmNoiseType.Size = new System.Drawing.Size(322, 23);
			this.cbmNoiseType.TabIndex = 1;
			this.cbmNoiseType.Text = "电锯型鼾声";
			// 
			// btnNoiseStop
			// 
			this.btnNoiseStop.Location = new System.Drawing.Point(364, 53);
			this.btnNoiseStop.Name = "btnNoiseStop";
			this.btnNoiseStop.Size = new System.Drawing.Size(166, 23);
			this.btnNoiseStop.TabIndex = 2;
			this.btnNoiseStop.Text = "停止";
			this.btnNoiseStop.UseVisualStyleBackColor = true;
			this.btnNoiseStop.Click += new System.EventHandler(this.btnNoiseStop_Click);
			// 
			// cmbJkDevices
			// 
			this.cmbJkDevices.FormattingEnabled = true;
			this.cmbJkDevices.Location = new System.Drawing.Point(18, 24);
			this.cmbJkDevices.Name = "cmbJkDevices";
			this.cmbJkDevices.Size = new System.Drawing.Size(322, 23);
			this.cmbJkDevices.TabIndex = 4;
			// 
			// btnJkStop
			// 
			this.btnJkStop.Location = new System.Drawing.Point(364, 53);
			this.btnJkStop.Name = "btnJkStop";
			this.btnJkStop.Size = new System.Drawing.Size(166, 23);
			this.btnJkStop.TabIndex = 6;
			this.btnJkStop.Text = "停止";
			this.btnJkStop.UseVisualStyleBackColor = true;
			this.btnJkStop.Click += new System.EventHandler(this.btnJkStop_Click);
			// 
			// btnJkStart
			// 
			this.btnJkStart.Location = new System.Drawing.Point(364, 24);
			this.btnJkStart.Name = "btnJkStart";
			this.btnJkStart.Size = new System.Drawing.Size(166, 23);
			this.btnJkStart.TabIndex = 5;
			this.btnJkStart.Text = "开始";
			this.btnJkStart.UseVisualStyleBackColor = true;
			this.btnJkStart.Click += new System.EventHandler(this.btnJkStart_Click);
			// 
			// gbNoiseMaker
			// 
			this.gbNoiseMaker.Controls.Add(this.label1);
			this.gbNoiseMaker.Controls.Add(this.txtPeriodEnd);
			this.gbNoiseMaker.Controls.Add(this.txtPeriodStart);
			this.gbNoiseMaker.Controls.Add(this.rbSuiji);
			this.gbNoiseMaker.Controls.Add(this.rbPeizhi);
			this.gbNoiseMaker.Controls.Add(this.cbmNoiseType);
			this.gbNoiseMaker.Controls.Add(this.cbxNoiseTiming);
			this.gbNoiseMaker.Controls.Add(this.btnNoiseStart);
			this.gbNoiseMaker.Controls.Add(this.btnNoiseStop);
			this.gbNoiseMaker.Location = new System.Drawing.Point(12, 12);
			this.gbNoiseMaker.Name = "gbNoiseMaker";
			this.gbNoiseMaker.Size = new System.Drawing.Size(545, 152);
			this.gbNoiseMaker.TabIndex = 7;
			this.gbNoiseMaker.TabStop = false;
			this.gbNoiseMaker.Text = "生成";
			// 
			// rbSuiji
			// 
			this.rbSuiji.AutoSize = true;
			this.rbSuiji.Checked = true;
			this.rbSuiji.Location = new System.Drawing.Point(116, 88);
			this.rbSuiji.Name = "rbSuiji";
			this.rbSuiji.Size = new System.Drawing.Size(88, 19);
			this.rbSuiji.TabIndex = 5;
			this.rbSuiji.TabStop = true;
			this.rbSuiji.Text = "每日随机";
			this.rbSuiji.UseVisualStyleBackColor = true;
			// 
			// rbPeizhi
			// 
			this.rbPeizhi.AutoSize = true;
			this.rbPeizhi.Location = new System.Drawing.Point(17, 88);
			this.rbPeizhi.Name = "rbPeizhi";
			this.rbPeizhi.Size = new System.Drawing.Size(88, 19);
			this.rbPeizhi.TabIndex = 4;
			this.rbPeizhi.Text = "跟随配置";
			this.rbPeizhi.UseVisualStyleBackColor = true;
			// 
			// cbxNoiseTiming
			// 
			this.cbxNoiseTiming.AutoSize = true;
			this.cbxNoiseTiming.Checked = true;
			this.cbxNoiseTiming.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbxNoiseTiming.Location = new System.Drawing.Point(18, 57);
			this.cbxNoiseTiming.Name = "cbxNoiseTiming";
			this.cbxNoiseTiming.Size = new System.Drawing.Size(59, 19);
			this.cbxNoiseTiming.TabIndex = 3;
			this.cbxNoiseTiming.Text = "定时";
			this.cbxNoiseTiming.UseVisualStyleBackColor = true;
			// 
			// gbJk
			// 
			this.gbJk.Controls.Add(this.lblStatus);
			this.gbJk.Controls.Add(this.txtJkLog);
			this.gbJk.Controls.Add(this.cmbJkDevices);
			this.gbJk.Controls.Add(this.btnJkStop);
			this.gbJk.Controls.Add(this.btnJkStart);
			this.gbJk.Location = new System.Drawing.Point(12, 182);
			this.gbJk.Name = "gbJk";
			this.gbJk.Size = new System.Drawing.Size(545, 368);
			this.gbJk.TabIndex = 8;
			this.gbJk.TabStop = false;
			this.gbJk.Text = "监测";
			// 
			// lblStatus
			// 
			this.lblStatus.AutoSize = true;
			this.lblStatus.Location = new System.Drawing.Point(15, 336);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(37, 15);
			this.lblStatus.TabIndex = 8;
			this.lblStatus.Text = "状态";
			// 
			// txtJkLog
			// 
			this.txtJkLog.Location = new System.Drawing.Point(18, 99);
			this.txtJkLog.Multiline = true;
			this.txtJkLog.Name = "txtJkLog";
			this.txtJkLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtJkLog.Size = new System.Drawing.Size(512, 221);
			this.txtJkLog.TabIndex = 7;
			// 
			// txtPeriodStart
			// 
			this.txtPeriodStart.Location = new System.Drawing.Point(116, 113);
			this.txtPeriodStart.Name = "txtPeriodStart";
			this.txtPeriodStart.Size = new System.Drawing.Size(100, 25);
			this.txtPeriodStart.TabIndex = 6;
			this.txtPeriodStart.Text = "22:00";
			// 
			// txtPeriodEnd
			// 
			this.txtPeriodEnd.Location = new System.Drawing.Point(240, 113);
			this.txtPeriodEnd.Name = "txtPeriodEnd";
			this.txtPeriodEnd.Size = new System.Drawing.Size(100, 25);
			this.txtPeriodEnd.TabIndex = 7;
			this.txtPeriodEnd.Text = "06:00";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(219, 119);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(15, 15);
			this.label1.TabIndex = 9;
			this.label1.Text = "-";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(569, 566);
			this.Controls.Add(this.gbNoiseMaker);
			this.Controls.Add(this.gbJk);
			this.Name = "MainForm";
			this.Text = "噪";
			this.gbNoiseMaker.ResumeLayout(false);
			this.gbNoiseMaker.PerformLayout();
			this.gbJk.ResumeLayout(false);
			this.gbJk.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnNoiseStart;
		private System.Windows.Forms.ComboBox cbmNoiseType;
		private System.Windows.Forms.Button btnNoiseStop;
		private System.Windows.Forms.ComboBox cmbJkDevices;
		private System.Windows.Forms.Button btnJkStop;
		private System.Windows.Forms.Button btnJkStart;
		private System.Windows.Forms.GroupBox gbNoiseMaker;
		private System.Windows.Forms.GroupBox gbJk;
		private System.Windows.Forms.TextBox txtJkLog;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.RadioButton rbPeizhi;
		private System.Windows.Forms.RadioButton rbSuiji;
		private System.Windows.Forms.CheckBox cbxNoiseTiming;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtPeriodEnd;
		private System.Windows.Forms.TextBox txtPeriodStart;
	}
}

