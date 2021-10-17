using System.Collections.Generic;
using UnityEngine;

namespace Balltracking.Scripts
{
    public class MovingAverage  
    {
        public float Average { get; private set; }

        public MovingAverage Add(float newSample)
        {
            sampleAccumulator += newSample;
            samples.Enqueue(newSample);

            if (samples.Count > windowSize)
                sampleAccumulator -= samples.Dequeue();

            Average = sampleAccumulator / samples.Count;

            return this;
        }

        float sampleAccumulator;
        int windowSize = 5;
        Queue<float> samples = new Queue<float>();
    }
    
    public class MovingAverageVector
    {
        public Vector3 Average { get; private set; }

        public MovingAverageVector Add(Vector3 newSample)
        {
            sampleAccumulator += newSample;
            samples.Enqueue(newSample);

            if (samples.Count > windowSize)
                sampleAccumulator -= samples.Dequeue();

            Average = sampleAccumulator / samples.Count;

            return this;
        }

        Vector3 sampleAccumulator;
        int windowSize = 5;
        Queue<Vector3> samples = new Queue<Vector3>();
    }
    
    
}