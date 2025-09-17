using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaoApp
{
	public static class MathExtensions
	{
		/// <summary>
		/// 将值限制在指定范围内
		/// </summary>
		/// <typeparam name="T">数值类型</typeparam>
		/// <param name="value">要限制的值</param>
		/// <param name="min">最小值</param>
		/// <param name="max">最大值</param>
		/// <returns>限制后的值</returns>
		public static T Clamp<T>(this T value, T min, T max) where T : IComparable<T>
		{
			if (min.CompareTo(max) > 0)
			{
				throw new ArgumentException("最小值不能大于最大值");
			}

			if (value.CompareTo(min) < 0)
			{
				return min;
			}
			else if (value.CompareTo(max) > 0)
			{
				return max;
			}
			return value;
		}
	}
}
