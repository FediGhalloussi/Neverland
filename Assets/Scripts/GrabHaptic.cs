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
            var retrievableItem = interactable.GetComponent<RetrievableItem>();
            if (retrievableItem != null)
            {
                retrievableItem.isBeingRetrieved = false;
            }
            hapticManager.PlayGrabHaptic(controller);
            var magnyfingGlass = interactable.GetComponent<MagnyfingGlass>();
            if (_interactor.transform.GetChild(0).CompareTag("LeftHand"))
            {

                if (magnyfingGlass)
                {
                    interactable.GetComponent<MagnyfingGlass>().holdingHand = MagnyfingGlass.Hand.Left;

                }
            }
            else
            {
                if (magnyfingGlass)
                {
                    interactable.GetComponent<MagnyfingGlass>().holdingHand = MagnyfingGlass.Hand.Right;
                }
            }
        };

        _interactor.WhenInteractableUnselected.Action += interactable =>
        {
            var magnyfingGlass = interactable.GetComponent<MagnyfingGlass>();
            if (magnyfingGlass)
            {
                interactable.GetComponent<MagnyfingGlass>().holdingHand = MagnyfingGlass.Hand.None;
            }
            
        };
        
        _interactor.WhenInteractableSet.Action += (delegate(GrabInteractable interactable)
        {
            hapticManager.PlayCanGrabHaptic(controller);
        });
    }

}
