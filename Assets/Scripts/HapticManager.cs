using System.Collections;
using System.Collections.Generic;
using Oculus.Haptics;
using UnityEngine;

public class HapticManager : MonoBehaviour
{

    [SerializeField] private HapticClip[] clips;
    private HapticClipPlayer player;
    void Awake()
    {
        player = new HapticClipPlayer(clips[0]);
        DontDestroyOnLoad(gameObject);
    }

    public void PlayGrabHaptic(OVRInput.Controller controller)
    {

        StartCoroutine(PlayHaptic(1.0f,0.5f,0.1f,controller));
    }

    public void PlayCannonHaptic()
    {
        player.Play(Controller.Both);
    }

    public void PlayCanGrabHaptic(OVRInput.Controller controller)
    {
        StartCoroutine(PlayHaptic(1.0f,0.4f,0.1f,controller));

    }

    private IEnumerator PlayHaptic(float frequency, float strength, float duration, OVRInput.Controller controller)
    {
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

}
