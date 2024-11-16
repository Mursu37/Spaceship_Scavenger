using System.Collections;
using UnityEngine;

public abstract class Switch : MonoBehaviour, IInteractable
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        animator.Play("TurnOn");
        AudioManager.PlayModifiedClipAtPoint("LeverPull", transform.position, 1, 1, 1, 1000);
        StartCoroutine(SwitchAction());
    }

    protected abstract IEnumerator SwitchAction();
}
