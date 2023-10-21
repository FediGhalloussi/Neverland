using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManagerSnowball : MonoBehaviour
{
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private float step;

    
    // Update is called once per frame
    void Update()
    { 
        step = 5.0f * Time.deltaTime;
        if (OVRInput.Get(OVRInput.RawButton.RThumbstickLeft)) transform.Rotate(0, 5.0f * step, 0);
        if (OVRInput.Get(OVRInput.RawButton.RThumbstickRight)) transform.Rotate(0, -5.0f * step, 0);
        if (OVRInput.GetUp(OVRInput.Button.One))
        {
            OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);
        }
        if ((OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.0f || OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0.0f))
        {
            transform.position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            transform.rotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
        }
    }
}
