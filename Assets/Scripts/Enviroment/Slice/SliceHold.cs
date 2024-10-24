using EzySlice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceHold : MonoBehaviour
{
    private SliceManager sliceManager;
    private List<Slice> collidedSliceObjects;
    private bool hasCollided = false;

    private GameObject multitool;

    private void Start()
    {
        multitool = GameObject.Find("Multitool");
        if (multitool != null )
        {
            sliceManager = multitool.GetComponent<SliceManager>();
        }

        collidedSliceObjects = new List<Slice>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (sliceManager.isUpdating)
        {
            Debug.Log("Collision happened.");
            foreach (Slice slice in sliceManager.sliceObjects)
            {
                if (slice != null && slice.gameObject == other.gameObject)
                {
                    slice.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
        }
    }
}
