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
    private float movementDist;
    private float currentDist;
    private Vector3[] handPositions;
    private Vector3[] hand2Positions;
    private int currentPositionIndex;
    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject hand2;
    private bool isScared;
    private bool wasScared;
    [SerializeField] private float interactionDistance;
    [SerializeField] private GameObject chestSpawn;
    [SerializeField] private float speedLimit;

    private float defaultHeight;

    [SerializeField] private FairyEnding fairyEndingScript;
    // Start is called before the first frame update
    void Start()
    {
        defaultHeight = transform.position.y;
        handPositions = new Vector3[10];
        hand2Positions = new Vector3[10];
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
                TrackHandPosition();
            }
            Move();
        }
    }

    private void StartMovement()
    {
        if (movementOver) // todo: check
        {
            target = new Vector3(Random.Range(-.25f,.25f),Random.Range(-.25f+defaultHeight,.25f+defaultHeight),Random.Range(.90f,1.1f));
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

    private void TrackHandPosition()
    {
        handPositions[currentPositionIndex] = hand.transform.position;
        hand2Positions[currentPositionIndex] = hand2.transform.position;
        if (currentPositionIndex<9) currentPositionIndex++;
        else currentPositionIndex=0;
        Invoke("TrackHandPosition",0.2f);
        if ((Vector3.Distance(hand.transform.position,transform.position)<interactionDistance)||(Vector3.Distance(hand2.transform.position,transform.position)<interactionDistance)&&!isScared)
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
        FindObjectOfType<AudioManager>().Play("fairy_fear");
        //FindObjectOfType<AudioManager>().Play("lose_sound");
        Debug.Log("Scared");
        target = new Vector3(0f,defaultHeight,5f);
        movementOver=false;
        movementDist = Vector3.Distance(transform.position,target);
    }

    private void Success()
    {
        FindObjectOfType<AudioManager>().Stop("bells");
        FindObjectOfType<AudioManager>().Play("chest_unlock");
        FindObjectOfType<AudioManager>().Play("victory_bell");
        Debug.Log("Success");
        GameManager.Instance.chestOpenable = true;
        chestSpawn.SetActive(true);
        fairyEndingScript.hasStarted=true;
        //TODO remove if we decide to do EndingScript
        //Invoke("DestroyFairy",3f);
    }

    private void DestroyFairy()
    {
        Destroy(gameObject);
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
        for (int i=currentPositionIndex;i<9;i++)
        {
            if (Vector3.Distance(hand2Positions[i],hand2Positions[i+1])>speedLimit/5f)
            {
                rep=false;
            }
        }
        if (currentPositionIndex!=0) 
        {
            if (Vector3.Distance(hand2Positions[9],hand2Positions[0])>speedLimit/5f)
            {
                rep=false;
            }
        }
        for (int i=0;i<currentPositionIndex;i++)
        {
            if (Vector3.Distance(hand2Positions[i],hand2Positions[i+1])>speedLimit/5f)
            {
                rep=false;
            }
        }
        // if we decide to move SuccessOrScared from positiontracking algo we need to add the last frames here
        return rep;
        
    }
}
