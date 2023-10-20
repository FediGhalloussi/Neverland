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
        if (gm.game_snow_started == false)
        {
            Instantiate(snowman, new Vector3(0, 0, 5f), Quaternion.identity);
            gm.game_snow_started = true;
        }
        ;
        
    }


    void Update()
    {
        
    }
}
