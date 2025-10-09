using NAudio.Wave;
using System;

namespace ZaoApp
{
	public class RealSnoreGenerator : IAudioGenerator
	{
		public enum SnoreType
		{
			Light,      // 轻柔的鼾声
			Moderate,   // 中等强度的鼾声
			Heavy,      // 沉重的鼾声
			Chainsaw    // 类似电锯的鼾声
		}

		private class SnoreProfile
		{
			public float BaseFreq { get; set; }         // 基础频率(Hz)
			public float FreqVariation { get; set; }    // 频率变化幅度
			public float NoiseLevel { get; set; }       // 噪声水平
			public float DurationMin { get; set; }      // 最短持续时间(秒)
			public float DurationMax { get; set; }      // 最长持续时间(秒)
			public float IntervalMin { get; set; }      // 最短间隔时间(秒)
			public float IntervalMax { get; set; }      // 最长间隔时间(秒)
			public float Resonance { get; set; }        // 共振强度
		}

		private readonly SnoreProfile[] profiles = new[]
		{
            // 轻度鼾声
            new SnoreProfile {
				BaseFreq = 180f, FreqVariation = 0.15f, NoiseLevel = 0.15f,
				DurationMin = 0.3f, DurationMax = 0.8f,
				IntervalMin = 2f, IntervalMax = 6f,
				Resonance = 0.1f
			},
            // 中度鼾声
            new SnoreProfile {
				BaseFreq = 120f, FreqVariation = 0.25f, NoiseLevel = 0.25f,
				DurationMin = 0.5f, DurationMax = 1.2f,
				IntervalMin = 1f, IntervalMax = 4f,
				Resonance = 0.2f
			},
            // 重度鼾声
            new SnoreProfile {
				BaseFreq = 80f, FreqVariation = 0.35f, NoiseLevel = 0.35f,
				DurationMin = 0.8f, DurationMax = 1.8f,
				IntervalMin = 0.5f, IntervalMax = 2.5f,
				Resonance = 0.3f
			},
            // 电锯型鼾声
            new SnoreProfile {
				BaseFreq = 60f, FreqVariation = 0.5f, NoiseLevel = 0.5f,
				DurationMin = 1f, DurationMax = 2.0f,
				IntervalMin = 1.5f, IntervalMax = 2.5f,
				Resonance = 0.4f
			}
		};
		private WaveOutEvent waveOut;
		private bool isPlaying;
		private float volume = 0.5f;
		private SnoreType currentType = SnoreType.Chainsaw;
		private ContinuousSnoreWaveProvider continuousProvider;

		public RealSnoreGenerator()
		{
			waveOut = new WaveOutEvent();
			continuousProvider = new ContinuousSnoreWaveProvider(profiles[(int)currentType], volume);
			waveOut.Init(continuousProvider);
		}

		public void Play()
		{
			if (isPlaying) return;

			isPlaying = true;
			waveOut.Play();
		}

		public void Stop()
		{
			isPlaying = false;
			waveOut.Stop();
		}

		public void SetVolume(float vol)
		{
			volume = vol.Clamp(0f, 1f);
			continuousProvider.Volume = volume;
		}

		public void SetSnoreType(SnoreType type)
		{
			currentType = type;
			continuousProvider.UpdateProfile(profiles[(int)currentType]);
		}

		public void Dispose()
		{
			Stop();
			waveOut?.Dispose();
		}

		private class ContinuousSnoreWaveProvider : ISampleProvider
		{
			private enum State
			{
				FadeIn,      // 渐入阶段
				Snoring,     // 鼾声阶段
				FadeOut,     // 渐出阶段
				Silence      // 静音阶段
			}

			private float phase;
			private float modPhase;
			private readonly Random rand;
			private float currentFreq;
			private float volume;
			private SnoreProfile currentProfile;
			private float snoreSamplesRemaining;
			private float silenceSamplesRemaining;
			private State currentState;
			private const float FadeDuration = 0.15f; // 淡入淡出时长(秒)
			private int fadeSamples;
			private int currentFadeSamples;

