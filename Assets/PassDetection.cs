using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassDetection : MonoBehaviour
{
    [Header("Prefabs")] 
    [SerializeField] GameObject _soccerBall;

    [Header("Tracked Objects")] 
    [SerializeField] Transform _foot;
    [SerializeField] Transform _passTarget;

    void Start() => _movingAverage = new MovingAverage();
    
    void FixedUpdate()
    {
        _footPosition = _foot.transform.position;
        var deltaFootPosition =  _footPosition - _previousFootPosition;
        
        var passTargetPosition = _passTarget.transform.position;
        _passTargetDirection = passTargetPosition - deltaFootPosition;

        var footMovementInTargetDirection = Vector3.Project(deltaFootPosition, _passTargetDirection).z;

        _movingAverage.Add(footMovementInTargetDirection);
        
        var velocityMeterPerSecond = _movingAverage.Average / Time.fixedDeltaTime;
        Debug.Log(velocityMeterPerSecond);

        
        if (velocityMeterPerSecond > 1 && _isReadyForPass)
        {
            print("Kick");
            var currentBall = Instantiate(_soccerBall, _footPosition, Quaternion.identity);
            var rigidBody = currentBall.GetComponent<Rigidbody>();
            rigidBody.AddRelativeForce(deltaFootPosition / Time.fixedDeltaTime, ForceMode.VelocityChange);
            _isReadyForPass = false;
            StartCoroutine(ResetPass(currentBall, 3f));
        }
        
        _previousFootPosition = _footPosition;
    }

    IEnumerator ResetPass(GameObject objectToDestroy, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        Destroy(objectToDestroy);
        _isReadyForPass = true;
    }

    Vector3 _passTargetDirection;
    Vector3 _previousFootPosition;
    Vector3 _footPosition;
    MovingAverage _movingAverage;
    bool _isReadyForPass = true;
}

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
