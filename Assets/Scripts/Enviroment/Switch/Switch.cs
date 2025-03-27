using System.Collections;
using UnityEngine;

public abstract class Switch : MonoBehaviour, IInteractable
{
    public int id;
    public bool turnedOn = false;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (CheckpointManager.switchesTurnedOn.Contains(id))
        {
            turnedOn = true;
            if (gameObject.GetComponent<SwitchLampColorChange>() != null)
            {
                gameObject.GetComponent<SwitchLampColorChange>().ChangeLightColor(2);
                animator.Play("OpenCover", 1, 1);
            }
            animator.Play("TurnOn", 0, 1);
        }
    }

    public virtual void Interact()
    {
        if (!turnedOn)
        {
            animator.Play("TurnOn");
            AudioManager.PlayModifiedClipAtPoint("LeverPull", transform.position, 1, 1, 1, 1000);
            StartCoroutine(SwitchAction());
            turnedOn = true;
        }
    }

    protected abstract IEnumerator SwitchAction();
}
