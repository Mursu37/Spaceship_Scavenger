using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPowerOn : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        PowerOn.isPowerOn = true;
    }
}
