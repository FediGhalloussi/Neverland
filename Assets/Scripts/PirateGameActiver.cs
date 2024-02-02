using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PirateGameActiver : MonoBehaviour
{
    [SerializeField] private GameObject part1;
    [SerializeField] private GameObject part2;
    [SerializeField] private GameObject snow;
    
    public void Active1()
    {
        part1.SetActive(true);
        snow.SetActive(false);
    }

    public void Active2()
    {
        part2.SetActive(true);
        Destroy(FindObjectOfType<MagnyfingGlass>().gameObject);
        //FindObjectOfType<OVRSceneRoom>().gameObject.SetActive(false);
        var passthroughGradients = FindObjectsOfType<PassthroughFader>();
        foreach (var gradient in passthroughGradients)
        {
            gradient.Disappear();
        }

        var paintings = GameObject.FindGameObjectsWithTag("Painting");
        foreach (var p in paintings)
        {
            Destroy(p);
        }

        var switchModels = FindObjectsOfType<SwitchModel>();
        foreach (var model in switchModels)
        {
            model.Switch();
        }
        
    }
}
