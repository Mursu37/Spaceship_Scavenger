using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private Animator animator;
    public int doorNumber;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Open()
    {
        animator.SetBool("isOpen", true);
    }

    public void Close()
    {
        animator.SetBool("isOpen", false);
    }
}
