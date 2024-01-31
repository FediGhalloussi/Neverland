using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetrievableItem : MonoBehaviour
{

    private OVRBoundary boundary = new OVRBoundary();

    private float timeBeforeRetrieving = 3f;

    private float timer;

    public bool isBeingRetrieved;

    private Transform previousParent;

    private FairyAI fairy; 
    // Start is called before the first frame update
    void Start()
    {
        fairy = FindObjectOfType<FairyAI>();
        previousParent = transform.parent;
        isBeingRetrieved = false;
    }

    private bool IsInsideBoundary(Vector3 point)
    {

        float minX, minZ, maxX, maxZ;
        var rect = boundary.GetGeometry(OVRBoundary.BoundaryType.PlayArea);
        // we convert the first point of the rectangle boundary to world space
        var p0 = rect[0];
        // we initialize the bounds values
        minX = p0.x;
        minZ = p0.z;
        maxX = p0.x;
        maxZ = p0.z;
        
        foreach (var p in rect)
        {
            if (p.x < minX)
            {
                minX = p.x;
            }

            if (p.z < minZ)
            {
                minZ = p.z;
            }

            if (p.x > maxX)
            {
                maxX = p.x;
            }

            if (p.z > maxZ)
            {
                maxZ = p.z;
            }
        }

        return (point.x > minX && point.x < maxX && point.z > minZ && point.z < maxZ);
    }
    // Update is called once per frame
    void Update()
    {
        if (IsInsideBoundary(transform.position))
        {
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }

        if (timer > timeBeforeRetrieving)
        { 
            Debug.LogError("Go retrieve !");
            if (!isBeingRetrieved)
            {
                fairy.GoRetrieve(this);
            }
        }
    }

    public void ResetParent()
    {
        transform.parent = previousParent;
    }
    
}
