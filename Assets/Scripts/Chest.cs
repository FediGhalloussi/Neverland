using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [Tooltip("Objects need to be ordered")]
    public GameObject[] objectsInChest;
    
    bool isOpen = false;
    Animator animator;
    
    private void Start()
    {
        objectsInChest[GameManager.Instance.currentObjectIndex].SetActive(true);
        animator = GetComponentInChildren<Animator>();
    }
    
    private void Update()
    {
        //if chest is open and object of current index is not colliding with chest, close chest
        if (isOpen && !objectsInChest[GameManager.Instance.currentObjectIndex].GetComponent<Collider>().bounds.Intersects(GetComponent<Collider>().bounds))
        {
            CloseChest();
        }
    }
    
    public void NextObject()
    {
        objectsInChest[GameManager.Instance.currentObjectIndex].SetActive(false);
        if (GameManager.Instance.currentObjectIndex < objectsInChest.Length)
        {
            objectsInChest[GameManager.Instance.currentObjectIndex].SetActive(true);
        } 
        GameManager.Instance.chestOpenable = true;
    }
    
    public void OpenChest()
    {
        Debug.Log("Open chest");
        // play animation of opening chest
        animator.SetBool("open", true);
        // play sound of opening chest
        
        isOpen = true;
    }
    
    public void CloseChest()
    {
        // play animation of closing chest
        animator.SetBool("open", false);
        // play sound of closing chest
        
        // make chest disappear
        
        GameManager.Instance.chestOpenable = false;
        isOpen = false;
    }

    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger detected with " + other.name);
        Debug.Log("Trigger detected with " + other.tag);
        Debug.Log(GameManager.Instance.chestOpenable);
        if (other.gameObject.CompareTag("Hand") && GameManager.Instance.chestOpenable)
        {
            OpenChest();
        }
    }
}
