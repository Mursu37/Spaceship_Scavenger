using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : CuttingPointManager
{
    private Animator animator;

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
        }
    }

    private void Update()
    {
        if (AreCuttingPointsNull())
        {
            if (animator != null)
            {
                animator.Play("DoorOpening");
            }
        }
    }
}
