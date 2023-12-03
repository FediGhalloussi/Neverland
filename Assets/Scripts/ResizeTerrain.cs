using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeTerrain : MonoBehaviour
{
    public GameObject terrain;
    public Vector3 positionOffset;
    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {   //get the normal
        Vector3 right = GetComponentInParent<OVRScenePlane>().gameObject.transform.right;
        Vector3 forward = GetComponentInParent<OVRScenePlane>().gameObject.transform.forward;
        //scale only local x and z
        terrain.transform.localScale = new Vector3(GetComponentInParent<OVRScenePlane>().Width, GetComponentInParent<OVRScenePlane>().Height,.15f );
        //terrain.transform.localScale = new Vector3(GetComponentInParent<OVRScenePlane>().Width*10, GetComponentInParent<OVRScenePlane>().Height*10,.15f );
        //terrain.gameObject.transform.rotation = GetComponentInParent<OVRScenePlane>().gameObject.transform.rotation;
        initialPosition = transform.position;

    }

    private void Update()
    {
        //Attack();
        transform.position = initialPosition + positionOffset;
    }

}
