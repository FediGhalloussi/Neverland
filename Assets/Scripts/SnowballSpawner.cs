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
        if (other.CompareTag("RightHand"))
        {
            FindObjectOfType<HapticManager>().PlayCanGrabHaptic(OVRInput.Controller.RTouch);
        }
        else if (other.CompareTag("LeftHand"))
        {
            FindObjectOfType<HapticManager>().PlayCanGrabHaptic(OVRInput.Controller.LTouch);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
        
        if (other.CompareTag("RightHand") && (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger) || OVRInput.Get(OVRInput.Button.SecondaryHandTrigger)) && !hasInstantiatedSnowball)
        {
            // Instantiate the snowball with a smaller initial scale
            GameObject snowballInstance = Instantiate(snowballPrefab, other.transform.position, Quaternion.identity);
            snowballInstance.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f); // Set the initial scale as per your preference

            // Start a coroutine to gradually increase the snowball's scale only when moving
            StartCoroutine(IncreaseSnowballScale(snowballInstance, OVRInput.Controller.RTouch));

            hasInstantiatedSnowball = true;
            Debug.Log("Spawning snowball!");
        }
        
        if (other.CompareTag("LeftHand") && OVRInput.Get(OVRInput.Button.PrimaryHandTrigger) && !hasInstantiatedSnowball)
        {
            Debug.Log("Trigger detected with LEFT" + other.name);

            // Instantiate the snowball with a smaller initial scale
            GameObject snowballInstance = Instantiate(snowballPrefab, other.transform.position, Quaternion.identity);
            snowballInstance.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f); // Set the initial scale as per your preference

            // Start a coroutine to gradually increase the snowball's scale only when moving
            StartCoroutine(IncreaseSnowballScale(snowballInstance, OVRInput.Controller.LTouch));

            hasInstantiatedSnowball = true;
            Debug.Log("Spawning snowball!");
        }
    }

    private IEnumerator IncreaseSnowballScale(GameObject snowball, OVRInput.Controller controller)
    {
        float duration = 2.0f;
        float scaleFactor = 5f;

        float elapsedTime = 0f;
        Vector3 initialScale = snowball.transform.localScale;
        Debug.Log("initialScale " + initialScale);
        
        while (elapsedTime < duration)
        {
            if (snowball == null)
            {
                yield break;
            }
            if (OVRInput.GetLocalControllerVelocity(controller).magnitude > 0.1f)
            {
                // snowball.transform.localScale += initialScale * scaleFactor * Time.deltaTime / duration;
                snowball.transform.localScale += (initialScale * scaleFactor - initialScale) * 3f * Time.deltaTime * OVRInput.GetLocalControllerVelocity(controller).magnitude;
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
        StartCoroutine(LerpToSnowAmount(4));
    }

    private void OnDisable()
    {
        //lerp material to opacity 0
        StartCoroutine(LerpMaterialToTransparent());
        //destroy all snowballs
        GameObject[] snowballs = GameObject.FindGameObjectsWithTag("Snowball");
        foreach (GameObject snowball in snowballs)
        {
            Destroy(snowball);
        }
        
    }

    private IEnumerator LerpToScale(Vector3 finalScale, float duration = 3f)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            gameObject.transform.localScale = Vector3.Lerp(Vector3.zero, finalScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        gameObject.transform.localScale = finalScale;
    }
    
    private IEnumerator LerpToSnowAmount(float duration = 3f)
    {
        float elapsedTime = 0f;
        Renderer renderer = GetComponent<Renderer>();
        Material material = renderer.material;
        float initialSnowAmount = 0;
        float targetSnowAmount = 1f; // Set your target snow amount here

        while (elapsedTime < duration)
        {
            float currentSnowAmount = Mathf.Lerp(initialSnowAmount, targetSnowAmount, elapsedTime / duration);
            material.SetFloat("_SnowAmount", currentSnowAmount);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final snow amount is set correctly
        material.SetFloat("_SnowAmount", targetSnowAmount);
    }
    
    private IEnumerator LerpMaterialToTransparent()
    {
        float duration = 3f;
        float elapsedTime = 0f;
        Renderer renderer = GetComponent<Renderer>();
        Color initialColor = renderer.material.color;
        while (elapsedTime < duration)
        {
            renderer.material.color = Color.Lerp(initialColor, Color.clear, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        renderer.material.color = Color.clear;
    }
}