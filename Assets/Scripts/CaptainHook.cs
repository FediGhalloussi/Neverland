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
    [SerializeField] GameObject crocodile;
    [SerializeField] GameObject boat;
    [SerializeField] private Animator anim_boat;
    Collider boatCollider;
    private Animator animCaptain;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        movement = Vector3.back * speed * Time.deltaTime;
        crocodile.SetActive(true);
        boatCollider = boat.GetComponent<Collider>();
        animCaptain = GetComponent<Animator>();
        
        animCaptain.SetBool("isRunning",true);
    }

    // Update is called once per frame
    void Update()
    {
        distance= Vector3.Distance(player.transform.position, transform.position);
        if (distance < triggerArea)
        {
            Flee();
        }
        else
        {
            animCaptain.SetBool("isRunning",true);
        }
    }

    public void Flee()
    {
        transform.LookAt(player.transform);
        movement = Vector3.back * speed * Time.deltaTime;
        animCaptain.SetBool("isRunning", true);
        transform.Translate(movement);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger detected with " + other.name);
        if (other.gameObject.CompareTag("Crocodile"))
        {
            Debug.Log("Game won");
            GameWon(); 
        }else if (other == boatCollider)
        {
            anim_boat.Play("boat");
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