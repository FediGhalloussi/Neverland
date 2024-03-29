using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    List<Vector3> trackingPos = new List<Vector3>();
    public float velocity = 1000f;
    public bool pickedUpRight = false;
    public bool pickedUpLeft = false;
    GameObject parentHand;
    Rigidbody rb;
    Collider collider;
    public string targetTag = "Snowman";
    public float aimAssistStrength = 0.2f; 
    public float angleThreshold = 15f;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (pickedUpRight || pickedUpLeft)
        {
            rb.useGravity = false;
            transform.position = parentHand.transform.position;
            transform.rotation = parentHand.transform.rotation;
        }

        if (trackingPos.Count > 15)
        {
            trackingPos.RemoveAt(0);
        }
        trackingPos.Add(transform.position);
        
        
        float triggerRight = OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger);
        if (triggerRight < 0.1f && pickedUpRight)
        {
            pickedUpRight = false;

            // Calculate the release direction without aim assist
            Vector3 releaseDirection = (trackingPos[trackingPos.Count - 1] - trackingPos[0]).normalized;

            // Find all objects with the specified tag
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(targetTag);

            foreach (GameObject obj in objectsWithTag)
            {
                // Calculate the direction towards each object
                Vector3 targetDirection = (obj.transform.position - transform.position).normalized;

                // Check if the angle between release direction and target direction is within the threshold
                float angle = Vector3.Angle(releaseDirection, targetDirection);
                if (angle < angleThreshold)
                {
                    // Apply aim assist to the release direction
                    releaseDirection = Vector3.Lerp(releaseDirection, targetDirection, aimAssistStrength);
                }
            }

            // Add haptic feedback when releasing the object
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);

            // Release the object with the calculated release direction and velocity and multiple the velocity by the velocity of controller
            rb.velocity = releaseDirection * velocity * OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch).magnitude;
            rb.useGravity = true;
            rb.isKinematic = false;
            collider.isTrigger = false;
        }
        
        float triggerLeft = OVRInput.Get(OVRInput.RawAxis1D.LHandTrigger);
        if (triggerLeft < 0.1f && pickedUpLeft)
        {
            pickedUpLeft = false;

            // Calculate the release direction without aim assist
            Vector3 releaseDirection = (trackingPos[trackingPos.Count - 1] - trackingPos[0]).normalized;

            // Find all objects with the specified tag
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(targetTag);

            foreach (GameObject obj in objectsWithTag)
            {
                // Calculate the direction towards each object
                Vector3 targetDirection = (obj.transform.position - transform.position).normalized;

                // Check if the angle between release direction and target direction is within the threshold
                float angle = Vector3.Angle(releaseDirection, targetDirection);
                if (angle < angleThreshold)
                {
                    // Apply aim assist to the release direction
                    releaseDirection = Vector3.Lerp(releaseDirection, targetDirection, aimAssistStrength);
                }
            }

            // Add haptic feedback when releasing the object
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);

            // Release the object with the calculated release direction and velocity and multiple the velocity by the velocity of controller
            rb.velocity = releaseDirection * velocity * OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch).magnitude;
            rb.useGravity = true;
            rb.isKinematic = false;
            collider.isTrigger = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        float triggerRight = OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger);
        if (other.gameObject.tag == "RightHand" && triggerRight > 0.9f)
        {
            pickedUpRight = true;
            parentHand = other.gameObject;
        }
        
        float triggerLeft = OVRInput.Get(OVRInput.RawAxis1D.LHandTrigger);
        if (other.gameObject.tag == "LeftHand" && triggerLeft > 0.9f)
        {
            pickedUpLeft = true;
            parentHand = other.gameObject;
        }
    }
}
