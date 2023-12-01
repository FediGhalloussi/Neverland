using System.Collections.Generic;
using System.Linq;
using Unity.XR.CoreUtils;
using UnityEngine;

public class Snowmen : MonoBehaviour
{
    [SerializeField] private GameObject snowman;
    [SerializeField] private GameObject magnifyingGlassPrefab;
    [SerializeField] private float speedSnowman;
    private List<GameObject> children;
    private int numberOfHitsSnowman;
    private GameObject player;
    private MeshRenderer meshRenderer;
    private OVRSemanticClassification floor;
    private Vector3 initialPosition;
    private float heightSnowman;

    void Start()
    {
        numberOfHitsSnowman = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = true;
        floor = FindObjectsOfType<OVRSemanticClassification>()
            .Where(c => c.Contains(OVRSceneManager.Classification.Floor))
            .ToArray()[0];
        heightSnowman = meshRenderer.bounds.size.y; //height of the snowman
        //initialPosition = floor.GetComponentInChildren<SnowballSpawner>().transform.position + floor.GetComponentInChildren<SnowballSpawner>().transform.forward * heightSnowman/2f ;
        //initial pos is 10 meter in front of the player with a random angle between -30 and 30 degrees around player forward
        //initialPosition = player.transform.position + player.transform.forward * 5f + floor.transform.forward *  heightSnowman;
        transform.position = GetInitialPosition();
    }

    private void Update()
    {
        Attack();
    }

    
    private Vector3 GetInitialPosition()
    {
        return player.transform.position + player.transform.forward * 5f + player.transform.right * Random.Range(-10f, 10f) + floor.GetComponentInChildren<SnowballSpawner>().transform.forward * heightSnowman * 1f;
    }
    
    void Attack()
    {
        Vector3 directionSnowman = (player.transform.position - transform.position).normalized;
        transform.position += directionSnowman * Time.deltaTime * speedSnowman;
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
    
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision detected with " + other.collider.name);
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
            snowman.transform.position = initialPosition;
        }
    }

    void GameWon()
    {
        Debug.Log("Mini game successful!");
        var chests = GameObject.FindGameObjectsWithTag("Chest");
        foreach (var chest in chests)
        {
            chest.GetChildGameObjects(children);
            foreach (var child in children)
            {
                Destroy(child);
            }
        }
        Destroy(this); //destroy the snowman bc game is finished
    }

    void GameOver()
    {
        meshRenderer.enabled = false;
        Invoke("Start", 3.0f); //game starts again
    }
}