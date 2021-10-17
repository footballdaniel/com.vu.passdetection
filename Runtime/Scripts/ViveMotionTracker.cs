using System;
using UnityEngine;
using Valve.VR;

namespace Balltracking.Scripts
{
    [Serializable]
    public class ViveMotionTracker : MonoBehaviour 
    {
        public Vector3 Position =>  _trackedObject.transform.position;
        
        [SerializeField] SteamVR_TrackedObject.EIndex _device = SteamVR_TrackedObject.EIndex.Device1;

        void Awake() => InstantiateTracker();

        void InstantiateTracker()
        {
            _trackedObject = gameObject.AddComponent<SteamVR_TrackedObject>();
            _trackedObject.index = _device;
        }

        SteamVR_TrackedObject _trackedObject;
    }
}