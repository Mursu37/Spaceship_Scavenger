using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDownWallTrigger : MonoBehaviour
{
    [SerializeField] private Animator animator;


    private void OnTriggerEnter(Collider other)
    {
        if (FindObjectOfType<MeltdownPhase>().enabled)
        animator.Play("Closing");
    }
}
