using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchModel : MonoBehaviour
{

    [SerializeField] private Renderer firstModel;

    [SerializeField] private Renderer secondModel;

    public void Switch()
    {
        firstModel.enabled = false;
        secondModel.enabled = true;
    }
}
