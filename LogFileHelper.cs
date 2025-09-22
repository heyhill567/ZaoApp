using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaoApp
{
	public class LogFileHelper
	{
		private readonly string logFileBaseName;
		private readonly long maxFileSize; // 单位: bytes
		private int currentLogIndex;
		private string currentLogFile;

		public LogFileHelper(string baseName, long maxSizeBytes)
		{
			logFileBaseName = baseName;
			maxFileSize = maxSizeBytes;
			currentLogIndex = 0;
			currentLogFile = GetLastLogFile();
		}

		/// <summary>
		/// 获取当前日志文件（如果超出大小则生成新文件）
		/// </summary>
		public string GetCurrentLogFile()
		{
			if (string.IsNullOrEmpty(currentLogFile) || IsFileExceedLimit(currentLogFile))
			{
				currentLogIndex++;
				currentLogFile = BuildLogFileName(currentLogIndex);
			}
			return currentLogFile;
		}

		/// <summary>
		/// 追加写入日志
		/// </summary>
		public void AppendLog(string message)
		{
			string file = GetCurrentLogFile();
			File.AppendAllText(file, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {message}{Environment.NewLine}");
		}

		/// <summary>
		/// 判断文件是否超过大小限制
		/// </summary>
		private bool IsFileExceedLimit(string file)
		{
			FileInfo fi = new FileInfo(file);
			return fi.Exists && fi.Length >= maxFileSize;
		}

		/// <summary>
		/// 构建日志文件名
		/// </summary>
		private string BuildLogFileName(int index)
		{
			return $"{logFileBaseName}_{index}_{DateTime.Now:yyyyMMdd}.txt";
		}

		/// <summary>
		/// 找到最后一个已存在的日志文件
		/// </summary>
		private string GetLastLogFile()
		{
			string todayPattern = $"{logFileBaseName}_*_{DateTime.Now:yyyyMMdd}.txt";
			string dir = AppDomain.CurrentDomain.BaseDirectory;
			string[] files = Directory.GetFiles(dir, todayPattern);

			if (files.Length == 0)
			{
				currentLogIndex = 1;
				return BuildLogFileName(currentLogIndex);
			}

			// 找到最大 index 的文件
			int maxIndex = 0;
			foreach (var file in files)
			{
				string name = Path.GetFileNameWithoutExtension(file);
				string[] parts = name.Split('_');
				if (parts.Length >= 3 && int.TryParse(parts[1], out int idx))
				{
					if (idx > maxIndex) maxIndex = idx;
				}
			}

			currentLogIndex = maxIndex;
			string lastFile = BuildLogFileName(currentLogIndex);

			// 检查是否超过限制
			if (IsFileExceedLimit(lastFile))
			{
				currentLogIndex++;
				return BuildLogFileName(currentLogIndex);
			}

			return lastFile;
		}
	}
}
