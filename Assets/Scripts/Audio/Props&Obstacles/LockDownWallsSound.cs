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
            AudioManager.PlayModifiedClipAtPoint("BlastDoorsClosing",transform.position, 1, 1, 1, 2000, false);
            hasSoundPlayed = true; 
        }
    }

    private bool IsAnimationPlaying(string animationName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
        {
            return true;
        }
        return false;
    }
}
