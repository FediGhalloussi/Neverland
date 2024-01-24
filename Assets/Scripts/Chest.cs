using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [Tooltip("Objects need to be ordered")]
    public GameActiver[] objectsInChest = new GameActiver[2];
    
    bool isOpen = false;
    private bool wasOpenOnce = false;
    Animator animator;
    
    private void Start()
    {
        objectsInChest[0] = GetComponentInChildren<SnowGlobe>(true);
        objectsInChest[0].gameObject.SetActive(false);
        objectsInChest[1] = GetComponentInChildren<MagnyfingGlass>(true);
        objectsInChest[1].gameObject.SetActive(false);
        
        Debug.Log(objectsInChest[GameManager.Instance.currentObjectIndex]);
        Debug.Log(objectsInChest[GameManager.Instance.currentObjectIndex].gameObject);
        animator = GetComponentInChildren<Animator>();
        
        if (!wasOpenOnce)
        {
            objectsInChest[GameManager.Instance.currentObjectIndex].gameObject.SetActive(true);
            animator.SetBool("open", false);
        }

    }
    
    private void Update()
    {
        //if chest is open and object of current index is not colliding with chest, close chest
        Collider collider = objectsInChest[GameManager.Instance.currentObjectIndex].gameObject.GetComponent<Collider>();
        if (collider == null)
        {
            collider = objectsInChest[GameManager.Instance.currentObjectIndex].gameObject.GetComponentInChildren<Collider>();
        }
        if (!collider.bounds.Intersects(GetComponent<Collider>().bounds))
        {
            //CloseChest();
            objectsInChest[GameManager.Instance.currentObjectIndex].ActivateGame();
        }
    }
    
    public void NextObject()
    {
        int index = GameManager.Instance.currentObjectIndex;
        objectsInChest[index].gameObject.SetActive(false);

        index++;

        if (index < objectsInChest.Length && !wasOpenOnce)
        {
            objectsInChest[index].gameObject.SetActive(true);
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
        wasOpenOnce = true;
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
    
    public void MakeChestDisappear()
    {
        // todo add vfx maybe ?
        
        // make chest disappear
        gameObject.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger detected with " + other.name);
        Debug.Log("Trigger detected with " + other.tag);
        Debug.Log(GameManager.Instance.chestOpenable);
        if (other.gameObject.CompareTag("Hand") && GameManager.Instance.chestOpenable && !wasOpenOnce)
        {
            OpenChest();
        }
    }
}
