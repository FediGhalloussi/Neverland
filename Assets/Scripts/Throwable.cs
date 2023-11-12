using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    List<Vector3> trackingPos = new List<Vector3>();
    public float velocity = 1000f;
    bool pickedUp = false;
    GameObject parentHand;
    Rigidbody rb;
    Collider collider;
    
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (pickedUp == true)
        {
            rb.useGravity = false;
            transform.position=parentHand.transform.position;
            transform.rotation=parentHand.transform.rotation;
        }
        if (trackingPos.Count > 15)
        {
            trackingPos.RemoveAt(0);
        }
        trackingPos.Add(transform.position);

        float triggerRight = OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger);
        if (triggerRight < 0.1f)
        {
            pickedUp = false;
            Vector3 direction = trackingPos[trackingPos.Count - 1]-trackingPos[0];
            rb.AddForce(direction*velocity);
            rb.useGravity = true;
            rb.isKinematic = false;
            collider.isTrigger = false;
        }
    }
    private void OnTriggerEnter (Collider other)
    {
        float triggerRight = OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger);
        if (other.gameObject.tag == "Hand" && triggerRight > 0.9f)
        {
            pickedUp = true;
            parentHand=other.gameObject;
        }
    }
}