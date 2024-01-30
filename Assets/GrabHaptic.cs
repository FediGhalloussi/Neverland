using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class GrabHaptic : MonoBehaviour
{
    private GrabInteractor _interactor;

    [SerializeField] private OVRInput.Controller controller;
    
    //Start is called before the first frame update
    void Start()
    {
        HapticManager hapticManager = FindObjectOfType<HapticManager>();
        _interactor = GetComponent<GrabInteractor>();

        _interactor.WhenInteractableSelected.Action += interactable =>
        {
            hapticManager.PlayGrabHaptic(controller);
        };
        
        _interactor.WhenInteractableSet.Action += (delegate(GrabInteractable interactable)
        {
            hapticManager.PlayCanGrabHaptic(controller);
        });
    }

}
