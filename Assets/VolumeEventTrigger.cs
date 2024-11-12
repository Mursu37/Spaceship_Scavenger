using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class VolumeEventTrigger : MonoBehaviour
{
    private Collider triggerVolume;

    //Define a UnityEvent, which will show up in the inspector
    [SerializeField]
    private UnityEvent onEventTriggered;

    private void Start()
    {
        triggerVolume = this.GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerEvent();
        }
    }

    //Public method to trigger the event
    public void TriggerEvent()
    {
        if (onEventTriggered != null)
        {
            onEventTriggered.Invoke();
        }
    }
}
