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
    }

    public void Flee()
    {
        transform.LookAt(player.transform);
        transform.Translate(Vector3.back * speed * Time.deltaTime);
        //todo : debug si bloqué dans un angle
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

    private void GameWon()
    {
        Debug.Log("player has won");
    }
}