using System;
using UnityEngine;

public class SnowballSpawner : MonoBehaviour
{
    public GameObject snowballPrefab;
    public float offsetFloor = 5f;
    public bool hasInstantiatedSnowball = false;

    private void Start()
    {
        Vector3 normal = GetComponentInParent<OVRScenePlane>().gameObject.transform.forward;
        gameObject.transform.position = GetComponentInParent<OVRScenePlane>().gameObject.transform.position + normal * offsetFloor;
        Debug.Log("Snowball spawner ON");
    }
    private void Update()
    {
        Debug.Log("input detected " + (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger) || OVRInput.Get(OVRInput.Button.SecondaryHandTrigger)));
        if (hasInstantiatedSnowball && !(OVRInput.Get(OVRInput.Button.PrimaryHandTrigger) || OVRInput.Get(OVRInput.Button.SecondaryHandTrigger)))
        {
            hasInstantiatedSnowball = false;
        }
        
        Debug.Log("hasInstantiatedSnowball " + hasInstantiatedSnowball);
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Hand") && (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger) || OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))  && !hasInstantiatedSnowball)
        {
            // Instantiate the snowball prefab when the hand is close to the overlap box
            Instantiate(snowballPrefab, collision.collider.transform.position, Quaternion.identity);
            hasInstantiatedSnowball = true;
            Debug.Log("Spawning snowball !");

        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Trigger detected with " + other.name);
        if (other.CompareTag("Hand") && (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger) || OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))  && !hasInstantiatedSnowball)
        {
            Debug.Log("Trigger detected with hand");
            Instantiate(snowballPrefab, other.transform.position, Quaternion.identity);
            hasInstantiatedSnowball = true;
            Debug.Log("Spawning snowball !");


        }
    }
}
