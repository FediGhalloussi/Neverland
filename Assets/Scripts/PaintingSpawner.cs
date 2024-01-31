using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Drawing;

public class PaintingSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] paintingPrefabs = new GameObject[3];
    private List<Transform> paintingTransforms = new List<Transform>();

    void Start()
    {
        SpawnAllPainting();
    }
    
    private void SpawnPainting(GameObject painting, Transform spawnParent)
    {
        GameObject p = Instantiate(painting, spawnParent);
        OVRScenePlane plane = spawnParent.GetComponent<OVRScenePlane>();
        Vector2 dimension = plane.Dimensions;
        p.transform.localPosition = new Vector3(Random.Range(dimension.x / 2f - dimension.x + 0.5f,dimension.x/2f - 0.5f),
                                           Random.Range(dimension.y / 2f - dimension.y + 0.5f, dimension.y / 2f - 0.5f),
                                           0.02f);
        foreach(var t in paintingTransforms)
        {
            // if the painting we are spawning have the same parent has one of the instantiated painting, do the check
            if (t.parent == p.transform.parent)
            {
                // all painting have the same scale
                // if painting overlap, destroy the instantiated painting and retry
                if (Mathf.Abs(p.transform.localPosition.x - t.localPosition.x) < t.lossyScale.x ||
                    Mathf.Abs(p.transform.localPosition.y - t.localPosition.y) < t.lossyScale.y)
                {
                    Destroy(p);
                    SpawnPainting(painting, spawnParent);
                    return;
                }
            }
        }
        paintingTransforms.Add(p.transform);
    }
    
    private void SpawnAllPainting()
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