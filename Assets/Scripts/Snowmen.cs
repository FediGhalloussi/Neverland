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
    // GameManager gm;
    // GameObject wall;

    // Start is called before the first frame update
    void Start()
    {
        nb_hit_snowman = 0; 
        Instantiate(snowman);
        player = GameObject.FindGameObjectWithTag("Player");
        //gm = FindObjectOfType<GameManager>;
        //wall = gm.wall;
        snowman.transform.position = new Vector3(0, 0, 5);
    }
    private void Update()
    {
        Attack();
    }

    void Attack()
    {
        Vector3 direction_snowman = (snowman.transform.position - player.transform.position).normalized;
        snowman.transform.position += direction_snowman * Time.deltaTime * speed_snowman;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Snowball")
        {
            isHit();
        }
        else if (other.gameObject.tag == "Player")
        {
            gameOver();
        }
    }
    void isHit()
    {
        nb_hit_snowman += 1;
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
        Debug.Log("mini jeu réussi !");
    }
    void gameOver()
    {
        Destroy(snowman);
        Invoke("Start", 3.0f);
    }
}