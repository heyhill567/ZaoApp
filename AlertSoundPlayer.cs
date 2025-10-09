using NAudio.Wave.SampleProviders;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaoApp
{
	/// <summary>
	/// 震撼警示音工具类（NAudio 2.2.1）
	/// 
	/// 默认播放（低沉有力）
	/// AlertSoundPlayer.Play();

	/// 自定义音色（更低沉、更长、更强）
	/// AlertSoundPlayer.Play(duration: 1.2f, baseFreq: 90f, highFreq: 360f, volume: 1.0f);

	/// 高频警示（刺耳提醒）
	/// AlertSoundPlayer.Play(duration: 0.5f, baseFreq: 400f, highFreq: 1200f, volume: 0.7f);
	/// </summary>
	public static class AlertSoundPlayer
	{
		private static readonly int SampleRate = 44100;

		/// <summary>
		/// 播放震撼警示音
		/// </summary>
		/// <param name="duration">时长（秒）</param>
		/// <param name="baseFreq">低频主音（Hz）</param>
		/// <param name="highFreq">高频谐波（Hz）</param>
		/// <param name="volume">音量（0~1）</param>
		public static void Play(
			float duration = 0.8f,
			float baseFreq = 120f,
			float highFreq = 480f,
			float volume = 0.8f)
		{
			// 低频层：方波
			var lowTone = new SignalGenerator(SampleRate, 1)
			{
				Gain = volume * 0.6,
				Frequency = baseFreq,
				Type = SignalGeneratorType.Square
			};

			// 高频层：锯齿波
			var highTone = new SignalGenerator(SampleRate, 1)
			{
				Gain = volume * 0.3,
				Frequency = highFreq,
				Type = SignalGeneratorType.SawTooth
			};

			// 混音
			var mix = new MixingSampleProvider(new[] { lowTone, highTone });

			// 包络衰减（避免突然结束爆音）
			var envelope = new EnvelopeSampleProvider(mix, SampleRate, duration);

			// 播放到系统声卡
			using (var waveOut = new WaveOutEvent())
			{
				waveOut.Init(envelope);
				waveOut.Play();

				// 阻塞到播放结束
				int ms = (int)(duration * 1000);
				System.Threading.Thread.Sleep(ms);
			}
		}
	}

	/// <summary>
	/// 指数衰减包络
	/// </summary>
	internal class EnvelopeSampleProvider : ISampleProvider
	{
		private readonly ISampleProvider source;
		private readonly int totalSamples;
		private int sampleIndex = 0;

		public EnvelopeSampleProvider(ISampleProvider source, int sampleRate, float duration)
		{
			this.source = source;
			this.totalSamples = (int)(sampleRate * duration);
			WaveFormat = source.WaveFormat;
		}

		public WaveFormat WaveFormat { get; }

		public int Read(float[] buffer, int offset, int count)
		{
			int samples = source.Read(buffer, offset, count);
			for (int i = 0; i < samples; i++)
			{
				float t = (float)sampleIndex / totalSamples;
				float envelope = (float)(Math.Exp(-5 * t)); // 快速衰减包络
				buffer[offset + i] *= envelope;
				sampleIndex++;
			}
			return samples;
		}
	}
}
