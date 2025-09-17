namespace ZaoApp
{
	partial class Form1
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
			this.button1 = new System.Windows.Forms.Button();
			this.cbType = new System.Windows.Forms.ComboBox();
			this.btnStop = new System.Windows.Forms.Button();
			this.cbTiming = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(376, 24);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(166, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "开始";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// cbType
			// 
			this.cbType.FormattingEnabled = true;
			this.cbType.Items.AddRange(new object[] {
            "鼾声",
            "低频"});
			this.cbType.Location = new System.Drawing.Point(24, 24);
			this.cbType.Name = "cbType";
			this.cbType.Size = new System.Drawing.Size(166, 23);
			this.cbType.TabIndex = 1;
			this.cbType.Text = "鼾声";
			// 
			// btnStop
			// 
			this.btnStop.Location = new System.Drawing.Point(376, 53);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(166, 23);
			this.btnStop.TabIndex = 2;
			this.btnStop.Text = "停止";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// cbTiming
			// 
			this.cbTiming.AutoSize = true;
			this.cbTiming.Checked = true;
			this.cbTiming.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbTiming.Location = new System.Drawing.Point(24, 57);
			this.cbTiming.Name = "cbTiming";
			this.cbTiming.Size = new System.Drawing.Size(59, 19);
			this.cbTiming.TabIndex = 3;
			this.cbTiming.Text = "定时";
			this.cbTiming.UseVisualStyleBackColor = true;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(569, 101);
			this.Controls.Add(this.cbTiming);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.cbType);
			this.Controls.Add(this.button1);
			this.Name = "Form1";
			this.Text = "噪";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ComboBox cbType;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.CheckBox cbTiming;
	}
}

