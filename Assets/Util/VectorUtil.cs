using UnityEngine;

namespace Util
{
	public static class VectorUtil 
	{
		public static Vector2 Vector2(float angle, float magnitude)
		{
			return new Vector2(Mathf.Cos(angle) * magnitude, Mathf.Sin(angle) * magnitude);
		}

		public static short AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 thirdDimension)
		{
			Vector3 perp = Vector3.Cross(fwd, targetDir);
			float dir = Vector3.Dot(perp, thirdDimension);

			if (dir > 0f) return 1;
			if (dir < 0f) return -1;
			return 0;
		}
	}
}
