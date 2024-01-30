using System;
using System.Collections;
using Oculus.Haptics;
using UnityEngine;

public class SnowballSpawner : MonoBehaviour
{
    public GameObject snowballPrefab;
    public float offsetFloor = 5f;
    public bool hasInstantiatedSnowball = false;

    private void Start()
    {
        Vector3 normal = GetComponentInParent<OVRScenePlane>().gameObject.transform.forward;
        gameObject.transform.position =
            GetComponentInParent<OVRScenePlane>().gameObject.transform.position + normal * offsetFloor;
        Debug.Log("Snowball spawner ON");
    }

    private void Update()
    {
        if (hasInstantiatedSnowball && !(OVRInput.Get(OVRInput.Button.PrimaryHandTrigger) ||
                                         OVRInput.Get(OVRInput.Button.SecondaryHandTrigger)))
        {
            hasInstantiatedSnowball = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            FindObjectOfType<HapticManager>().PlayCanGrabHaptic(OVRInput.Controller.Active);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Hand") && (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger) || OVRInput.Get(OVRInput.Button.SecondaryHandTrigger)) && !hasInstantiatedSnowball)
        {
            Debug.Log("Trigger detected with hand");

            // Instantiate the snowball with a smaller initial scale
            GameObject snowballInstance = Instantiate(snowballPrefab, other.transform.position, Quaternion.identity);
            snowballInstance.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f); // Set the initial scale as per your preference

            // Start a coroutine to gradually increase the snowball's scale only when moving
            StartCoroutine(IncreaseSnowballScale(snowballInstance));

            hasInstantiatedSnowball = true;
            Debug.Log("Spawning snowball!");
        }
    }

    private IEnumerator IncreaseSnowballScale(GameObject snowball)
    {
        float duration = 2.0f;
        float scaleFactor = 5f;

        float elapsedTime = 0f;
        Vector3 initialScale = snowball.transform.localScale;
        Debug.Log("initialScale " + initialScale);
        
        while (elapsedTime < duration)
        {
            if (OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch).magnitude > 0.1f)
            {
                // snowball.transform.localScale += initialScale * scaleFactor * Time.deltaTime / duration;
                snowball.transform.localScale += (initialScale * scaleFactor - initialScale) * 3f * Time.deltaTime * OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch).magnitude;
                // snowball.transform.localScale = Vector3.Lerp(initialScale, initialScale * scaleFactor, elapsedTime / duration);
                if (snowball.transform.localScale.x > initialScale.x * scaleFactor)
                {
                    snowball.transform.localScale = initialScale * scaleFactor;
                }
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // snowball.transform.localScale = initialScale * scaleFactor; // Ensure the final scale is set correctly
    }

    private void OnEnable()
    {
        //save current scale,, put scale to 0,0,0 and then lerp to saved scale
        Vector3 initialScale = gameObject.transform.localScale;
        gameObject.transform.localScale = Vector3.zero;
        StartCoroutine(LerpToScale(initialScale));
    }
    
    private IEnumerator LerpToScale(Vector3 finalScale)
    {
        float duration = 3f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            gameObject.transform.localScale = Vector3.Lerp(Vector3.zero, finalScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        gameObject.transform.localScale = finalScale;
    }
}