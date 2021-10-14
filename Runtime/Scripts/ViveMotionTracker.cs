using System;
using UnityEngine;

namespace VROOM.Scripts
{
    [Serializable]
    public class ViveMotionTracker : MonoBehaviour 
    {
        [SerializeField] SteamVR_TrackedObject.EIndex _device = SteamVR_TrackedObject.EIndex.Device1;
        SteamVR_TrackedObject _trackedObject;

        void Awake() => InstantiateTracker();

        void InstantiateTracker()
        {
            _trackedObject = gameObject.AddComponent<SteamVR_TrackedObject>();
            _trackedObject.index = _device;
        }

        public Vector3 GetPosition() =>  _trackedObject.transform.position;
    }
}