using Oculus.Interaction;
using UnityEngine;

public class SnowGlobe : MonoBehaviour, GameActiver
{
    public ParticleSystem snowfallParticleSystem;
    public ParticleSystem snowfallSnowballParticleSystem;

    [SerializeField] private float shakeThreshold = 0.5f;
    private float lastShakeTime;
    [SerializeField] private float shakeCooldown = 1.0f;
    void Start()
    {
        //snowfallParticleSystem.Stop();
        snowfallSnowballParticleSystem.Stop();
        if (snowfallParticleSystem == null)
        {
            //todo pas beau
            snowfallParticleSystem = (ParticleSystem) FindObjectOfType<ParticleSystemShapeFitter>().gameObject.GetComponent<ParticleSystem>();
        }
    }
    void Update()
    {
        // Check if the snow globe is being shaken and grabbed by the player
        if (IsShakeDetected() && GetComponent<Grabbable>().SelectingPointsCount >= 1)
        { 
            if (snowfallParticleSystem!= null && !snowfallParticleSystem.isPlaying)
            {
                snowfallParticleSystem.GetComponent<ParticleSystemShapeFitter>().enabled = true;
                snowfallParticleSystem.Play();
                DemoSnow demoSnow = FindFirstObjectByType<DemoSnow>(FindObjectsInactive.Include);
                demoSnow.gameObject.SetActive(true);
                demoSnow.enabled = true;
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

        if (velocity > shakeThreshold)
        {
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

    public void ActivateGame()
    {
        
    }
}