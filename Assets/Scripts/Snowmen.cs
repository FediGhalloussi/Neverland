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
    public GameObject player;
    private MeshRenderer meshRenderer;
    private OVRSemanticClassification floor;
    private Vector3 initialPosition;
    private float heightSnowman;

    private GameObject middleEyeAnchor;

    
    private PirateGameActiver activer;

    void Start()
    {
        numberOfHitsSnowman = 0;
        activer = FindObjectOfType<PirateGameActiver>();
        //player = GameObject.FindGameObjectWithTag("Player");
        middleEyeAnchor = GameObject.FindGameObjectWithTag("MainCamera");
        //nextGame = GameObject.FindGameObjectWithTag("PirateGame");
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
        //transform.position = GetInitialPosition();
        //transform.position = new Vector3(0f, 0f, 5f);

        Vector3 initialPosition = GetInitialPositionFrontOfObstacles();
        this.transform.position = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z);
        this.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, /*transform.localPosition.z*/ 1f);
        Debug.Log("Snowman spawned  at : " + initialPosition);
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
        Debug.DrawRay(player.transform.position+ GameManager.Instance.floorNormal * heightSnowman * .5f, player.transform.forward * 100f, Color.green, 1f);
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

    private Vector3 GetInitialPosition2()
    {
        if (floor == null)
        {
            floor = GameManager.Instance.floor;
        }
        
        OVRScenePlane plane = floor.GetComponent<OVRScenePlane>();
        Vector2 dimension = plane.Dimensions;
        this.transform.localPosition = new Vector3(Random.Range(dimension.x / 2f - dimension.x,dimension.x/2f),
            Random.Range(dimension.y / 2f - dimension.y, dimension.y / 2f),
            0);
        return transform.localPosition;
    }
    
    private Vector3 GetInitialPositionFrontOfObstacles()
    {
        float maxDistance = 0;
        Vector3 furthestPosition = Vector3.zero;
        float radius = 1f; // radius of the sphere cast

        for (float theta = 0; theta < 360; theta += 10) // increment theta by 10 degrees each time
        {
            float phi = 0; // start phi at 0 each time
                // convert spherical coordinates to cartesian coordinates
                Vector3 direction = new Vector3(
                    Mathf.Cos(theta * Mathf.Deg2Rad) * Mathf.Cos(phi * Mathf.Deg2Rad),
                    Mathf.Sin(phi * Mathf.Deg2Rad),
                    Mathf.Sin(theta * Mathf.Deg2Rad) * Mathf.Cos(phi * Mathf.Deg2Rad)
                );
                Debug.DrawRay(player.transform.position, direction * 100f, Color.green, 10000f);

                RaycastHit hit;
                if (Physics.Raycast(player.transform.position, direction, out hit, 1000f))
                {
                    float distance = Vector3.Distance(player.transform.position, hit.point);
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        furthestPosition = hit.point;
                    }
                }
        }

        return furthestPosition;
    }
    
    void Attack()
    {
        Vector3 directionSnowman = (player.transform.position - transform.position).normalized;
        transform.position += new Vector3(directionSnowman.x * Time.deltaTime * speedSnowman,directionSnowman.y * Time.deltaTime * speedSnowman*0f,directionSnowman.z * Time.deltaTime * speedSnowman);
        // look at the player changing only the x axis
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
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
            Vector3 directionSnowman = GetInitialPositionFrontOfObstacles();
            snowman.transform.localPosition = new Vector3(directionSnowman.x, .5f, directionSnowman.z);
            //snowman.transform.position = new Vector3(0f, 1f, 5f);
        }
    }

    void GameWon()
    {
        Debug.Log("Mini game successful!");
        
        FindObjectOfType<ParticleSystemShapeFitter>().gameObject.SetActive(false);
        var chests = FindObjectsOfType<Chest>();

        foreach (var chest in chests)
        {
            chest.NextObject();
        }
        
        GameManager.Instance.currentObjectIndex++;
        
        var snowGround  = FindObjectsOfType<OVRSemanticClassification>()
            .Where(c => c.Contains(OVRSceneManager.Classification.Floor))
            .ToArray()[0].gameObject.transform.GetChild(2);
        snowGround.gameObject.SetActive(false);
        
        Destroy(gameObject); //destroy the snowman bc game is finished
    }

    void GameOver()
    {
        Start(); //game starts again
    }
}