			// 改进的噪声生成
			private float[] noiseBuffer;
			private int noisePosition;
			private const int NoiseBufferSize = 44100; // 1秒的噪声缓存
			private float noiseFilterState;

			public WaveFormat WaveFormat { get; } = WaveFormat.CreateIeeeFloatWaveFormat(44100, 1);

			public ContinuousSnoreWaveProvider(SnoreProfile profile, float volume)
			{
				this.currentProfile = profile;
				this.volume = volume;
				rand = new Random();
				fadeSamples = (int)(FadeDuration * WaveFormat.SampleRate);
				currentState = State.Silence;

				// 初始化噪声生成器
				noiseBuffer = new float[NoiseBufferSize];
				GenerateSmoothNoise();

				StartNewCycle();
			}

			public void UpdateProfile(SnoreProfile profile)
			{
				currentProfile = profile;
			}

			public float Volume
			{
				get => volume;
				set => volume = value;
			}

			private void GenerateSmoothNoise()
			{
				// 使用滤波噪声生成更自然的气流声
				float cutoff = 0.1f; // 低通滤波截止频率

				for (int i = 0; i < NoiseBufferSize; i++)
				{
					float whiteNoise = (float)(rand.NextDouble() * 2 - 1);

					noiseFilterState += cutoff * (whiteNoise - noiseFilterState);
					noiseBuffer[i] = noiseFilterState * 0.6f; // 降低噪声幅度
				}
			}

			private void StartNewCycle()
			{
				// 计算本次鼾声持续时间和间隔时间
				float duration = currentProfile.DurationMin +
					(float)rand.NextDouble() * (currentProfile.DurationMax - currentProfile.DurationMin);

				float interval = currentProfile.IntervalMin +
					(float)rand.NextDouble() * (currentProfile.IntervalMax - currentProfile.IntervalMin);

				snoreSamplesRemaining = duration * WaveFormat.SampleRate;
				silenceSamplesRemaining = interval * WaveFormat.SampleRate;

				// 随机生成基础频率（±10%变化）
				currentFreq = currentProfile.BaseFreq * (1 + (float)(rand.NextDouble() - 0.7) * 0.1f);

				// 重置相位
				phase = 0;
				modPhase = 0;

				// 刷新噪声缓存
				if (rand.NextDouble() < 0.3) // 30%概率刷新噪声
				{
					GenerateSmoothNoise();
				}

				// 开始淡入
				currentState = State.FadeIn;
				currentFadeSamples = 0;
			}

			public int Read(float[] buffer, int offset, int count)
			{
				for (int i = 0; i < count; i++)
				{
					float sampleValue = 0f;

					switch (currentState)
					{
						case State.FadeIn:
							// 计算淡入增益 (使用缓动曲线)
							float fadeInProgress = (float)currentFadeSamples / fadeSamples;
							float fadeInGain = (float)Math.Pow(fadeInProgress, 0.5); // 缓入效果

							sampleValue = GenerateSnoreSample(fadeInGain);

							currentFadeSamples++;
							if (currentFadeSamples >= fadeSamples)
							{
								currentState = State.Snoring;
							}
							break;

						case State.Snoring:
							// 生成正常鼾声
							sampleValue = GenerateSnoreSample(1f);

							snoreSamplesRemaining--;
							if (snoreSamplesRemaining <= fadeSamples)
							{
								// 准备淡出
								currentState = State.FadeOut;
								currentFadeSamples = 0;
							}
							break;

						case State.FadeOut:
							// 计算淡出增益 (使用缓动曲线)
							float fadeOutProgress = (float)currentFadeSamples / fadeSamples;
							float fadeOutGain = 1f - (float)Math.Pow(fadeOutProgress, 0.8); // 缓出效果

							sampleValue = GenerateSnoreSample(fadeOutGain);

							currentFadeSamples++;
							snoreSamplesRemaining--;

							if (currentFadeSamples >= fadeSamples || snoreSamplesRemaining <= 0)
							{
								currentState = State.Silence;
							}
							break;

						case State.Silence:
							// 静音
							sampleValue = 0f;
							silenceSamplesRemaining--;

							if (silenceSamplesRemaining <= 0)
							{
								StartNewCycle();
								i--; // 重新处理这个样本
								continue;
							}
							break;
					}

					buffer[offset + i] = sampleValue;
				}
				return count;
			}


