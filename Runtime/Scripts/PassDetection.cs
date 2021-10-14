using System.Collections;
using UnityEngine;

namespace Balltracking.Scripts
{
    public class PassDetection : MonoBehaviour
    {
        [Header("Prefabs")] 
        [SerializeField] GameObject _soccerBall;

        [Header("Tracked Objects")] 
        [SerializeField] ViveMotionTracker _tracker;
        [SerializeField] Transform _foot;
        [SerializeField] Transform _passTarget;
        
        /// <summary>
        /// Put the tracker on the ground at the origin of the space and call the function
        /// </summary>
        public void CalibrateOrigin() => _calibrationOffset = _foot.transform.position;

        void Start() => _movingAverage = new MovingAverage();
    
        void Update() => _foot.transform.position = _tracker.GetPosition() - _calibrationOffset;
        
        void FixedUpdate() => DetectKick();

        void DetectKick()
        {
            _footPosition = _foot.transform.position;
            var deltaFootPosition = _footPosition - _previousFootPosition;

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
                StartCoroutine(ResetPassAfter(currentBall, 3f));
            }

            _previousFootPosition = _footPosition;
        }

        IEnumerator ResetPassAfter(GameObject objectToDestroy, float delay)
        {
            yield return new WaitForSeconds(delay);
        
            Destroy(objectToDestroy);
            _isReadyForPass = true;
        }
        void OnDrawGizmos()
        {
            Gizmos.DrawWireMesh(
            _passTarget.GetComponent<MeshFilter>().sharedMesh,
            _passTarget.transform.position,
            _passTarget.transform.rotation,
            _passTarget.transform.localScale);
        }

        Vector3 _passTargetDirection;
        Vector3 _previousFootPosition;
        Vector3 _footPosition;
        MovingAverage _movingAverage;
        bool _isReadyForPass = true;

        Vector3 _calibrationOffset;
    }
}