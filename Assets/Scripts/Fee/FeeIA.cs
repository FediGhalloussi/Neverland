using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeeIA : MonoBehaviour
{
    public bool started = false;
    private bool localStarted = false;
    [SerializeField] private float speed;
    [SerializeField] private AnimationCurve speedCurve;
    private Vector3 target;
    private bool movementOver;
    float movementDist;
    float currentDist;
    private Vector3[] handPositions;
    private int currentPositionIndex;
    [SerializeField] private GameObject hand;

    // Start is called before the first frame update
    void Start()
    {
        handPositions = new Vector3[10];
        currentPositionIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            if (!localStarted)
            {
                StartMovement();
                localStarted=true;
                trackHandPosition();
            }
            Move();
        }
    }

    private void StartMovement()
    {
        target = new Vector3(Random.Range(-.25f,.25f),Random.Range(-.25f,.25f),Random.Range(.90f,1.1f));
        movementOver=false;
        movementDist = Vector3.Distance(transform.position,target);
    }

    private void Move()
    {
        if (Vector3.Distance(transform.position,target)<speed*speedCurve.Evaluate(DistanceManagement())*Time.deltaTime)
        {
            if (!movementOver)
            {
                transform.position = target;
                Invoke("StartMovement",0.15f);
                movementOver=true;
            }
            
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed*speedCurve.Evaluate(DistanceManagement())*Time.deltaTime);
            //print(DistanceManagement());
            
        }
    }

    private float DistanceManagement()
    {
        currentDist = Vector3.Distance(transform.position, target);
        //print(currentDist/movementDist);
        return currentDist/movementDist;
    }

    private void trackHandPosition()
    {
        handPositions[currentPositionIndex] = hand.transform.position;
        if (currentPositionIndex<9) currentPositionIndex++;
        else currentPositionIndex=0;
        Invoke("trackHandPosition",0.2f);
    }
}
