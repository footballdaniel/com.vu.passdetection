using System;
using UnityEngine;

namespace Balltracking.Scripts
{
	[Serializable]
	public class ViveMotionTracker : MonoBehaviour
	{
		public Vector3 Position => transform.position;

	}
}