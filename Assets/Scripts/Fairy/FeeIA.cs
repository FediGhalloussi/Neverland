using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeeIA : MonoBehaviour
{
    public bool started = false;
    private bool localStarted = false;
    [SerializeField] private float initialSpeed;
    private float speed;
    [SerializeField] private float fearSpeed;
    [SerializeField] private AnimationCurve speedCurve;
    private Vector3 target;
    private bool movementOver;
    float movementDist;
    float currentDist;
    private Vector3[] handPositions;
    private int currentPositionIndex;
    [SerializeField] private GameObject hand;
    private bool isScared;
    private bool wasScared;
    [SerializeField] private float interactionDistance;
    [SerializeField] private GameObject chestSpawn;
    [SerializeField] private float speedLimit;

    [SerializeField] private FairyEnding fairyEndingScript;
    // Start is called before the first frame update
    void Start()
    {
        handPositions = new Vector3[10];
        currentPositionIndex = 0;
        speed = initialSpeed;
        movementOver=true;
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
        if (movementOver) // todo: check
        {
            target = new Vector3(Random.Range(-.25f,.25f),Random.Range(-.25f,.25f),Random.Range(.90f,1.1f));
            movementOver=false;
            movementDist = Vector3.Distance(transform.position,target);
        }
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
                if (isScared) isScared=false;
                else if (wasScared) wasScared=false;
                if (transform.position.z <2f) speed=initialSpeed;
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
        if (Vector3.Distance(hand.transform.position,transform.position)<interactionDistance&&!isScared)
        {
            if (SuccessOrScared())
            {
                Success();
                Destroy(this);
            }
            else
            {
                Scared();
                isScared=true;
                speed=fearSpeed;
            }
            
        }
    }

    private void Scared()
    {
        target = new Vector3(0f,0f,5f);
        movementOver=false;
        movementDist = Vector3.Distance(transform.position,target);
    }

    private void Success()
    {
        chestSpawn.SetActive(true);
        fairyEndingScript.hasStarted=true;
    }

    private bool SuccessOrScared() //returns true if success false if scared (according to velocity)
    {
        bool rep = true;
        if (wasScared) //pour pas qu'on puisse attendre au point d'arrivée de la fée
        {
            return false;
        }
        for (int i=currentPositionIndex;i<9;i++)
        {
            if (Vector3.Distance(handPositions[i],handPositions[i+1])>speedLimit/5f)
            {
                rep=false;
            }
        }
        if (currentPositionIndex!=0) 
        {
            if (Vector3.Distance(handPositions[9],handPositions[0])>speedLimit/5f)
            {
                rep=false;
            }
        }
        for (int i=0;i<currentPositionIndex;i++)
        {
            if (Vector3.Distance(handPositions[i],handPositions[i+1])>speedLimit/5f)
            {
                rep=false;
            }
        }
        // if we decide to move SuccessOrScared from positiontracking algo we need to add the last frames here
        return rep;
        
    }
}
