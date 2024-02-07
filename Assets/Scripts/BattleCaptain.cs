using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleCaptain : MonoBehaviour
{
     [SerializeField] private GameObject player;
     [SerializeField] private GameObject clock;
     [SerializeField] private GameObject croco;

     [SerializeField] private GameObject hand1;
     [SerializeField] private GameObject hand2;

     [SerializeField] private Animator animator;
     
     [SerializeField] private float fearTime = 5;
     

     private float speed = 1f;
     private float lastFearTime = 0;

     private bool handsAreClose = false;
     private float handsCloseDist = 0.1f;

     private float lostDist = 0.5f;

     private float crocoRange = 2f;


     void Start()
     {
         animator.SetBool("isScared",false);
         FindObjectOfType<AudioManager>().Play("voc3");
     }
     
     void Update()
     {
          var position = transform.position;
          transform.LookAt(new Vector3(position.x,player.transform.position.y,position.z));
          Move();
          if (handsAreClose)
          {
               if (Vector3.Distance(hand1.transform.position, hand2.transform.position) > handsCloseDist)
               {
                    handsAreClose = false;
               }
          }
          else
          {
               if ((Vector3.Distance(hand1.transform.position, hand2.transform.position) < handsCloseDist)&&(Vector3.Distance(hand1.transform.position,clock.transform.position)<handsCloseDist*3))
               {
                    if (Time.time - lastFearTime > fearTime)
                    {
                         handsAreClose = true;
                         lastFearTime = Time.time;
                         FindObjectOfType<AudioManager>().Play("clock");
                         animator.SetBool("isScared",true);
                    }
               }
          }

          if (Vector3.Distance(transform.position, new Vector3(player.transform.position.x,0,player.transform.position.z)) < lostDist)
          {
               FindObjectOfType<AudioManager>().Play("lose_sound");
               transform.position = new Vector3(0, 0, -10);
          }

          if (Vector3.Distance(transform.position, croco.transform.position) < crocoRange)
          {
               Win();
          }
     }

     void Move()
     {
          if ((Time.time - lastFearTime) > fearTime)
          {
               var pos = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
               transform.position = new Vector3(pos.x, 0, pos.z);
               animator.SetBool("isScared",false);
          }
          else
          {
               var pos = transform.position;
               var dir = player.transform.position - pos;
               var newpos = Vector3.MoveTowards(transform.position, transform.position-dir, speed * Time.deltaTime);
               transform.position = new Vector3(newpos.x, 0, newpos.z);
          }
     }

     void Win()
     {
          FindObjectOfType<AudioManager>().Play("victory_bell");
          Destroy(gameObject);
          Fireworks();
          Invoke("Fireworks",3f);
          Invoke("Fireworks",6f);
          Invoke("Fireworks",9f);
     }

     void Fireworks()
     {
          VFXFactory.Instance.GetVFX(VFXType.Fireworks1, transform.position);
          VFXFactory.Instance.GetVFX(VFXType.Fireworks2, new Vector3(-1,2,1));
          VFXFactory.Instance.GetVFX(VFXType.Fireworks1, new Vector3(0,2,1));
          VFXFactory.Instance.GetVFX(VFXType.Fireworks2, new Vector3(1,2,1));
     }
     
}
