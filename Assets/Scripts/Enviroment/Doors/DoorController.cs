using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : CuttingPointManager
{
    private Animator animator;

    public bool doorOpened = false;

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        FindCuttableObjects(transform);
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
            
        }
    }

    private void Update()
    {
        if (AreCuttingPointsNull())
        {
            if (animator != null)
            {
                animator.Play("DoorOpening");
                if (!AudioManager.IsPlaying("DoorOpen") && !doorOpened)
                {
                    AudioManager.PlayModifiedClipAtPoint("DoorOpen", transform.position, 1, 1, 1, 500);
                }

                doorOpened = true;
            }
        }
    }
}
