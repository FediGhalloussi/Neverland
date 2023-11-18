using System.Linq;
using UnityEngine;

public class Snowmen : MonoBehaviour
{
    [SerializeField] private GameObject snowman;
    [SerializeField] private float speedSnowman;
    private int numberOfHitsSnowman;
    private GameObject player;
    private MeshRenderer meshRenderer;
    private OVRSemanticClassification floor;

    void Start()
    {
        numberOfHitsSnowman = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = true;
        floor = FindObjectsOfType<OVRSemanticClassification>()
            .Where(c => c.Contains(OVRSceneManager.Classification.Floor))
            .ToArray()[0];
        transform.position = new Vector3(0, floor.transform.position.y + meshRenderer.bounds.size.y/2f, 5);
    }

    private void Update()
    {
        Attack();
    }

    void Attack()
    {
        Vector3 directionSnowman = (player.transform.position - snowman.transform.position).normalized;
        transform.position += directionSnowman * Time.deltaTime * speedSnowman;
        transform.position = new Vector3(transform.position.x, floor.transform.position.y + meshRenderer.bounds.size.y/2f, transform.position.z);
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