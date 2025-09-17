using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaoApp
{
	public class PureToneGenerator : IAudioGenerator
	{
		private readonly WaveOutEvent waveOut;
		private bool isPlaying;
		private float volume = 1f;
		private readonly float targetFrequency = 100f; // 固定100Hz低频

		public PureToneGenerator()
		{
			waveOut = new WaveOutEvent();
			waveOut.Volume = volume;
		}

		public void Play()
		{
			if (isPlaying) return;

			isPlaying = true;
			var provider = new SineWaveProvider(targetFrequency, volume);

			waveOut.Init(provider);
			waveOut.Play();
		}

		public void Stop()
		{
			isPlaying = false;
			waveOut.Stop();
		}

		public void SetVolume(float vol)
		{
			volume = vol;
			waveOut.Volume = volume;
		}

		public void Dispose()
		{
			Stop();
			waveOut?.Dispose();
		}

		// 100Hz正弦波提供器
		private class SineWaveProvider : WaveProvider32
		{
			private float phase;
			private readonly float frequency;
			private readonly float amplitude;

			public SineWaveProvider(float freq, float amp)
			{
				frequency = freq;
				amplitude = amp;
			}

			public override int Read(float[] buffer, int offset, int sampleCount)
			{
				for (int i = 0; i < sampleCount; i++)
				{
					float sampleValue = amplitude * (float)Math.Sin(phase * 2 * Math.PI);

					buffer[offset + i] = sampleValue;
					phase += frequency / WaveFormat.SampleRate;

					if (phase > 1) phase -= 1;
				}

				return sampleCount;
			}
		}
	}
}
