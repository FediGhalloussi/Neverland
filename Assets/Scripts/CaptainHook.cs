using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CaptainHook : MonoBehaviour
{
    private GameObject player;
    float distance;
    [SerializeField] float triggerArea = 5;
    [SerializeField] float speed;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        distance= Vector3.Distance(player.transform.position, transform.position);
        if (distance < triggerArea)
        {
            Flee();
        }
        //todo : gameWon si le captain touche le crocodile
    }

    public void Flee()
    {
        //todo : fuite sur les côtés si rencontre un mur
        transform.Translate(Vector3.back * speed * Time.deltaTime);
    }
}