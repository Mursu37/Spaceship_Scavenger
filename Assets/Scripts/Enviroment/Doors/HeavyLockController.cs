using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyLockController : MonoBehaviour
{
    private List<GameObject> locks;
    [SerializeField] DoorController doorController;

    private bool canCheck = false;

    private void Start()
    {
        AddDescendantsWithTag(transform, "Lock", locks);
    }

    // Update is called once per frame
    private void Update()
    {
        if (AreAllLocksNull() && !canCheck)
        {
            doorController.ForceOpen();
            canCheck = true;
        }
    }

    private void AddDescendantsWithTag(Transform parent, string tag, List<GameObject> list)
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.tag == tag)
            {
                list.Add(child.gameObject);
            }
            AddDescendantsWithTag(child, tag, list);
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
