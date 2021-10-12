using UnityEngine;

public class CalibrateSpace : MonoBehaviour
{
    [SerializeField] Transform _trackedCalibrationObject;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            Calibrate();
    }

    void Calibrate()
    {
        
    }
}
