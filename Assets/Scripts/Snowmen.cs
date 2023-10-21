using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class Snowmen : MonoBehaviour
{
    [SerializeField] GameObject snowman;
    [SerializeField] float speed_snowman;
    int nb_hit_snowman;
    GameObject player;
    MeshRenderer meshRenderer;
    // GameManager gm;
    // GameObject wall;

    // Start is called before the first frame update
    void Start()
    {
        nb_hit_snowman = 0; 
        //Instantiate(snowman);
        player = GameObject.FindGameObjectWithTag("Player");
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = true;
        //gm = FindObjectOfType<GameManager>;
        //wall = gm.wall;
        transform.position = new Vector3(0, 0.5f, 5);
    }
    private void Update()
    {
        Attack();
    }

    void Attack()
    {
        
        Vector3 direction_snowman = (player.transform.position-snowman.transform.position).normalized;
        transform.position += direction_snowman * Time.deltaTime * speed_snowman;
        transform.position = new Vector3(transform.position.x,0.5f,transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger detected with " + other.name);
        if (other.gameObject.tag == "Snowball")
        {
            Debug.Log("snowman hit");
            isHit();
        }
        else if (other.gameObject.tag == "Player")
        {
            Debug.Log("game over");
            gameOver();
        }
    }
    void isHit()
    {
        nb_hit_snowman += 1;
        Debug.Log(nb_hit_snowman + " snowman hit");
        if (nb_hit_snowman == 3)
        {
            gameWon();
        }
        else
        {
            snowman.transform.position = new Vector3(2, 0, 5);
        }
    }

    void gameWon()
    {
        Debug.Log("mini jeu reussi !");
    }
    void gameOver()
    {
        meshRenderer.enabled = false;
        Invoke("Start", 3.0f);
    }
}