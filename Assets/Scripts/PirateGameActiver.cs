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
        var planeObjects = FindObjectsOfType<OVRSemanticClassification>()
            .Where(c => c.Contains(OVRSceneManager.Classification.WallFace))
            .ToArray();
        foreach (var p in planeObjects)
        {
            p.transform.GetComponentInChildren<PassthroughGradient>().Disappear();
        }

        var paintings = GameObject.FindGameObjectsWithTag("Painting");
        foreach (var p in paintings)
        {
            Destroy(p);
        }
    }
}
