using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SwitchModel : MonoBehaviour
{

    
    
    [SerializeField] private Material material;

    [SerializeField] private MeshRenderer renderer;


    private void Start()
    {
    }

    public void Switch()
    {
        renderer.materials[0] = material;
        renderer.materials[1] = material;
    }
}
