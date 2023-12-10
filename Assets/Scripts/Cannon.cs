using Meta.WitAi;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject cannonballPrefab;
    public float cannonballSpeed = 10f;
    public float shootingInterval = 2f;
    [SerializeField] float cannonballLifetime = 3f;
    Vector3 fromCannonToPlayer;
    private GameObject player;
    private float timer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        timer = 30;
        // Start shooting cannonballs at regular intervals
        InvokeRepeating("ShootCannonball", 0f, shootingInterval);
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer-=Time.deltaTime;
        }
        else
        {
            GameWon();
        }
    }

    private void ShootCannonball()
    {
        // Instantiate a new cannonball
        GameObject cannonball = Instantiate(cannonballPrefab, transform.position, Quaternion.identity);
        fromCannonToPlayer = player.transform.position - cannonball.transform.position;

        // Get the Rigidbody component of the cannonball
        Rigidbody rb = cannonball.GetComponent<Rigidbody>();

        // Shoot the cannonball in the forward direction with specified speed
        rb.velocity = fromCannonToPlayer * cannonballSpeed;

        Destroy(cannonball, cannonballLifetime);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger detected with " + other.name);
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Game over");
            GameOver();
        }
    }

    private void GameWon()
    {
        CancelInvoke();
        Debug.Log("player has won");
    }

    private void GameOver()
    {
        timer = 30;
    }
}
