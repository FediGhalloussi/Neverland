using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class MagnyfingGlass : MonoBehaviour , GameActiver
{
    private LayerMask mask;
    private float charge;
    private float chargeToActivate = 2f;
    private PirateGameActiver activer;

    private Rigidbody rb;
    private Grabbable grabbable;
    private bool canActivateGravity = false;

    
    // Start is called before the first frame update
    void Start()
    {
        //pirateGame2 = GameObject.FindGameObjectWithTag("PirateGame2");
        activer = FindObjectOfType<PirateGameActiver>();
        mask = LayerMask.GetMask("Painting");
        rb = GetComponent<Rigidbody>();
        grabbable = GetComponent<Grabbable>();
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
        

        //if component is grabbed and gravity is false, then activate gravity
        if (grabbable.SelectingPointsCount >= 1)
        {
            canActivateGravity = true;
            rb.useGravity = true;
            rb.isKinematic = false;
        }
        
        if (canActivateGravity && grabbable.SelectingPointsCount == 0)
        {
            rb.isKinematic = false;
        }
        //if velocity is too high, then clamp it
        if (rb.velocity.magnitude > 1f && grabbable.SelectingPointsCount == 0)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, 1f);
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
