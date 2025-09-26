using NAudio.Dsp;
using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ZaoApp
{
	public partial class MainForm : Form
	{
		private IAudioGenerator generator;
		private System.Windows.Forms.Timer timer;

		public MainForm()
		{
			InitializeComponent();
			LoadInputDevices();
		}

		private void LoadInputDevices()
		{
			for (int i = 0; i < WaveIn.DeviceCount; i++)
			{
				var caps = WaveIn.GetCapabilities(i);
				cmbJkDevices.Items.Add($"{i}. {caps.ProductName}");
			}
			if (cmbJkDevices.Items.Count > 0)
				cmbJkDevices.SelectedIndex = 0;
		}

		private void OnTimerTick(object sender, EventArgs e)
		{
			DateTime now = DateTime.Now;
			DateTime startDate = DateTime.Parse(Properties.Settings.Default.开始日期);
			DateTime endDate = DateTime.Parse(Properties.Settings.Default.结束日期);
			string duan = Properties.Settings.Default.时间段;
			List<List<string>> dayPeriods = JsonConvert.DeserializeObject<List<List<string>>>(duan);

			if (now > startDate && now < endDate)
			{
				List<string> timePeriods = dayPeriods[0];

				if (dayPeriods.Count == 7)
				{
					int dayOfWeek = (int)now.DayOfWeek;

					dayOfWeek--;
					dayOfWeek = dayOfWeek < 0 ? 6 : dayOfWeek;
					timePeriods = dayPeriods[dayOfWeek];
				}

				for (int j = 0; j < timePeriods.Count; j++)
				{
					string[] timeSplits = timePeriods[j].Split('-');
					DateTime startTime = DateTime.Parse(now.ToString("yyyy-MM-dd") + " " + timeSplits[0]);
					DateTime endTime = DateTime.Parse(now.ToString("yyyy-MM-dd") + " " + timeSplits[1]);

					if (now > startTime && now < endTime)
					{
						generator.Play();
						return;
					}
				}
			}

			generator.Stop();
		}

		private void btnNoiseStart_Click(object sender, EventArgs e)
		{
			if (cbxNoiseTiming.Checked)
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

			if (cbmNoiseType.SelectedItem == "鼾声")
			{
				generator = new RealSnoreGenerator();
			}
			else
			{
				generator = new PureToneGenerator();
			}

			generator.Play();
		}

		private void btnNoiseStop_Click(object sender, EventArgs e)
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

		private WaveInEvent waveIn;
		private volatile bool isMonitoring = false;
		private static readonly LogFileHelper log = new LogFileHelper("noise", 5 * 1024 * 1024);

		private void btnJkStart_Click(object sender, EventArgs e)
		{
			StartMonitoring();
		}

		private void btnJkStop_Click(object sender, EventArgs e)
		{
			StopMonitoring();
		}

		private void StartMonitoring()
		{
			if (isMonitoring) return;

			try
			{
				// 先设置标志再启动设备
				isMonitoring = true;

				waveIn = new WaveInEvent
				{
					DeviceNumber = cmbJkDevices.SelectedIndex,
					WaveFormat = new WaveFormat(44100, 1)
				};
				waveIn.DataAvailable += OnDataAvailable;
				waveIn.StartRecording();

				btnJkStart.Enabled = false;
				btnJkStop.Enabled = true;

				UpdateStatus($"监测中 - 日志: {log.GetCurrentLogFile()}");
				log.AppendLog("监测开始...");
			}
			catch (Exception ex)
			{
				isMonitoring = false;
				UpdateStatus($"启动失败: {ex.Message}");
				log.AppendLog($"启动失败: {ex.Message}");
			}
		}

		private void StopMonitoring()
		{
			if (!isMonitoring) return;

			// 先停止设备再修改标志
			waveIn?.StopRecording();

			// 设置标志阻止回调继续处理
			isMonitoring = false;

			// 安全释放资源
			if (waveIn != null)
			{
				waveIn.DataAvailable -= OnDataAvailable;
				waveIn.Dispose();
				waveIn = null;
			}

			btnJkStart.Enabled = true;
			btnJkStop.Enabled = false;
			UpdateStatus($"监测已停止 - 最后日志: {log.GetCurrentLogFile()}");
			log.AppendLog("监测已停止");
		}

		private void OnDataAvailable(object sender, WaveInEventArgs e)
		{
			if (!isMonitoring) return; // 检查监控状态

			try
			{
				// 计算分贝值
				double overallDb = CalculateDb(e.Buffer, e.BytesRecorded);
				double lowFreqDb = CalculateLowFrequencyDb(e.Buffer, e.BytesRecorded);

				string timeStamp = DateTime.Now.ToString("HH:mm:ss.fff");
				string message = $"{timeStamp}\t\t{overallDb:F1}\t\t{lowFreqDb:F1}";

				// 线程安全更新UI和日志
				this.BeginInvoke((Action)(() =>
				{
					if (isMonitoring) // 再次检查状态
					{
						txtJkLog.AppendText(message + Environment.NewLine);
						log.AppendLog(message);
					}
				}));
			}
			catch (Exception ex)
			{
				this.BeginInvoke((Action)(() =>
					log.AppendLog($"处理错误: {ex.Message}")));
			}
		}

		// 全带宽分贝计算 (相当于声压级 RMS)
		private double CalculateDb(byte[] buffer, int bytesRecorded)
		{
			if (buffer == null || buffer.Length == 0 || bytesRecorded <= 0 || bytesRecorded > buffer.Length)
				return 0;

			int sampleCount = bytesRecorded / 2;
			if (sampleCount == 0) return 0;

			double sumSquares = 0;

			for (int i = 0; i < bytesRecorded; i += 2)
			{
				short sample = BitConverter.ToInt16(buffer, i);
				double normalized = sample / 32768.0; // 转换到 -1.0 ~ +1.0
				sumSquares += normalized * normalized;
			}

			// RMS
			double rms = Math.Sqrt(sumSquares / sampleCount);

			// 转换为 dBFS (full scale)
			double dbFS = 20 * Math.Log10(Math.Max(rms, double.Epsilon));

			// 将 dBFS 转换为近似 dB SPL（需要校准）
			const double CalibrationOffset = 90.0; // 校准偏移，可调，例如让静音环境 ≈ 30dB
			double dbSPL = dbFS + CalibrationOffset;

			return Math.Max(0, Math.Round(dbSPL, 1));
		}

		// 低频 (20–200Hz) 分贝计算
		private double CalculateLowFrequencyDb(byte[] buffer, int bytesRecorded)
		{
			int sampleCount = bytesRecorded / 2;
			if (sampleCount == 0) return 0;

			// 转换为 float 数组
			float[] samples = new float[sampleCount];
			for (int i = 0; i < sampleCount; i++)
			{
				short sample = BitConverter.ToInt16(buffer, i * 2);
				samples[i] = sample / 32768f;
			}

			// FFT
			int fftLength = 2048; // 提高频率分辨率
			Complex[] fftBuffer = new Complex[fftLength];

			// Hann 窗，减少频谱泄露
			for (int i = 0; i < fftLength; i++)
			{
				if (i < samples.Length)
				{
					double window = 0.5 * (1 - Math.Cos(2 * Math.PI * i / (fftLength - 1)));
					fftBuffer[i].X = (float)(samples[i] * window);
				}
				else
				{
					fftBuffer[i].X = 0;
				}
				fftBuffer[i].Y = 0;
			}

			FastFourierTransform.FFT(true, (int)Math.Log(fftLength, 2.0), fftBuffer);

			// 采样率
			double sampleRate = 44100.0;
			double binSize = sampleRate / fftLength;

			int binStart = (int)(20 / binSize);
			int binEnd = (int)(200 / binSize);

			double sumSquares = 0;
			int count = 0;

			for (int i = binStart; i <= binEnd; i++)
			{
				double mag = Math.Sqrt(fftBuffer[i].X * fftBuffer[i].X + fftBuffer[i].Y * fftBuffer[i].Y);
				sumSquares += mag * mag;
				count++;
			}

			double rms = Math.Sqrt(sumSquares / Math.Max(1, count));

			// 转换为 dBFS
			double dbFS = 20 * Math.Log10(Math.Max(rms, double.Epsilon));

			// 校准为 dB SPL
			const double CalibrationOffset = 90.0;
			double dbSPL = dbFS + CalibrationOffset;

			return Math.Max(0, Math.Round(dbSPL, 1));
		}

		private void UpdateStatus(string message)
		{
			lblStatus.Text = $"状态: {message}";
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			StopMonitoring();
			base.OnFormClosing(e);
		}
	}
}
