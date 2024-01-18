using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateParent : MonoBehaviour
{

    public void Deactivate()
    {
        //todo play vfx
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
