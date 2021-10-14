using System.Collections.Generic;

namespace Balltracking.Scripts
{
    public class MovingAverage  
    {
        public float Average { get; private set; }

        public void Add(float newSample)
        {
            sampleAccumulator += newSample;
            samples.Enqueue(newSample);

            if (samples.Count > windowSize)
                sampleAccumulator -= samples.Dequeue();

            Average = sampleAccumulator / samples.Count;
        }

        float sampleAccumulator;
        int windowSize = 5;
        Queue<float> samples = new Queue<float>();
    }
}