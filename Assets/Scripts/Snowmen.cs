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

    private GameObject middleEyeAnchor;

    void Start()
    {
        numberOfHitsSnowman = 0;
        //player = GameObject.FindGameObjectWithTag("Player");
        middleEyeAnchor = GameObject.FindGameObjectWithTag("MainCamera");
        player = middleEyeAnchor;
        floor = FindObjectsOfType<OVRSemanticClassification>()
            .Where(c => c.Contains(OVRSceneManager.Classification.Floor))
            .ToArray()[0];
        if (floor == null)
        {
            Debug.Log("1");
            floor = GameManager.Instance.floor;
        }
        heightSnowman = GetComponent<CapsuleCollider>().height; //height of the snowman
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
        //instead of 5f, send raycast to the direction of the player right and put distance to the first wall (layer mask wall)
        RaycastHit hit;
        if (floor == null)
        {
            Debug.Log("2");
            floor = GameManager.Instance.floor;
        }
        //todo change player forward to where camera is looking
        Debug.DrawRay(player.transform.position+ GameManager.Instance.floorNormal * heightSnowman * .5f, player.transform.forward * 100f, Color.green, 10000f);
        if (Physics.Raycast(player.transform.position+ GameManager.Instance.floorNormal * heightSnowman * .5f, player.transform.forward, out hit, 100f, LayerMask.GetMask("Wall")))
        {
            Debug.DrawRay(player.transform.position + GameManager.Instance.floorNormal * heightSnowman * .5f, player.transform.forward * hit.distance, Color.yellow, 10000f);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(player.transform.position, player.transform.right * 100f, Color.white);
            Debug.Log("Did not Hit");
            return initialPosition;
        }
        //todo add random range to the right
        initialPosition = player.transform.position + player.transform.forward * (hit.distance - .1f) +
                          player.transform.right * Random.Range(-0f, 0f) + GameManager.Instance.floorNormal * heightSnowman * .5f;
        return initialPosition;
    }
    
    void Attack()
    {
        Vector3 directionSnowman = (player.transform.position - transform.position).normalized;
        transform.position += directionSnowman * Time.deltaTime * speedSnowman;
        if (Vector3.Distance(transform.position, player.transform.position)<0.2f)
        {
            GameOver();
        }
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
            //snowman.transform.position = GetInitialPosition();
            snowman.transform.position = new Vector3(0f, 1f, 5f);
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