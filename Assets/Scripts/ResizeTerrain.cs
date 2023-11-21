using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeTerrain : MonoBehaviour
{
    public GameObject terrain;
    // Start is called before the first frame update
    void Start()
    {
        terrain.transform.localScale = new Vector3(GetComponentInParent<OVRScenePlane>().Width, 0, GetComponentInParent<OVRScenePlane>().Height);
        //terrain.gameObject.transform.rotation = GetComponentInParent<OVRScenePlane>().gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
