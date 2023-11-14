using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawn : MonoBehaviour
{
    [SerializeField] private GameObject chest;
    // Start is called before the first frame update

    void Start()
    {
        Instantiate(chest, new Vector3(-1f,-1f,1f), Quaternion.identity);
        Instantiate(chest, new Vector3(1f,-1f,1f), Quaternion.identity);
    }


}
