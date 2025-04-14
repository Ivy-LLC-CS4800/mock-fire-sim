using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask pickableLayerMask;
    [SerializeField] private LayerMask useableLayerMask;
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private GameObject pickUpUI;
    [SerializeField] private GameObject useableUI;
    [SerializeField] [Min(1)] private float hitRange = 3;
    [SerializeField] private Transform pickUpParent;
    [SerializeField] private GameObject inHandItem;
    [SerializeField] private InputActionReference interactionInput, dropInput, useInput;
    private RaycastHit pickableHit;
    private RaycastHit useableHit;
    private Collider lastHit = null;


    private void Start()
    {
        //Starting causes interaction system to assign functions
        interactionInput.action.performed += PickUp;
        dropInput.action.performed += Drop;
        useInput.action.performed += Use;
    }

    //Funtion meant to interaction one object in hand with one in sim
    private void Use(InputAction.CallbackContext obj){
        if(useableHit.collider != null){
            if(inHandItem != null){
                IUseableFloor usable = useableHit.collider.GetComponent<IUseableFloor>();
                //If the object has a Use() function, calls it
                if (usable != null){
                    usable.Use(inHandItem);
                } 
                else{
                    Debug.Log("Hit object not implementing IUseableFloor");
                }
            }
            else{
                Debug.Log("No held item");
            }
        }
        else {
            Debug.Log("No useable objects");
        }
    }

    //Function to drop any item currently in inHandItem slot, does nothing if empty
    private void Drop(InputAction.CallbackContext obj){
        if (inHandItem != null){
            //Drops object at inHandItem slot position, maybe put it slightly in front of character
            inHandItem.transform.SetParent(null);
            //If object has a rigid body, make the object be affected by physics once dropped
            Rigidbody rb = inHandItem.GetComponent<Rigidbody>();
            if (rb != null){
                rb.isKinematic = false;
            }
            inHandItem = null;
        }
    }

    //Function to pick up an object in the interactable layer
    //Object must include script IPickupableItem
    private void PickUp(InputAction.CallbackContext obj){
        //Players need to be actively looking at object and have their hands slot free to pick up
        if(pickableHit.collider != null && inHandItem == null){
            IPickable pickableItem = pickableHit.collider.GetComponent<IPickable>();
            if (pickableItem != null){
                //Places object in hand slot & sets slot to parent so object moves with player
                inHandItem = pickableItem.PickUp();
                inHandItem.transform.SetParent(pickUpParent.transform, pickableItem.KeepWorldPosition);
            }
        }
    }

    //Function to detect if the camera is looking at an interactable object
    //Uses raycast to check every second
    //IF PickUpUI NOT FILLED RAYCAST DOES NOT UPDATE
    private void Update(){
        //Line below shows the raycast line but can be removed without issue
<<<<<<< HEAD
        //Keeps the raycast in front of the camera
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out useableHit, hitRange, useableLayerMask)){
            Debug.DrawRay(playerCameraTransform.position, playerCameraTransform.forward * hitRange, Color.blue);
            if(inHandItem != null){
                useableUI.SetActive(true);
                return;
            }
        } 
        else{
            useableUI.SetActive(false);
        }
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out pickableHit, hitRange, pickableLayerMask)){
            Debug.DrawRay(playerCameraTransform.position, playerCameraTransform.forward * hitRange, Color.red);
            pickUpUI.SetActive(true);
        
            var script = pickableHit.collider.GetComponent<Outline>();
            if(script != null){
                if(lastHit != null){
                    var lastScript = lastHit.GetComponent<Outline>();
                    if(lastScript != null){
                        lastScript.enabled = false;
                    }
                }
                script.enabled = true;
                lastHit = pickableHit.collider;
            } 
        }
        else{
            pickUpUI.SetActive(false);
            if(lastHit != null){
                var lastScript = lastHit.GetComponent<Outline>();
                if(lastScript != null){
                    lastScript.enabled = false;
                }
                lastHit = null;
            }
=======
        Debug.DrawRay(playerCameraTransform.position, playerCameraTransform.forward * hitRange, Color.red);
        //Shows the pickUpUI if the raycast detect something
        if (hit.collider != null){
            hit.collider.GetComponent<Highlight>()?.ToggleHighlight(false);
            pickUpUI.SetActive(false);
>>>>>>> 803b8a7 (test)
        }
        if (inHandItem != null){
            return;
        }
<<<<<<< HEAD
=======
        //Keeps the raycast in front of the camera
        if (Physics.Raycast(
            playerCameraTransform.position, 
            playerCameraTransform.forward, 
            out hit, 
            hitRange, 
            pickableLayerMask))
        {
            hit.collider.GetComponent<Highlight>()?.ToggleHighlight(true);
            pickUpUI.SetActive(true);
        }
>>>>>>> 803b8a7 (test)
    }
}
