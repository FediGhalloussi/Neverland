using Meta.WitAi;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private GameObject cannonballPrefab;
    [SerializeField] private Transform[] cannonTransforms;
    [SerializeField] private float cannonballSpeed;
    [SerializeField] private float cannonballLifetime;
    public float shootingInterval = 2f;
    [SerializeField] private Transform player;
    private float timer;
    [SerializeField] private Animator anim_Hook;

    private void Start()
    {
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
        // Get a random cannon
        Transform cannon = cannonTransforms[Random.Range(0, cannonTransforms.Length)];

        if (Random.Range(0, 2) == 0)
        {
            FindObjectOfType<AudioManager>().Play("canon1");
        }

        else if (Random.Range(0, 1) == 0)
        {
            FindObjectOfType<AudioManager>().Play("canon2");
        }

        else
        {
            FindObjectOfType<AudioManager>().Play("canon3");
        }

        GameObject cannonball = Instantiate(cannonballPrefab, cannon.position, Quaternion.identity);
        cannonball.GetComponent<Rigidbody>().velocity =
            (player.position - cannonball.transform.position).normalized * cannonballSpeed;
        Destroy(cannonball, cannonballLifetime);
    }
    
    private void GameWon()
    {
        FindObjectOfType<AudioManager>().Play("victory_bell");
        CancelInvoke();
        Debug.Log("player has won");
        anim_Hook.Play("captain");
        Destroy(this);
    }

    public void GameOver()
    {
        FindObjectOfType<AudioManager>().Play("lose_sound");
        timer = 30;
    }
}
