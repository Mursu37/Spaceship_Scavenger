using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActivateDispenser : MonoBehaviour, IInteractable
{
    private Animator animator;
    private GameObject newCanister;

    [SerializeField] private GameObject canister;
    [SerializeField] private Transform capsule;

    private enum DispenserState
    {
        Idle,
        Shut
    }

    private DispenserState currentState = DispenserState.Idle;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        newCanister = Instantiate(canister, capsule.position, capsule.rotation, capsule);
        Destroy(newCanister.transform.GetChild(0).GetComponent<Rigidbody>());
        animator.Play("Eject");
        //StartCoroutine(DispenseCanister());
    }

    private IEnumerator DispenseCanister()
    {
        yield return new WaitForSeconds(1f);
        

    }

    private void Update()
    {
        switch(currentState)
        {
            case DispenserState.Idle:
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    newCanister.transform.GetChild(0).AddComponent<Rigidbody>();
                    currentState = DispenserState.Shut;
                }
                break;
            case DispenserState.Shut:
                if (Vector3.Distance(newCanister.transform.position, capsule.position) >= 2f)
                {
                    animator.Play("Shut");
                    currentState = DispenserState.Idle;
                }
                break;
        }
    }
}
