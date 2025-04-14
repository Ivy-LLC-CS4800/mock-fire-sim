using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour, IPickable
{
    [field: SerializeField]
    public bool KeepWorldPosition { get; private set; }

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public GameObject PickUp()
    {
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(270f, 0f, 340f);
        transform.localScale = Vector3.one;
        return this.gameObject;
    }

    public void OnTriggerEnter(Collider other)
    {
        IUseableFloor floorItem = other.GetComponent<IUseableFloor>();
        if(floorItem != null){
            floorItem.Use(this.gameObject);
        }        
    }
}