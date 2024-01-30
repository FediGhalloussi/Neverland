using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInteractiveShaderEffects : MonoBehaviour
{
    [SerializeField]
    RenderTexture rt;
    [SerializeField]
    Transform target;
    
    // Start is called before the first frame update
    void Awake()
    {
        if (GameManager.Instance.floor != null)
        target = GameManager.Instance.floor.gameObject.transform;
        // target = GameObject.FindGameObjectsWithTag("RightHand")[0].transform;
        Shader.SetGlobalTexture("_GlobalEffectRT", rt);
        Shader.SetGlobalFloat("_OrthographicCamSize", GetComponent<Camera>().orthographicSize);
    }

    private void Update()
    {
        if (target != null)
        {
            transform.position = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            Shader.SetGlobalVector("_Position", transform.position);        
        }
        else if (GameManager.Instance.floor != null)
        {
            target = GameManager.Instance.floor.gameObject.transform;
        }
    }


}