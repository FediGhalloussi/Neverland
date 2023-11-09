using UnityEngine;
using System.Collections;
using System.Linq;

public class BoxesSpawner : MonoBehaviour
{
    [SerializeField] private GameObject boxPrefab;
    private OVRSemanticClassification[] spawnerObjects=new OVRSemanticClassification[2];
    private Vector3[] spawnerPosition=new Vector3[2];

    void Start()
    {
        // find ovr scene manager component in scene
        var sceneManager = FindObjectOfType<OVRSceneManager>();
        sceneManager.SceneModelLoadedSuccessfully += OnSceneModelLoadedSuccessfully;
        Debug.Log("Scene manager found");
    }

    public Vector3[] getSpawnerPositions()
    {
        return spawnerPosition;
    }
    private void boxeSpawn(Vector3 spawner)
    {
        Debug.Log("Boxe spawn");
        Instantiate(boxPrefab);
        boxPrefab.transform.position = spawner;
        boxPrefab.SetActive(true);
    }
    
    
    private void OnSceneModelLoadedSuccessfully()
    {
        Debug.Log("Scene loaded");
        spawnerObjects[1] = FindObjectsOfType<OVRSemanticClassification>()
            .Where(c => c.Contains(OVRSceneManager.Classification.Floor))
            .ToArray()[0];
        spawnerPosition[1] = spawnerObjects[1].transform.position;
        OVRSemanticClassification[] desks = FindObjectsOfType<OVRSemanticClassification>()
            .Where(c => c.Contains(OVRSceneManager.Classification.Table))
            .ToArray();
        if (desks.Length != 0)
        {
            spawnerObjects[0] = desks[0];
            spawnerPosition[0] = spawnerObjects[0].transform.position;
        }
        else
        {
            spawnerObjects[0] = spawnerObjects[1];
            spawnerPosition[0] = spawnerObjects[1].transform.position + new Vector3(spawnerObjects[1].transform.localScale.x/4,0, spawnerObjects[1].transform.localScale.z/4) ;
        }

        boxeSpawn(spawnerPosition[0]);
        boxeSpawn(spawnerPosition[1]);
    }


}