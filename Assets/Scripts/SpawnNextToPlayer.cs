using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNextToPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(player.transform.position.x,player.transform.position.y,player.transform.position.z+1f);
        Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
