using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestReveal : MonoBehaviour
{
    //todo : remove pblic
    public Chest[] chests;
    // Start is called before the first frame update

    void Start()
    {
        chests = FindObjectsByType<Chest>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (var chest in chests)
        {
            chest.gameObject.SetActive(true);
        }
    }

    public void NextObject()
    {
        foreach (var chest in chests)
        {
            chest.objectsInChest[GameManager.Instance.currentObjectIndex].gameObject.SetActive(false);
        }
        GameManager.Instance.currentObjectIndex++;

        foreach (var chest in chests)
        {
            if (GameManager.Instance.currentObjectIndex < chest.objectsInChest.Length)
            {
                chest.objectsInChest[GameManager.Instance.currentObjectIndex].gameObject.SetActive(true);
            } 
        }
        
        GameManager.Instance.chestOpenable = true;

    }

}
