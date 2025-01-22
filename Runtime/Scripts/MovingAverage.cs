using System.Collections.Generic;
using UnityEngine;

namespace Balltracking
{
	public class MovingAverage
	{
		public MovingAverage(int windowSize)
		{
			_windowSize = windowSize;
		}

		public float Average { get; private set; }

		public MovingAverage Add(float newSample)
		{
			sampleAccumulator += newSample;
			_samples.Enqueue(newSample);

			if (_samples.Count > _windowSize)
				sampleAccumulator -= _samples.Dequeue();

			Average = sampleAccumulator / _samples.Count;

			return this;
		}

		float sampleAccumulator;
		readonly int _windowSize = 5;
		Queue<float> _samples = new();
	}

	public class MovingAverageVector
	{
		public MovingAverageVector(int windowSize)
		{
			_windowSize = windowSize;
		}

		public Vector3 Average { get; private set; }

		public MovingAverageVector Add(Vector3 newSample)
		{
			sampleAccumulator += newSample;
			_samples.Enqueue(newSample);

			if (_samples.Count > _windowSize)
				sampleAccumulator -= _samples.Dequeue();

			Average = sampleAccumulator / _samples.Count;

			return this;
		}

		Vector3 sampleAccumulator;
		readonly int _windowSize = 5;
		Queue<Vector3> _samples = new();
	}


}