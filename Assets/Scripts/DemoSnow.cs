using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoSnow : MonoBehaviour
{
    [SerializeField] private Material[] materials;

    private void Awake()
    {
        for (int i = 0; i < materials.Length; ++i)
        {
            materials[i].SetFloat("_SnowAmount", 0.0f);
        }
    }
    

    private void Update()
    {
        //add a variable to control the speed of the snow amount changing
        float snowAmount = (Time.time / 50.0f) % 1.2f;

        for(int i = 0; i < materials.Length; ++i)
        {
            if (materials[i].GetFloat("_SnowAmount") < 1.0f)
            {
                materials[i].SetFloat("_SnowAmount", snowAmount);
            }
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < materials.Length; ++i)
        {
            materials[i].SetFloat("_SnowAmount", 0.0f);
        }
    }
}