			private float prevIn = 0f, prevOut = 0f; // 鼻腔滤波器状态

			private float BandPass(float input, float centerFreq, float q)
			{
				float w0 = 2 * (float)Math.PI * centerFreq / WaveFormat.SampleRate;
				float alpha = (float)Math.Sin(w0) / (2 * q);

				float b0 = alpha;
				float b1 = 0;
				float b2 = -alpha;
				float a0 = 1 + alpha;
				float a1 = -2 * (float)Math.Cos(w0);
				float a2 = 1 - alpha;

				float output = (b0 / a0) * input + (b1 / a0) * prevIn + (b2 / a0) * prevOut
							   - (a1 / a0) * prevIn - (a2 / a0) * prevOut;

				prevOut = prevIn;
				prevIn = output;
				return output;
			}

			private float RandomFreqBiased(float min = 100f, float max = 500f)
			{
				float r = (float)Math.Sqrt(rand.NextDouble()); // 偏向小值
				return min + r * (max - min);
			}

			private float GenerateSnoreSample(float envelope)
			{
				// --- 频率调制 ---
				float freqMod = 1 + currentProfile.FreqVariation *
								(float)Math.Sin(modPhase * 0.25f); // 慢速抖动
				float currentEffectiveFreq = currentFreq * freqMod;

				// --- 主振荡器 (粗糙感) ---
				float sine = (float)Math.Sin(phase * 2 * Math.PI);
				float saw = 2f * (phase - (float)Math.Floor(phase + 0.5f)); // 锯齿波
				float square = phase < 0.5f ? 1f : -1f;                      // 方波
				float mainOsc = sine * 0.7f + saw * 0.2f + square * 0.1f;

				// --- 鼻腔共鸣滤波 ---
				mainOsc = BandPass(mainOsc, RandomFreqBiased(100f, 250f), 2f); // 带通，Q=2

				// --- 获取平滑噪声 (随呼吸) ---
				if (++noisePosition >= NoiseBufferSize)
					noisePosition = 0;
				float noise = noiseBuffer[noisePosition] * currentProfile.NoiseLevel;
				noise *= (0.8f + 0.2f * (float)rand.NextDouble());

				// --- 包络 (自然呼吸曲线) ---
				float progress = 1 - snoreSamplesRemaining / (snoreSamplesRemaining + fadeSamples);
				float attack = (float)Math.Pow(Math.Min(progress * 4f, 1), 0.6);     // 起音快
				float release = (float)Math.Pow(1 - Math.Max(0, (progress - 0.5f) * 2f), 1.5); // 释音慢
				float env = attack * release * envelope;

				// --- 呼吸断顿 (tremolo) ---
				float tremolo = 1f - 0.15f * (float)Math.Sin(2 * Math.PI * modPhase * 5); // 5Hz 抖动

				// --- 混合输出 ---
				float sampleValue = volume * env * (mainOsc * 0.8f + noise * 0.2f) * tremolo;

				// --- 更新相位 ---
				phase += currentEffectiveFreq / WaveFormat.SampleRate;
				modPhase += 0.1f / WaveFormat.SampleRate; // 更慢的调制

				if (phase > 1) phase -= 1;

				// --- 防止削波 ---
				return sampleValue.Clamp(-0.98f, 0.98f);
			}
		}
	}
}