using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pirateSoundScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("ocean_noise");
    }

}
