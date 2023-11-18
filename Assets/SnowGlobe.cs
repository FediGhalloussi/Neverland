using UnityEngine;

public class SnowGlobe : MonoBehaviour
{
    public ParticleSystem snowfallParticleSystem;

    [SerializeField] private float shakeThreshold = 0.5f;
    private float lastShakeTime;
    [SerializeField] private float shakeCooldown = 1.0f;

    void Start()
    {
        snowfallParticleSystem = FindObjectOfType<ParticleSystem>();
    }
    void Update()
    {
        // Check if the snow globe is being shaken
        if (IsShakeDetected())
        {
            snowfallParticleSystem.Play();
        }
    }

    bool IsShakeDetected()
    {
        Vector3 currentPos = transform.position;
        float velocity = (currentPos - lastPosition).magnitude / Time.deltaTime;

        if (velocity > shakeThreshold && Time.time > lastShakeTime + shakeCooldown)
        {
            lastShakeTime = Time.time;
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