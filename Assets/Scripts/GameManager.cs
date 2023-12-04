using System.Linq;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    private bool gameSnowStarted = false;
    private bool gameCannonStarted = false;

    public OVRSemanticClassification floor;
    public OVRSemanticClassification ceiling;
    
    public Vector3 floorNormal;
    public bool GameSnowStarted
    {
        get { return gameSnowStarted; }
        set { gameSnowStarted = value; }
    }


    public bool GameCannonStarted
    {
        get { return gameCannonStarted; }
        set { gameCannonStarted = value; }
    }
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
    }
    
    private void Update()
    {
        
        // debug raycast normal floor
        Debug.DrawRay(floor.transform.position, GameManager.Instance.floorNormal * 100f, Color.red, 10000f);
        Debug.Log("normalfloor " + GameManager.Instance.floorNormal);
        
    }
}