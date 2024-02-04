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
        Shader.SetGlobalTexture("_GlobalEffectRT", rt);
        Shader.SetGlobalFloat("_OrthographicCamSize", GetComponent<Camera>().orthographicSize);
    }

    private void Update()
    {
        if (target == null)
        {
            GameObject leftHand = GameObject.FindGameObjectWithTag("LeftHand1");
            GameObject rightHand = GameObject.FindGameObjectWithTag("RightHand1");

            if (leftHand == null && rightHand != null)
                target = rightHand.transform;
            else if (leftHand != null && rightHand == null)
                target = leftHand.transform;
            else if (leftHand != null && rightHand != null)
                target = leftHand.transform.position.y > rightHand.transform.position.y ? leftHand.transform : rightHand.transform;
            else
                return;
        }
        else
        {
            // if (target.transform.position.y > 0.05f)
            // {
                //Shader.SetGlobalVector("_Position", new Vector3(0,0,0));
                return;
            // }
        }
        transform.position = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        Shader.SetGlobalVector("_Position", transform.position);
    }


}