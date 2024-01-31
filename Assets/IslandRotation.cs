using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandRotation : MonoBehaviour
{
    
    OVRBoundary boundary = new OVRBoundary();
    
    // Start is called before the first frame update
    void Start()
    {
        Vector3[] geometry = boundary.GetGeometry(OVRBoundary.BoundaryType.PlayArea);
        if (Mathf.Abs(geometry[0].x) > Mathf.Abs(geometry[1].z))
        {
            Debug.LogError("A");
        }
        else
        {
            Debug.LogError("B");
        }
    }
    
}
