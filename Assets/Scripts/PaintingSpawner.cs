using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Drawing;

public class PaintingSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] paintingPrefabs = new GameObject[3];
    [SerializeField] List <Vector3> paintingPositions = new List<Vector3>();
    Vector3 size;

    void Start()
    {
        SpawnAllPainting();
        var renderer = paintingPrefabs[0].GetComponent<MeshRenderer>();
        size = renderer.bounds.size;
    }
    
    private void SpawnPainting(GameObject painting, Transform spawnParent)
    {
        GameObject p = Instantiate(painting, spawnParent);
        OVRScenePlane plane = spawnParent.GetComponent<OVRScenePlane>();
        Vector2 dimension = plane.Dimensions;
        p.transform.localPosition = new Vector3(Random.Range(dimension.x / 2f - dimension.x + 0.5f,dimension.x/2f - 0.5f),
                                           Random.Range(dimension.y / 2f - dimension.y + 0.5f, dimension.y / 2f - 0.5f),
                                           0.02f);
        paintingPositions.Add(p.transform.localPosition);
        foreach(var position in paintingPositions)
        {
            if(Mathf.Abs(p.transform.localPosition.x-position.x)<size.x || Mathf.Abs(p.transform.localPosition.y - position.y) < size.y)
            {
                paintingPositions.Remove(p.transform.localPosition);
                Destroy(p);
                SpawnPainting(painting, spawnParent);
            }
        }
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