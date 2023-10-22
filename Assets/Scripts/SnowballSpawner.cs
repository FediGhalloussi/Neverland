using System;
using UnityEngine;

public class SnowballSpawner : MonoBehaviour
{
    public GameObject snowballPrefab;
    private bool hasInstantiatedSnowball = false;

    private void Update()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Hand") && OVRInput.Get(OVRInput.Button.PrimaryHandTrigger) && !hasInstantiatedSnowball)
            {
                // Instantiate the snowball prefab when the hand is close to the overlap box
                Instantiate(snowballPrefab, collider.transform.position, Quaternion.identity);
                hasInstantiatedSnowball = true;
            }
        }
        
        if (hasInstantiatedSnowball && !OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
        {
            hasInstantiatedSnowball = false;
        }
        
    }

    // private void OnCollisionEnter(Collision collision)
    // {
    //     Debug.Log("Collision detected with " + collision.collider.name);
    //     if (collision.collider.CompareTag("Hand"))
    //     {
    //         Debug.Log("Collision detected with hand");
    //         Instantiate(snowballPrefab, collision.transform.position, Quaternion.identity);
    //     }
    // }
    //
    // private void OnTriggerEnter(Collider other)
    // {
    //     Debug.Log("Trigger detected with " + other.name);
    //     if (other.CompareTag("Hand"))
    //     {
    //         Debug.Log("Trigger detected with hand");
    //         Instantiate(snowballPrefab, other.transform.position, Quaternion.identity);
    //     }
    // }
}
