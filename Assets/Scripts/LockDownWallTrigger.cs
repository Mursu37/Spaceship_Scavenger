using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDownWallTrigger : MonoBehaviour
{
    private bool opened = false;

    [SerializeField] private Animator animator;

    private void Update()
    {
        if (!opened)
        {
            if (CheckpointManager.checkpointReached)
            {
                animator.Play("Closing", 0, 1);
                opened = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !opened)
        {
            if (!CheckpointManager.checkpointReached)
            {
                if (FindObjectOfType<MeltdownPhase>().enabled)
                {
                    animator.Play("Closing");
                    opened = true;
                }
            }
        }
    }
}
