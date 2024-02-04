using System.Collections.Generic;
using System.Linq;
using Unity.XR.CoreUtils;
using UnityEngine;

public class Snowmen : MonoBehaviour
{
    [SerializeField] private GameObject snowman;
    [SerializeField] private GameObject magnifyingGlassPrefab;
    [SerializeField] private float speedSnowman;
    private List<GameObject> children;
    private int numberOfHitsSnowman;
    public GameObject player;
    private MeshRenderer meshRenderer;
    private OVRSemanticClassification floor;
    private Vector3 initialPosition;
    private float heightSnowman;
    private bool canMove = true;

    private GameObject middleEyeAnchor;


    private PirateGameActiver activer;

    void Start()
    {
        FindObjectOfType<AudioManager>().Play("wind");
        numberOfHitsSnowman = 0;
        activer = FindObjectOfType<PirateGameActiver>();
        //player = GameObject.FindGameObjectWithTag("Player");
        middleEyeAnchor = GameObject.FindGameObjectWithTag("MainCamera");
        //nextGame = GameObject.FindGameObjectWithTag("PirateGame");
        player = middleEyeAnchor;
        floor = FindObjectsOfType<OVRSemanticClassification>()
            .Where(c => c.Contains(OVRSceneManager.Classification.Floor))
            .ToArray()[0];
        if (floor == null)
        {
            Debug.Log("1");
            floor = GameManager.Instance.floor;
        }

        heightSnowman = GetComponent<CapsuleCollider>().height; //height of the snowman
        //initialPosition = floor.GetComponentInChildren<SnowballSpawner>().transform.position + floor.GetComponentInChildren<SnowballSpawner>().transform.forward * heightSnowman/2f ;
        //initial pos is 10 meter in front of the player with a random angle between -30 and 30 degrees around player forward
        //initialPosition = player.transform.position + player.transform.forward * 5f + floor.transform.forward *  heightSnowman;
        //transform.position = GetInitialPosition();
        //transform.position = new Vector3(0f, 0f, 5f);

        Vector3 initialPosition = GetInitialPositionFrontOfObstacles(1);
        // this.transform.position = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z);
        // this.transform.localPosition = new Vector3(transform.localPosition.x,
        //     transform.localPosition.y, /*transform.localPosition.z*/ .5f);
        // Debug.Log("Snowman spawned  at : " + initialPosition);
        StartCoroutine(MoveSnowmanStart(initialPosition));
    }

    private void Update()
    {
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        Attack();
    }
    
    private Vector3 GetInitialPositionFrontOfObstacles(int furthestIndex)
    {
        List<Vector3> furthestPositions = new List<Vector3>();
        float radius = 1f; // radius of the sphere cast

        for (float theta = 0; theta < 360; theta += 10) // increment theta by 10 degrees each time
        {
            float phi = 0; // start phi at 0 each time
            // convert spherical coordinates to cartesian coordinates
            Vector3 direction = new Vector3(
                Mathf.Cos(theta * Mathf.Deg2Rad) * Mathf.Cos(phi * Mathf.Deg2Rad),
                Mathf.Sin(phi * Mathf.Deg2Rad),
                Mathf.Sin(theta * Mathf.Deg2Rad) * Mathf.Cos(phi * Mathf.Deg2Rad)
            );
            Debug.DrawRay(player.transform.position, direction * 100f, Color.green, 10000f);

            RaycastHit hit;
            if (Physics.Raycast(player.transform.position, direction, out hit, 1000f))
            {
                furthestPositions.Add(hit.point);
            }
        }

        // Sort the list of furthest positions based on distance from player
        furthestPositions.Sort((a, b) => Vector3.Distance(b, player.transform.position).CompareTo(Vector3.Distance(a, player.transform.position)));

        // Check if the requested furthest index is within range
        if (furthestIndex >= 0 && furthestIndex < furthestPositions.Count)
        {
            return furthestPositions[furthestIndex];
        }
        else
        {
            // Return Vector3.zero if the requested index is out of range
            return Vector3.zero;
        }
    }


    void Attack()
    {
        if (canMove == false)
        {
            return;
        }

        Vector3 directionSnowman = (player.transform.position - transform.position).normalized;
        transform.position += new Vector3(directionSnowman.x * Time.deltaTime * speedSnowman,
            directionSnowman.y * Time.deltaTime * speedSnowman * 0f,
            directionSnowman.z * Time.deltaTime * speedSnowman);
        // if distance between player and snowman is less than 0.7 withouth counting the up vector of this object
        Vector3 playerOnPlane = Vector3.ProjectOnPlane(player.transform.position, transform.up);
        Vector3 snowmanOnPlane = Vector3.ProjectOnPlane(transform.position, transform.up);
        
        if (Vector3.Distance(playerOnPlane, snowmanOnPlane) < .5f)
        {
            Debug.Log("Game over");
            FindObjectOfType<AudioManager>().Play("lose_sound");
            GameOver();
        }
        // Calculate the distance between player and snowman, excluding the vertical component
        Vector3 playerPos = player.transform.position;
        Vector3 snowmanPos = transform.position;
        playerPos.y = 0; // Ignore vertical component of player position
        snowmanPos.y = 0; // Ignore vertical component of snowman position
        float distanceWithoutY = Vector3.Distance(playerPos, snowmanPos);

        // Check if the distance between player and snowman (excluding vertical component) is less than 0.7
        // if (distanceWithoutY < 0.7f)
        // {
        //     Debug.Log("Game over");
        //     FindObjectOfType<AudioManager>().Play("lose_sound");
        //     GameOver();
        // }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger detected with " + other.name);
        if (other.gameObject.CompareTag("Snowball"))
        {
            Debug.Log("Snowman hit");
            Destroy(other.gameObject);
            IsHit();
        }
    }

