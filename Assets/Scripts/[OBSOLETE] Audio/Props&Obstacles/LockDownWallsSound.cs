using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDownWallsSound : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private bool hasSoundPlayed = false;

    private void Update()
    {
        if (IsAnimationPlaying("Closing") && !hasSoundPlayed)
        {
            hasSoundPlayed = true; 
        }
    }

    private bool IsAnimationPlaying(string animationName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animationName) && !CheckpointManager.checkpointReached)
        {
            return true;
        }
        return false;
    }
}
