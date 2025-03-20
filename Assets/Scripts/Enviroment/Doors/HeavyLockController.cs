using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyLockController : MonoBehaviour
{
    [SerializeField] GameObject[] locks;
    [SerializeField] DoorController doorController;

    private bool canCheck = false;

    // Update is called once per frame
    private void Update()
    {
        if (AreAllLocksNull() && !canCheck)
        {
            doorController.ForceOpen();
            canCheck = true;
        }
    }

    private bool AreAllLocksNull()
    {
        foreach (GameObject lockObj in locks)
        {
            if (lockObj != null)
            {
                return false;
            }
        }
        return true;
    }
}
