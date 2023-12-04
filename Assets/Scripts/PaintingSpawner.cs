using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PaintingSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] paintingPrefabs = new GameObject[3];

    void Start()
    {
        // find ovr scene manager component in scene
        var sceneManager = FindObjectOfType<OVRSceneManager>();
        sceneManager.SceneModelLoadedSuccessfully += OnSceneModelLoadedSuccessfully;
    }
    
    private void SpawnPainting(GameObject painting, Transform spawnParent)
    {
        var p = Instantiate(painting, spawnParent);
        OVRScenePlane plane = spawnParent.GetComponent<OVRScenePlane>();
        Vector2 dimension = plane.Dimensions;
        p.transform.position = new Vector3(Random.Range(dimension.x / 2f - dimension.x,dimension.x/2f),
                                           Random.Range(dimension.y / 2f - dimension.y, dimension.y / 2f),
                                           0);
    }
    
    private void OnSceneModelLoadedSuccessfully()
    {

        // find all walls in scene model
        var walls = FindObjectsOfType<OVRSemanticClassification>()
            .Where(c => c.Contains(OVRSceneManager.Classification.WallFace)).ToArray();
        
        foreach (var painting in paintingPrefabs)
        {
            int r = Random.Range(0, walls.Length);
            // spawn a painting on a random wall
            SpawnPainting(painting,walls[r].transform);
        }
        
    }


}