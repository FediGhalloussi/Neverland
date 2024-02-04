using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    private HapticManager _hapticManager;

    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        _hapticManager = FindObjectOfType<HapticManager>();
        _hapticManager.PlayCannonHaptic();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float d = (transform.position - player.position).magnitude;
        if (d < 2f)
        {
            float s = Mathf.Lerp(0.0f, 1.0f, 2 - d);
            _hapticManager.PlayChangingHaptics(s,OVRInput.Controller.Active);
        }

        _hapticManager.StopGrazeHaptic();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger detected with " + other.name);
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Game over");
            FindObjectOfType<Cannon>().GameOver();
            VFXFactory.Instance.GetVFX(VFXType.Impact, transform.position);
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Game over");
            FindObjectOfType<Cannon>().GameOver();
            VFXFactory.Instance.GetVFX(VFXType.Impact, transform.position);
            Destroy(gameObject);
        }
    }
}
