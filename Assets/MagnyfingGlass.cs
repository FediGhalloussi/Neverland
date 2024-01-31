using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnyfingGlass : MonoBehaviour , GameActiver
{
    private LayerMask mask;
    private float charge;
    private float chargeToActivate = 2f;
    private PirateGameActiver activer;
    // Start is called before the first frame update
    void Start()
    {
        //pirateGame2 = GameObject.FindGameObjectWithTag("PirateGame2");
        activer = FindObjectOfType<PirateGameActiver>();
        mask = LayerMask.GetMask("Painting");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Ray ray = new Ray(transform.position,transform.forward);
        bool res = Physics.SphereCast(ray,0.5f,100f,mask);

        if (res) {
            charge += Time.fixedTime;
        } else {
            charge -= Time.fixedTime;
        }
        if (charge < 0) {
            charge = 0;
        }

        if (charge > chargeToActivate)
        {
            activer.Active2();
        }
    }
    
    public void ActivateGame()
    {
        if (activer != null)
        {
            activer.Active1();
        }
    }
}
