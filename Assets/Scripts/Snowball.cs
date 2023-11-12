using System;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    [SerializeField] private GameObject snowman;

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(null))
        {
            Destroy(gameObject);
        }
    }*/


    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision detected with " + other.collider.name);
        if (!other.collider.CompareTag("Hand") && !other.collider.CompareTag("Snowball") && !other.collider.CompareTag("Player"))
        {
            Debug.Log("Collision detected with other than hand " + other.collider.name);
            Destroy(gameObject);
        }
    }
}