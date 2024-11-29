using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElevatorTerminal : MonoBehaviour, IInteractable
{
    public UnityEvent interactEvent;
    private bool coreIsDelivered = false;


    public void SetCoreDelivered(bool _bool)
    {
        coreIsDelivered = _bool;
    }

    // Start is called before the first frame update
    public void Interact()
    {
        if (!coreIsDelivered)
        {
            return;
        }

        interactEvent.Invoke();
    }
}
