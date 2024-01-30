using System.Linq;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    private bool gameSnowStarted = false;
    private bool gameCannonStarted = false;

    public OVRSemanticClassification floor;
    public OVRSemanticClassification ceiling;
    
    public Vector3 floorNormal;
    
    //Chest related variables
    public int currentObjectIndex = 0;
    public bool chestOpenable = true;
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
    
    
}