using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject cannonballPrefab;
    public float cannonballSpeed = 10f;
    public float shootingInterval = 2f;

    private void Start()
    {
        // Start shooting cannonballs at regular intervals
        InvokeRepeating("ShootCannonball", 0f, shootingInterval);
    }

    private void ShootCannonball()
    {
        // Instantiate a new cannonball
        GameObject cannonball = Instantiate(cannonballPrefab, transform.position, Quaternion.identity);

        // Get the Rigidbody component of the cannonball
        Rigidbody rb = cannonball.GetComponent<Rigidbody>();

        // Shoot the cannonball in the forward direction with specified speed
        rb.velocity = transform.forward * cannonballSpeed;

        // You can add more customization to the cannonball here if needed
    }
}
