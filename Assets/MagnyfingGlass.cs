using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnyfingGlass : MonoBehaviour
{
    private LayerMask mask;
    // Start is called before the first frame update
    void Start()
    {
        mask = LayerMask.GetMask("Painting");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Ray ray = new Ray(transform.position,transform.forward);
        bool res = Physics.SphereCast(ray,0.5f,100f,mask);

        if (res)
        {
            Debug.Log("Start le jeu boulet de cannon");
        }
    }
}
