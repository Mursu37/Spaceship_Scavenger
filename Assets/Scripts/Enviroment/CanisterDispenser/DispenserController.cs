using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DispenserController : MonoBehaviour, IInteractable
{
    
    private GameObject newCanister;
    private bool canDispense;
    [SerializeField]
    private MeshRenderer meterRenderer;
    private Material meterMaterial;
    private Coroutine meterCoroutine;

    public bool isOpen;

    [SerializeField] private float meterUpdateSpeed;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject canister;
    [SerializeField] private Transform holder;

    [SerializeField] private GameObject gasLine;
    private enum DispenserState
    {
        Idle,
        Dispensing,
        Reloading
    }

    private DispenserState currentState;

    private void Start()
    {

        currentState = DispenserState.Idle;
        canDispense = true;
        isOpen = false;

        if (meterRenderer != null)
        {
            meterMaterial = meterRenderer.material;
        }

    }

    public void Interact()
    {
        if (currentState == DispenserState.Idle && canDispense)
        {
            AudioManager.PlayAudio("InteractBeep", 1, 1, false);
            StartDispense();
        }
    }

    private void StartDispense()
    {
        canDispense = false;
        newCanister = Instantiate(canister, holder.position, holder.rotation, holder);
        if(gasLine != null) gasLine.SetActive(false);

        if (newCanister.transform.GetChild(0).TryGetComponent<Rigidbody>(out var rb))
        {
            Destroy(rb);
        }
        
        animator.Play("ButtonPress");
        StartCoroutine(DispenseCanister());
    }

    private IEnumerator DispenseCanister()
    {
        yield return new WaitForSeconds(0.3f);

        AudioManager.PlayModifiedClipAtPoint("DispenserOpen", transform.position, 1, 1, 1, 1000, false);
        animator.Play("Eject");
        if (meterCoroutine != null)
        {
            StopCoroutine(meterCoroutine);
        }
        meterCoroutine = StartCoroutine(UpdateMeter(0f));
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

        AudioManager.PlayModifiedClipAtPoint("DispenserClose", transform.position, 1, 1, 1, 1000, false);
        animator.Play("Shut");
        yield return new WaitForSeconds(1f);

        isOpen = false;
        AudioManager.PlayModifiedClipAtPoint("DispenserReload", transform.position, 1, 1, 1, 1000, false);
        animator.Play("Reload");
        yield return new WaitForSeconds(3f);

        if (meterCoroutine != null)
        {
            StopCoroutine(meterCoroutine);
        }
        meterCoroutine = StartCoroutine(UpdateMeter(1f));
        currentState = DispenserState.Idle;
        canDispense = true;
        newCanister = null;
        if (gasLine != null) gasLine.SetActive(true);
    }

    private IEnumerator UpdateMeter(float targetFloat)
    {
        if (meterMaterial == null)
        {
            yield break; // Exit if no material is assigned
        }

        // Get the current value
        float currentFillAmount = meterMaterial.GetFloat("_FillAmount");

        // Loop until the value is close enough to the target
        while (!Mathf.Approximately(currentFillAmount, targetFloat))
        {
            // Gradually move towards the target
            currentFillAmount = Mathf.Lerp(
            currentFillAmount,
            targetFloat,
            meterUpdateSpeed * Time.deltaTime
            );

            // Update the material property
            meterMaterial.SetFloat("_FillAmount", currentFillAmount);

            // Wait for the next frame
            yield return null;
        }

        // Ensure the target value is set exactly at the end
        meterMaterial.SetFloat("_FillAmount", targetFloat);
    }

    private void Update()
    {
        if (currentState == DispenserState.Dispensing)
        {
            if (newCanister != null)
            {
                if (Vector3.Distance(newCanister.transform.GetChild(0).transform.position, holder.position) >= 0.5f)
                {
                    if (currentState != DispenserState.Reloading)
                    {
                        StartCoroutine(ReloadDispenser());
                    }
                }
            }
            else
            {
                if (currentState != DispenserState.Reloading)
                {
                    StartCoroutine(ReloadDispenser());
                }
            }
        }
    }
}
