using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawn : MonoBehaviour
{
    [SerializeField] private GameObject chest1;
    [SerializeField] private GameObject chest2;
    // Start is called before the first frame update

    void Start()
    {
        chest1.SetActive(true);
        chest2.SetActive(true);
    }


}
