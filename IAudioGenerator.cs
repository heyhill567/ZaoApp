using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IAudioGenerator : IDisposable
{
	/// <summary>播放音频</summary>
	void Play();

	/// <summary>停止播放</summary>
	void Stop();

	/// <summary>设置音量 (0.0~1.0)</summary>
	void SetVolume(float volume);
}
