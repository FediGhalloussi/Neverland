using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoSnow : MonoBehaviour
{
    [SerializeField] private Material[] materials;
    public GameObject snowGlobe; //ONLY FOR TESTING PURPOSE

    private void Awake()
    {
        for (int i = 0; i < materials.Length; ++i)
        {
            materials[i].SetFloat("_SnowAmount", 1f);
        }
    }

    private void Start()
    {
        snowGlobe.transform.position = transform.position + new Vector3(1f, 1f, 1f);
    }

    private void Update()
    {
        float snowAmount = (Time.time / 50.0f) % 1.2f;

        for(int i = 0; i < materials.Length; ++i)
        {
            materials[i].SetFloat("_SnowAmount", 1f);
        }
    }
}