    void IsHit()
    {
        numberOfHitsSnowman += 1;
        Debug.Log(numberOfHitsSnowman + " snowman hit");
        VFXFactory.Instance.GetVFX(VFXType.Spirits, transform.position);
        FindObjectOfType<AudioManager>().Play("snowball_hit");
        if (numberOfHitsSnowman == 3)
        {
            GameWon();
        }
        else
        {
            Vector3 initialPosition = GetInitialPositionFrontOfObstacles(numberOfHitsSnowman+1);
            // WITH COROUTINE scale the snowman to 0, then move it to the initial position and scale it back to 1
            StartCoroutine(MoveSnowman(initialPosition));
        }
    }

    IEnumerator<WaitForSeconds> MoveSnowman(Vector3 initialPosition)
    {
        canMove = false;
        float time = 0f;
        float duration = 2f;
        Vector3 initialScale = transform.localScale;
        Vector3 finalScale = Vector3.zero;
        GameObject vfx = VFXFactory.Instance.GetVFX(VFXType.ShieldLeave, transform.position);

        Vector3 initialScaleVFX = vfx.transform.localScale;
        Vector3 finalScaleVFX = Vector3.zero;
        while (time < duration)
        {
            transform.localScale = LerpWithEase(initialScale, finalScale, time / duration);
            transform.localPosition = LerpWithEase(transform.localPosition,
                new Vector3(transform.localPosition.x, transform.localPosition.y, 0), time / duration);

            if (vfx != null)
            {
                vfx.transform.localScale = LerpWithEase(initialScaleVFX, finalScaleVFX, time / duration);
                //change also the y position of the snowman so that it stays on the floor even with the scale
                vfx.transform.position = transform.position;
            }
            time += Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
        }


        this.transform.position = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z);
        this.transform.localPosition = new Vector3(transform.localPosition.x,
            transform.localPosition.y, /*transform.localPosition.z*/ -.5f);
        time = 0f;

         vfx = VFXFactory.Instance.GetVFX(VFXType.ShieldLeave, transform.position);
        while (time < duration)
        {
            Debug.Log("time: " + time);
            Debug.Log("duration: " + duration);
            transform.localScale = LerpWithEase(finalScale, initialScale, time / duration);
            transform.localPosition = LerpWithEase(transform.localPosition,
                new Vector3(transform.localPosition.x, transform.localPosition.y, .5f), time / duration);

            if (vfx != null)
            {
                vfx.transform.localScale = LerpWithEase(finalScaleVFX, initialScaleVFX, time / duration);
                vfx.transform.position = transform.position;
            }

            time += Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
        }

        canMove = true;
    }
    
    private float CubicEaseInOut(float t)
    {
        // Applying cubic easing function
        return t < 0.5f ? 4f * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
    }

    private Vector3 LerpWithEase(Vector3 start, Vector3 end, float t)
    {
        // Apply easing function to t
        float easedT = CubicEaseInOut(t);

        // Perform linear interpolation using the eased t
        return Vector3.Lerp(start, end, easedT);
    }

    IEnumerator<WaitForSeconds> MoveSnowmanStart(Vector3 initialPosition)
    {
        canMove = false;
        float time = 0f;
        float duration = 2f;
        Vector3 initialScale = transform.localScale;
        Vector3 finalScale = Vector3.zero;

        GameObject vfx = VFXFactory.Instance.GetVFX(VFXType.ShieldLeave, transform.position);
        
        Vector3 initialScaleVFX = vfx.transform.localScale;
        Vector3 finalScaleVFX = Vector3.zero;

        this.transform.position = initialPosition;
        this.transform.localPosition = new Vector3(transform.localPosition.x,
            transform.localPosition.y, /*transform.localPosition.z*/ -.5f);

        while (time < duration)
        {
            Debug.Log("time: " + time);
            Debug.Log("duration: " + duration);
            transform.localScale = LerpWithEase(finalScale, initialScale, time / duration);
            transform.localPosition = LerpWithEase(transform.localPosition,
                new Vector3(transform.localPosition.x, transform.localPosition.y, .5f), time / duration);

            if (vfx != null)
            {
                vfx.transform.localScale = LerpWithEase(finalScaleVFX, initialScaleVFX, time / duration);
                vfx.transform.position = transform.position;
            }

            time += Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
        }

        canMove = true;
    }

    void GameWon()
    {
        Debug.Log("Mini game successful!");
        FindObjectOfType<AudioManager>().Stop("wind");
        FindObjectOfType<AudioManager>().Play("chest_unlock");

        GameObject.FindWithTag("SnowParticle").gameObject.SetActive(false);
        var chests = FindObjectsOfType<Chest>(true);

        foreach (var chest in chests)
        {
            chest.gameObject.SetActive(true);
            chest.NextObject();
        }

        GameManager.Instance.currentObjectIndex++;

        VFXFactory.Instance.GetVFX(VFXType.Fireworks1, transform.position);
        
        Destroy(FindObjectOfType<SnowballSpawner>().gameObject);

        Destroy(gameObject); //destroy the snowman bc game is finished
    }

    void GameOver()
    {
        Start(); //game starts again
    }
}