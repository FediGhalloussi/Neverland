using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerFairy : MonoBehaviour
{
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("fairy");
        FindObjectOfType<AudioManager>().Play("bells");
    }


}
