using System.Collections;
using UnityEngine;

public class CoreDisconnect : MonoBehaviour
{
    [SerializeField] private Transform targetPosition;
    private bool canMove = false;

    [SerializeField] private Animator animator;
    [SerializeField] private Animator coreAnimator;
    [SerializeField] private GameObject core;

    private enum State
    {
        Idle,
        Open,
        CoreMoving,
        Closing
    }

    private State currentState = State.Idle;

    private void Start()
    {
        animator.Play("TeleporterClose", 0, 1);
    }

    private void Update()
    {
        switch(currentState)
        {
            case State.Open:
                core.SetActive(true);
                StartCoroutine(Open());
                break;

            case State.CoreMoving:
                if (Vector3.Distance(core.transform.position, targetPosition.position) > 0.01f)
                {
                    canMove = true;
                }
                else
                {
                    canMove = false;
                    currentState = State.Closing;
                }
                break;
            case State.Closing:
                StartCoroutine(Close());
                break;
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            core.transform.position = Vector3.Lerp(core.transform.position, targetPosition.position, 2f * Time.fixedDeltaTime);
        }
    }

    private IEnumerator Open()
    {
        animator.Play("DoorOpen");
        yield return new WaitForSeconds(1f);
        animator.Play("TeleporterOpen");
        yield return new WaitForSeconds(1f);
        currentState = State.CoreMoving;
    }

    private IEnumerator Close()
    {
        coreAnimator.Play("Open");
        yield return new WaitForSeconds(1f);
        coreAnimator.Play("Rotate");
        core.GetComponent<Collider>().enabled = true;
        currentState = State.Idle;
    }

    public void Diconnect()
    {
        currentState = State.Open;
        if (!AudioManager.IsPlaying("TeleporterOpen"))
        {
            AudioManager.PlayModifiedClipAtPoint("TeleporterOpen", transform.position, 1, 1, 1, 1000);
        }
    }
}
