using UnityEngine;

public class Snowmen : MonoBehaviour
{
    [SerializeField] private GameObject snowman;
    [SerializeField] private float speedSnowman;
    private int numberOfHitsSnowman;
    private GameObject player;
    private MeshRenderer meshRenderer;

    void Start()
    {
        numberOfHitsSnowman = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = true;
        transform.position = new Vector3(0, 0.5f, 5);
    }

    private void Update()
    {
        Attack();
    }

    void Attack()
    {
        Vector3 directionSnowman = (player.transform.position - snowman.transform.position).normalized;
        transform.position += directionSnowman * Time.deltaTime * speedSnowman;
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger detected with " + other.name);
        if (other.gameObject.CompareTag("Snowball"))
        {
            Debug.Log("Snowman hit");
            IsHit();
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Game over");
            GameOver();
        }
    }

    void IsHit()
    {
        numberOfHitsSnowman += 1;
        Debug.Log(numberOfHitsSnowman + " snowman hit");
        if (numberOfHitsSnowman == 3)
        {
            GameWon();
        }
        else
        {
            snowman.transform.position = new Vector3(2, 2, 5);
        }
    }

    void GameWon()
    {
        Debug.Log("Mini game successful!");
    }

    void GameOver()
    {
        meshRenderer.enabled = false;
        Invoke("Start", 3.0f);
    }
}