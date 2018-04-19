using System.Collections.Generic;
using Random = System.Random;

namespace Util
{
	public static class RandomUtil
	{
		private static Random r = new Random();
		
		public static T RandomElement<T>(List<T> list)
		{
			return list[r.Next(0, list.Count)];
		}

		public static bool RandomEvent(double probability)
		{
			return r.NextDouble() < probability;
		}

		public static int Next(int min, int max)
		{
			return r.Next(min, max);
		}
		
		public static float Next(float min, float max)
		{
			return (float) (min + r.NextDouble() * (max - min));
		}

		public static double Next(double min, double max)
		{
			return min + r.NextDouble() * (max - min);
		}
	}
}
