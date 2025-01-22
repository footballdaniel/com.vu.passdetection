using System;
using System.Collections;
using Balltracking.Scripts;
using UnityEngine;

namespace Balltracking
{
	public class PassDetectionController : MonoBehaviour
	{
		[Header("Prefabs"), SerializeField] 
        	GameObject _soccerBall;

		[Header("Tracked Objects"), SerializeField] 
        	ViveMotionTracker _tracker;

		[SerializeField] Transform _foot;
		[SerializeField] Transform _passTarget;

		public static event Action<KickData> OnDetectPass;

		/// <summary>
		/// Put the tracker on the ground at the origin of the space and call the function
		/// </summary>
		public void CalibrateOrigin()
		{
			_calibrationOffset = _foot.transform.position;
		}

		void Awake()
		{
			var filterDuration_seconds = 0.1f;
			var filterWindowSize_frames = Mathf.CeilToInt(filterDuration_seconds / Time.fixedDeltaTime);

			_filterKickSignal = new MovingAverage(filterWindowSize_frames);
			_filterFootPosition = new MovingAverageVector(filterWindowSize_frames);
			_timeSeries = new TimeSeries();
		}

		void Update()
		{
			_foot.transform.position = _tracker.Position - _calibrationOffset;
		}

		void FixedUpdate()
		{
			CheckForKick();
		}

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
				WaitForMaxKickVelocity(velocityMeterPerSecond);

			_previousFootPosition = _footPosition;
		}

		void WaitForMaxKickVelocity(float velocityMeterPerSecond)
		{
			_timeSeries.Add(velocityMeterPerSecond);

			var velocityMultiplier = 3f;

			if (_timeSeries.Decreasing)
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
				print($"Have waited for {_timeSeries.Duration} and kicked at {_timeSeries.Value}");

				var currentBall = Instantiate(_soccerBall, _footPosition, Quaternion.identity);
				var rigidBody = currentBall.GetComponent<Rigidbody>();
				rigidBody.AddRelativeForce(velocityMultiplier * kickDirection / Time.fixedDeltaTime, ForceMode.VelocityChange);
				StartCoroutine(ResetBallAfter(currentBall, 3f));

				_timeSeries = new TimeSeries();
				_isReadyForPass = false;
			}
		}

		IEnumerator ResetBallAfter(GameObject objectToDestroy, float delay)
		{
			yield return new WaitForSeconds(delay);
			Destroy(objectToDestroy);
			_isReadyForPass = true;
		}

		Vector3 _passTargetDirection;
		Vector3 _previousFootPosition;
		Vector3 _footPosition;
		TimeSeries _timeSeries;
		MovingAverage _filterKickSignal;
		MovingAverageVector _filterFootPosition;
		bool _isReadyForPass = true;
		Vector3 _calibrationOffset;
	}

	public class TimeSeries
	{
		public bool Decreasing => _currentValue < _previousValue;
		public bool Increasing => _currentValue > _previousValue;
		public int Duration => _index;
		public float Value => _currentValue;

		public void Add(float newValue)
		{
			_previousValue = _currentValue;
			_currentValue = newValue;
			_index += 1;
		}

		int _index;
		float _previousValue;
		float _currentValue;
	}


}