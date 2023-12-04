using UnityEngine;
using System.Collections;
using System.Linq;

public class BoxesSpawner : MonoBehaviour
{
    [SerializeField] private GameObject boxPrefab;
    private OVRSemanticClassification[] spawnerObjects=new OVRSemanticClassification[2];
    private Transform[] spawnerPosition=new Transform[2];

    void Start()
    {
        // find ovr scene manager component in scene
        var sceneManager = FindObjectOfType<OVRSceneManager>();
        sceneManager.SceneModelLoadedSuccessfully += OnSceneModelLoadedSuccessfully;
        Debug.Log("Scene manager found");
    }
    
    private void boxeSpawn(Transform spawner)
    {
        Debug.Log("Boxe spawn");
        GameObject box = Instantiate(boxPrefab, spawner);
        OVRScenePlane plane = spawner.GetComponent<OVRScenePlane>();
        Vector2 dimension = plane.Dimensions;
        box.transform.position = new Vector3(Random.Range(dimension.x / 2f - dimension.x,dimension.x/2f),
                                             Random.Range(dimension.y / 2f - dimension.y, dimension.y / 2f),
                                             0);
        boxPrefab.SetActive(true);
    }
    
    
    private void OnSceneModelLoadedSuccessfully()
    {
        Debug.Log("Scene loaded");
        spawnerObjects[1] = FindObjectsOfType<OVRSemanticClassification>()
            .Where(c => c.Contains(OVRSceneManager.Classification.Floor))
            .ToArray()[0];
        spawnerPosition[1] = spawnerObjects[1].transform;
        OVRSemanticClassification[] desks = FindObjectsOfType<OVRSemanticClassification>()
            .Where(c => c.Contains(OVRSceneManager.Classification.Table))
            .ToArray();
        if (desks.Length != 0)
        {
            spawnerObjects[0] = desks[0];
            spawnerPosition[0] = spawnerObjects[0].transform;
        }
        else
        {
            spawnerObjects[0] = spawnerObjects[1];
            spawnerPosition[0] = spawnerObjects[1].transform;
        }

        boxeSpawn(spawnerPosition[0]);
        boxeSpawn(spawnerPosition[1]);
    }


}