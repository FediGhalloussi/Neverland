using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using TMPro;
using Random = UnityEngine.Random;

public class BoxesSpawner : MonoBehaviour
{
    [SerializeField] private GameObject boxPrefab;
    private OVRSemanticClassification[] spawnerObjects=new OVRSemanticClassification[2];
    private Transform[] spawnerPosition=new Transform[2];
    [SerializeField] private Transform trackingSpace;
    OVRBoundary boundary = new OVRBoundary();


    //todo use this to spawn boxes after catching fairy
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
   
        box.transform.localPosition = new Vector3(Random.Range(dimension.x / 2f - dimension.x,dimension.x/2f),
            Random.Range(dimension.y / 2f - dimension.y, dimension.y / 2f),
            0.1f);

        
        // the closest point on the boundary in tracking space
        if (!IsInsideBoundary(box.transform.position))
        {
            var rect = boundary.GetGeometry(OVRBoundary.BoundaryType.PlayArea);
            box.transform.position = trackingSpace.InverseTransformVector(new Vector3(Random.Range(rect[0].x, -rect[0].x),box.transform.position.y,Random.Range(-rect[0].z,rect[0].z)));
        }
        
        boxPrefab.SetActive(true);
        box.SetActive(false);
    }

    private bool IsInsideBoundary(Vector3 point)
    {

        float minX, minZ, maxX, maxZ;
        var rect = boundary.GetGeometry(OVRBoundary.BoundaryType.PlayArea);
        // we convert the first point of the rectangle boundary to world space
        var p0 = trackingSpace.InverseTransformVector(rect[0]);
        // we initialize the bounds values
        minX = p0.x;
        minZ = p0.z;
        maxX = p0.x;
        maxZ = p0.z;
        
        foreach (var p in rect)
        {
            var p_ = trackingSpace.InverseTransformVector(p);
            if (p_.x < minX)
            {
                minX = p_.x;
            }

            if (p_.z < minZ)
            {
                minZ = p_.z;
            }

            if (p_.x > maxX)
            {
                maxX = p_.x;
            }

            if (p_.z > maxZ)
            {
                maxZ = p_.z;
            }
        }

        return (point.x > minX && point.x < maxX && point.z > minZ && point.z < maxZ);
    }

    private void Update()
    {
        Debug.Log(boundary.GetConfigured());
        var points = boundary.GetGeometry(OVRBoundary.BoundaryType.PlayArea);
        for (int i = 0; i <= points.Length; i++)
        {
            Debug.LogWarning(trackingSpace.InverseTransformVector(points[i%points.Length]));
            Debug.DrawLine(trackingSpace.InverseTransformVector(points[i%points.Length]),
                           trackingSpace.InverseTransformVector(points[i+1%points.Length]),
                           Color.green);   
        }
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