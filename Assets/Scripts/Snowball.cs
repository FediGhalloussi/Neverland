using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour
{

    [SerializeField] GameObject snowman;
    private GameManager gm;


    void Start()
    {
        gm = (GameManager)FindObjectOfType(typeof(GameManager));
        if (gm.num_snowman==0)
        {
            Instantiate(snowman, new Vector3(0, 0, 5f), Quaternion.identity);
        }
        gm.num_snowman++;
        
    }


    void Update()
    {
        
    }
}
