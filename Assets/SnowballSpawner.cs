using System;
using UnityEngine;

public class SnowballSpawner : MonoBehaviour
{
    public GameObject snowballPrefab; // Définissez le prefab de la boule de neige dans l'inspecteur.

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with " + collision.collider.name);
        if (collision.collider.CompareTag("Hand"))
        {
            Debug.Log("Collision detected with hand");
            // Créez une nouvelle boule de neige à la position de collision.
            Instantiate(snowballPrefab, collision.transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger detected with " + other.name);
        if (other.CompareTag("Hand"))
        {
            Debug.Log("Trigger detected with hand");
            // Créez une nouvelle boule de neige à la position de collision.
            Instantiate(snowballPrefab, other.transform.position, Quaternion.identity);
        }
    }
}
