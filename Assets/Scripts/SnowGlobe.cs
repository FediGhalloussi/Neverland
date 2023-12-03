using Oculus.Interaction;
using UnityEngine;

public class SnowGlobe : MonoBehaviour
{
    public ParticleSystem snowfallParticleSystem;
    public ParticleSystem snowfallSnowballParticleSystem;

    [SerializeField] private float shakeThreshold = 0.5f;
    private float lastShakeTime;
    [SerializeField] private float shakeCooldown = 1.0f;
    void Start()
    {
        snowfallParticleSystem.Stop();
        snowfallSnowballParticleSystem.Stop();
        
        //TODO ONLY FOR TESTING PURPOSES - REMOVE LATER 
        //transform.position = FindObjectOfType<OVRCameraRig>().transform.position + FindObjectOfType<OVRCameraRig>().transform.up * .5f;
    }
    void Update()
    {
        // Check if the snow globe is being shaken and grabbed by the player
        if (IsShakeDetected() && GetComponent<Grabbable>().SelectingPointsCount >= 1)
        { 
            if (!snowfallParticleSystem.isPlaying)
            {
                snowfallParticleSystem.GetComponent<ParticleSystemShapeFitter>().enabled = true;
                snowfallParticleSystem.Play();
                FindFirstObjectByType<DemoSnow>(FindObjectsInactive.Include).gameObject.SetActive(true);
                //FIND object with tag
                
                
            }
        }
    }
    float shakeStartTime = 0.0f;

    bool IsShakeDetected()
    {
        Vector3 currentPos = transform.position;
        float velocity = (currentPos - lastPosition).magnitude / Time.deltaTime;
        lastPosition = currentPos;
        Debug.Log("velocity: " + velocity);

        if (velocity > shakeThreshold)
        {
            Debug.Log("Shake detected");
            if (snowfallSnowballParticleSystem.isStopped)
            {
                snowfallSnowballParticleSystem.Play();
            }
            if (shakeStartTime == 0.0f)
            {
                // Start the timer when shaking is detected
                shakeStartTime = Time.time;
            }
        }
        else
        {
            Debug.Log("Shake not detected");
            if (snowfallSnowballParticleSystem.isPlaying)
            {
                snowfallSnowballParticleSystem.Stop();
            }
            // Reset the timer when shaking stops
            shakeStartTime = 0.0f;
        }

        // Check if the timer has reached 2 seconds
        if (shakeStartTime > 0.0f && Time.time > shakeStartTime + .7f)
        {
            // Reset the timer for the next shake
            shakeStartTime = 0.0f;

            // Return true after at least 2 seconds of shaking
            return true;
        }

        return false;
    }

    private Vector3 lastPosition;

    void OnEnable()
    {
        // Store the initial position when enabled
        lastPosition = transform.position;
    }

    void OnDisable()
    {
        // Reset lastPosition when disabled
        lastPosition = Vector3.zero;
    }
}