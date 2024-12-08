using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : CuttingPointManager
{
    private Animator animator;

    public int id;
    public bool doorOpened = false;

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        FindCuttableObjects(transform);

        if (CheckpointManager.doorsOpened.Contains(id))
        {
            doorOpened = true;
            animator.Play("DoorOpening", 0, 1);
            DestroyCuttingPoints();
        }
    }

    public void HackOpen()
    {
        if (animator != null)
        {
            animator.Play("DoorOpening");
            if (!AudioManager.IsPlaying("DoorOpen"))
            {
                AudioManager.PlayModifiedClipAtPoint("DoorOpen", transform.position, 1, 1, 1, 500);
            }

            doorOpened = true;
            gameObject.GetComponent<AddArrayToXray>().SetGroupToLayer(0);
            DestroyCuttingPoints();
        }
    }

    private void OpenDoor()
    {
        if (AreCuttingPointsNull() && !doorOpened)
        {
            if (animator != null)
            {
                animator.Play("DoorOpening");
                if (!AudioManager.IsPlaying("DoorOpen"))
                {
                    AudioManager.PlayModifiedClipAtPoint("DoorOpen", transform.position, 1, 1, 1, 500);
                }
            }

            gameObject.GetComponent<AddArrayToXray>().SetGroupToLayer(0);
            doorOpened = true;
        }
    }

    public void ForceOpen()
    {
        DestroyCuttingPoints();
    }

    private void Update()
    {
        OpenDoor();
    }
}
