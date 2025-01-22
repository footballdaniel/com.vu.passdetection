using UnityEngine;

namespace Balltracking
{
	public struct KickData
	{
		public float time;
		public Vector3 origin;
		public Vector3 direction;
		public float velocity;

		public KickData(Vector3 direction, Vector3 origin, float time, float velocity)
		{
			this.direction = direction;
			this.origin = origin;
			this.time = time;
			this.velocity = velocity;
		}
	}
}