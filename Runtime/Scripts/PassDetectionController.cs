using System;
using System.Collections;
using UnityEngine;

namespace Balltracking.Scripts
{
    public class PassDetectionController : MonoBehaviour
    {
        [Header("Prefabs")] 
        [SerializeField] GameObject _soccerBall;

        [Header("Tracked Objects")] 
        [SerializeField] ViveMotionTracker _tracker;
        [SerializeField] Transform _foot;
        [SerializeField] Transform _passTarget;

        public static event Action<KickData> OnDetectPass;
        
        /// <summary>
        /// Put the tracker on the ground at the origin of the space and call the function
        /// </summary>
        public void CalibrateOrigin() => _calibrationOffset = _foot.transform.position;

        void Awake()
        {
            _filterKickSignal = new MovingAverage();
            _filterFootPosition = new MovingAverageVector();
        }

        void Update() => _foot.transform.position = _tracker.Position - _calibrationOffset;
        
        void FixedUpdate() => CheckForKick();

        void CheckForKick()
        {
            _footPosition = _foot.transform.position;
            var deltaFootPosition = _footPosition - _previousFootPosition;

            var passTargetPosition = _passTarget.transform.position;
            _passTargetDirection = passTargetPosition - deltaFootPosition;

            var footMovementInTargetDirection = Vector3.Project(deltaFootPosition, _passTargetDirection).z;

            _filterKickSignal.Add(footMovementInTargetDirection);
            _filterFootPosition.Add(deltaFootPosition);

            var velocityMeterPerSecond = _filterKickSignal.Average / Time.fixedDeltaTime;

            if (velocityMeterPerSecond > 2 && _isReadyForPass)
            {
                var kickDirection = _filterFootPosition.Average;
                
                var kickData = new KickData()
                {
                    time = Time.deltaTime,
                    origin = _footPosition,
                    direction = kickDirection,
                    velocity = velocityMeterPerSecond
                };
                
                OnDetectPass?.Invoke(kickData);
                
                var currentBall = Instantiate(_soccerBall, _footPosition, Quaternion.identity);
                var rigidBody = currentBall.GetComponent<Rigidbody>();
                rigidBody.AddRelativeForce(kickDirection / Time.fixedDeltaTime, ForceMode.VelocityChange);
                _isReadyForPass = false;
                StartCoroutine(ResetBallAfter(currentBall, 3f));
            }

            _previousFootPosition = _footPosition;
        }

        IEnumerator ResetBallAfter(GameObject objectToDestroy, float delay)
        {
            yield return new WaitForSeconds(delay);
        
            Destroy(objectToDestroy);
            _isReadyForPass = true;
        }
        void OnDrawGizmos() => Gizmos.DrawWireSphere(_passTarget.transform.position, 0.5f);

        Vector3 _passTargetDirection;
        Vector3 _previousFootPosition;
        Vector3 _footPosition;
        MovingAverage _filterKickSignal;
        MovingAverageVector _filterFootPosition;
        bool _isReadyForPass = true;
        Vector3 _calibrationOffset;
    }
}