using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.XR.Oculus;
using UnityEngine;


public class FairyAI : MonoBehaviour
{
    // a box of the size of the playfield, when idle the fairy is randomly moving
    private float roomHeight;
    private OVRScenePlane floor;
    [SerializeField] private float speed = 0.8f;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private Transform player;
        
    private Vector3 boundaryDimensions;

    private Vector3 targetPosition;

    [SerializeField] private Transform itemSlot;
    
    private RetrievableItem objectToRetrieve;
    
    // Start is called before the first frame update
    
    private enum State
    {
        Idle,
        Retrieving,
    }

    private State state;
    void Start()
    { 
        boundaryDimensions = new OVRBoundary().GetDimensions(OVRBoundary.BoundaryType.PlayArea);
        // the height of the room
        roomHeight = FindObjectsOfType<OVRSemanticClassification>()
            .Where(c => c.Contains(OVRSceneManager.Classification.WallFace))
            .ToArray()[0].GetComponent<OVRScenePlane>().Height;

        state = State.Idle;

        targetPosition = RandomPosition();

    }


    private Vector3 RandomPosition()
    {
        return new Vector3(Random.Range(-boundaryDimensions.x / 2f, boundaryDimensions.x / 2f), Random.Range(0f, roomHeight),
            Random.Range(-boundaryDimensions.z / 2f, boundaryDimensions.z / 2f));   
    }
    private void MoveTowardsTarget()
    {
        transform.position += (targetPosition - transform.position).normalized * (speed * Time.deltaTime);
        var targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
        var euler = targetRotation.eulerAngles;
        euler.x = 0;
        euler.z = 0;
        targetRotation.eulerAngles = euler;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void GoRetrieve(RetrievableItem item)
    {
        if (objectToRetrieve == null)
        {
            objectToRetrieve = item;
            objectToRetrieve.isBeingRetrieved = true;
            var rb = objectToRetrieve.GetComponent<Rigidbody>();
            state = State.Retrieving;
            if (rb)
            {
                rb.useGravity = false;
                rb.velocity = Vector3.zero;
                rb.rotation = Quaternion.identity;
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Idle:
                objectToRetrieve = null;
                if ((transform.position - targetPosition).magnitude > 0.1f)
                {
                    MoveTowardsTarget();
                }
                else
                {
                    targetPosition = RandomPosition();
                }
                break;
            case State.Retrieving:
                targetPosition = objectToRetrieve.transform.position;
                if ((transform.position - targetPosition).magnitude > 0.1f)
                {
                    MoveTowardsTarget();
                    
                }
                else
                {
                    objectToRetrieve.transform.parent = itemSlot;
                    objectToRetrieve.transform.localPosition = Vector3.zero;
                    objectToRetrieve.transform.rotation = Quaternion.identity;
                    state = State.Idle;
                    targetPosition = RandomPosition();
                }
                break;
        }
    }
}
