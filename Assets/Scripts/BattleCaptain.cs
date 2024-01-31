using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleCaptain : MonoBehaviour
{
     [SerializeField] private GameObject player;
     [SerializeField] private GameObject clock;

     [SerializeField] private float fearDistance = 2;
     [SerializeField] private float fearTime = 3;

     private float speed = 5;
     private float lastFearTime = 0;

     void Update()
     {
          Move();
     }

     void Move()
     {
          if ((Time.time - lastFearTime) > fearTime)
          {
               Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
          }
     }
     
}
