using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DispenserController : MonoBehaviour, IInteractable
{
    private Animator animator;
    private GameObject newCanister;
    private bool canDispense;

    public bool isOpen;
    [SerializeField] private GameObject canister;
    [SerializeField] private Transform holder;

    private enum DispenserState
    {
        Idle,
        Dispensing,
        Reloading
    }

    private DispenserState currentState;

    private void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();

        currentState = DispenserState.Idle;
        canDispense = true;
        isOpen = false;
    }

    public void Interact()
    {
        if (currentState == DispenserState.Idle && canDispense)
        {
            StartDispnese();
        }
    }

    private void StartDispnese()
    {
        canDispense = false;
        newCanister = Instantiate(canister, holder.position, holder.rotation, holder);

        if (newCanister.transform.GetChild(0).TryGetComponent<Rigidbody>(out var rb))
        {
            Destroy(rb);
        }

        animator.Play("Eject");
        StartCoroutine(DispenseCanister());
    }

    private IEnumerator DispenseCanister()
    {
        yield return new WaitForSeconds(1f);

        newCanister.transform.SetParent(null);

        Rigidbody rb = newCanister.transform.GetChild(0).AddComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        currentState = DispenserState.Dispensing;
        isOpen = true;
    }

    private IEnumerator ReloadDispenser()
    {
        currentState = DispenserState.Reloading;

        animator.Play("Shut");
        yield return new WaitForSeconds(1f);

        isOpen = false;
        animator.Play("Reload");
        yield return new WaitForSeconds(3f);

        currentState = DispenserState.Idle;
        canDispense = true;
        newCanister = null;
    }

    private void Update()
    {
        if (currentState == DispenserState.Dispensing && newCanister != null)
        {
            if (Vector3.Distance(newCanister.transform.GetChild(0).transform.position, holder.position) >= 0.5f)
            {
                if (currentState != DispenserState.Reloading)
                {
                    StartCoroutine(ReloadDispenser());
                }
            }
        }
    }
}
