using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZaoApp
{
	/// <summary>
	/// 电动车式震撼报警音（兼容 NAudio 2.2.1）
	/// 
	/// 播放默认“电动车被碰报警音”
	/// AlertAlarmPlayer.Play();
	/// 或者更长、更响的版本
	/// AlertAlarmPlayer.Play(duration: 4f, volume: 1.0f);
	/// 
	/// </summary>
	public static class AlertAlarmPlayer
	{
		/// <summary>
		/// 播放“被碰触式”报警音
		/// </summary>
		/// <param name="duration">持续时间（秒）</param>
		/// <param name="volume">音量 0~1</param>
		public static void Play(float duration = 3.0f, float volume = 0.9f)
		{
			int sampleRate = 44100;
			int channels = 1;

			// 创建音频流提供器
			var provider = new AlarmSampleProvider(sampleRate, duration, volume);

			using (var waveOut = new WaveOutEvent())
			{
				waveOut.Init(provider);
				waveOut.Play();
				Thread.Sleep((int)(duration * 1000));
			}
		}
	}

	/// <summary>
	/// 生成电动车报警音的样本提供器（方波+正弦波+脉冲调制）
	/// </summary>
	internal class AlarmSampleProvider : ISampleProvider
	{
		private readonly int sampleRate;
		private readonly int totalSamples;
		private readonly float volume;
		private int sampleIndex = 0;

		// 声音参数
		private const float BaseFreq = 90f;      // 低频共振
		private const float HighFreq = 1200f;    // 高频尖啸
		private const float PulseRate = 4f;      // 哔哔哔频率
		private const float DutyCycle = 0.4f;    // 每个脉冲持续比例

		public AlarmSampleProvider(int sampleRate, float duration, float volume)
		{
			this.sampleRate = sampleRate;
			this.totalSamples = (int)(sampleRate * duration);
			this.volume = volume;
			WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, 1);
		}

		public WaveFormat WaveFormat { get; }

		public int Read(float[] buffer, int offset, int count)
		{
			int samplesRemaining = totalSamples - sampleIndex;
			int samplesToGenerate = Math.Min(samplesRemaining, count);

			for (int i = 0; i < samplesToGenerate; i++)
			{
				float t = (float)sampleIndex / sampleRate;

				// 高频方波（主报警声）
				float highTone = Math.Sign(Math.Sin(2 * Math.PI * HighFreq * t));

				// 低频正弦（厚重底音）
				float lowTone = (float)Math.Sin(2 * Math.PI * BaseFreq * t);

				// 脉冲调制（间歇性“哔哔哔”）
				float phase = (t * PulseRate) % 1.0f;
				float gate = (phase < DutyCycle) ? 1.0f : 0.0f;

				// 包络衰减（防止结尾爆音）
				float env = (float)Math.Exp(-5f * (float)sampleIndex / totalSamples);

				buffer[offset + i] = (highTone * 0.7f + lowTone * 0.4f) * gate * env * volume;
				sampleIndex++;
			}

			return samplesToGenerate;
		}
	}
}
