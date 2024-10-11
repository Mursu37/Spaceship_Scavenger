using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpening : MonoBehaviour
{
    private Animator _doorAnimator;
   
    void Start()
    {
        _doorAnimator = GetComponent<Animator>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _doorAnimator.SetTrigger("Open");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _doorAnimator.SetTrigger("Closed");
        }
   
    }
}
