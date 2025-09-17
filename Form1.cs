using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZaoApp
{
	public partial class Form1 : Form
	{
		private IAudioGenerator generator;
		private System.Windows.Forms.Timer timer;

		public Form1()
		{
			InitializeComponent();
		}

		private void OnTimerTick(object sender, EventArgs e)
		{
			DateTime now = DateTime.Now;
			DateTime startDate = DateTime.Parse(Properties.Settings.Default.开始日期);
			DateTime endDate = DateTime.Parse(Properties.Settings.Default.结束日期);
			string[] timePeriods = Properties.Settings.Default.时间段.Split(',');
			int durationDays = (int)Math.Ceiling(endDate.Subtract(startDate).TotalDays);


			for (int i = 0; i < durationDays; i++)
			{
				DateTime targetTime = startDate.AddDays(i);

				for (int j = 0; j < timePeriods.Length; j++)
				{
					string[] timeSplits = timePeriods[j].Split('-');
					DateTime startTime = DateTime.Parse(targetTime.ToString("yyyy-MM-dd") + " " + timeSplits[0]);
					DateTime endTime = DateTime.Parse(targetTime.ToString("yyyy-MM-dd") + " " + timeSplits[1]);

					if (now > startTime && now < endTime)
					{
						generator.Play();
						return;
					}
				}
			}

			generator.Stop();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (cbTiming.Checked)
			{
				if (timer == null)
				{
					timer = new System.Windows.Forms.Timer();
					timer.Interval = 1000;
					timer.Tick += OnTimerTick;
				}

				timer.Stop();
				timer.Start();
			}
			else
			{
				if (timer != null)
				{
					timer.Stop();
					timer.Dispose();
					timer = null;
				}
			}

			if (generator != null)
			{
				generator.Dispose();
			}

			if (cbType.SelectedItem == "鼾声")
			{
				generator = new RealSnoreGenerator();
			}
			else
			{
				generator = new PureToneGenerator();
			}

			generator.Play();
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			if (timer != null)
			{
				timer.Stop();
			}

			if (generator != null)
			{
				generator.Stop();
			}
		}
	}
}
