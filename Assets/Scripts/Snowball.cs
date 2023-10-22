using System;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    [SerializeField] private GameObject snowman;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(null))
        {
            Destroy(gameObject);
        }
    }
}