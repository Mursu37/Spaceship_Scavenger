using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoreEventDispatcher : EventDispatcher
{
    //Public method to trigger the event
   public new void TriggerEvent()
    {
        base.TriggerEvent();
    }
}
