using System.Collections;
using System.Collections.Generic;
using Oculus.Haptics;
using UnityEngine;

public class HapticManager : MonoBehaviour
{

    [SerializeField] private HapticClip[] clips;
    private HapticClipPlayer player;
    private bool grazedHaptics = false;
    void Awake()
    {
        player = new HapticClipPlayer(clips[0]);
        DontDestroyOnLoad(gameObject);
    }

    public void PlayGrabHaptic(OVRInput.Controller controller)
    {
        grazedHaptics = false;
        StartCoroutine(PlayHaptic(1.0f,0.5f,0.1f,controller));
    }

    public void PlayCannonHaptic()
    {
        grazedHaptics = false;
        player.Play(Controller.Both);
    }

    public void PlayFearHaptic(OVRInput.Controller controller)
    {
        grazedHaptics = false;
        StartCoroutine(PlayHaptic(1.0f,1.0f,1f,controller,1f));
    }
    public void PlayCanGrabHaptic(OVRInput.Controller controller)
    {
        grazedHaptics = false;
        StartCoroutine(PlayHaptic(1.0f,0.04f,0.15f,controller));

    }

    private IEnumerator PlayHaptic(float frequency, float strength, float duration, OVRInput.Controller controller, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        OVRInput.SetControllerVibration(frequency,strength,controller);
        yield return new WaitForSeconds(duration);
        OVRInput.SetControllerVibration(frequency,0,controller);
    }
    
    public void StopHaptics()
    {
        OVRInput.SetControllerVibration(0,0);
    }

    void OnDestroy()
    {
        
    }

    public void PlayChangingHaptics(float strength,OVRInput.Controller controller)
    {
        grazedHaptics = true;
        OVRInput.SetControllerVibration(1.0f,strength,controller);
    }

    
    // stop haptics only if last haptics was cause by a cannonball grazing the player
    public void StopGrazeHaptic()
    {
        grazedHaptics = false;
        StopHaptics();
    }
}
