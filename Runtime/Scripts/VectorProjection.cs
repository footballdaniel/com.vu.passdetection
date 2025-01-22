using UnityEngine;

namespace Balltracking.Scripts
{
	public class VectorProjection
	{
		readonly Vector3 _movementVector;

		public VectorProjection(Vector3 movementVector)
		{
			_movementVector = movementVector;
		}

		public Vector3 InDirection(Vector3 directionVector)
		{
			return Vector3.Project(_movementVector, directionVector);
		}
	}
}