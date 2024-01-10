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
    Vector3 movement;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        movement = Vector3.back * speed * Time.deltaTime;

    }

    // Update is called once per frame
    void Update()
    {
        distance= Vector3.Distance(player.transform.position, transform.position);
        if (distance < triggerArea)
        {
            Flee();
        }
    }

    public void Flee()
    {
        transform.LookAt(player.transform);
        transform.Translate(movement);
        //todo : debug si bloqué dans un angle
        //utilise la fonction Vector3.Reflect(Vector3 incidence originalObject.position, Vector3 normal)
        //Vector3.Reflect(speed.normalized,coll.contacts[0].normal)
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger detected with " + other.name);
        if (other.gameObject.CompareTag("Crocodile"))
        {
            Debug.Log("Game won");
            GameWon();
        } 
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Crocodile"))
        {

        }
        else
        {
            transform.Translate(Vector3.Reflect(movement.normalized, collision.contacts[0].normal));
        }
    }

    private void GameWon()
    {
        Debug.Log("player has won");
    }
}