using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventDispatcher : MonoBehaviour
{

    //Define a UnityEvent, which will show up in the inspector
    [SerializeField]
    private UnityEvent onEventTriggered;

    //Public method to trigger the event
   public void TriggerEvent()
    {
        if (onEventTriggered != null)
        {
            onEventTriggered.Invoke();
        }
    }
}
