using System.Collections;
using UnityEngine;

public class CoreTeleporterEntrance : MonoBehaviour
{
    private Animator animator;
    private CoreTeleporterExit exit;
    private GravityGun gravityGun;
    private bool canMove = false;

    private MixerController mixerController;

    [SerializeField] private GameObject teleporterExit;
    [SerializeField] private Transform core;
    [SerializeField] private Transform coreHolder;
    [SerializeField] private GameObject multitool;

    private enum TeleporterState
    {
        Idle,
        Opening,
        CoreApproaching,
        Teleporting,
        Closing
    }

    private TeleporterState currentState = TeleporterState.Idle;

    private void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();

        mixerController = FindObjectOfType<MixerController>(); //For changing audio mixer snapshots

        if (multitool != null)
        {
            gravityGun = multitool.GetComponent<GravityGun>();
        }

        if (teleporterExit != null)
        {
            exit = teleporterExit.GetComponent<CoreTeleporterExit>();
        }
    }

    private void Update()
    {
        float distanceToCore = Vector3.Distance(transform.position, core.position);

        switch (currentState)
        {
            case TeleporterState.Idle:
                if (distanceToCore < 4f)
                {
                    animator.Play("DoorOpen");
                    currentState = TeleporterState.Opening;
                }
                break;

            case TeleporterState.Opening:
                if (distanceToCore < 2f)
                {
                    currentState = TeleporterState.CoreApproaching;
                    gravityGun.isAttracting = false;
                    core.GetComponent<Collider>().enabled = false;
                    Rigidbody coreRb = core.GetComponent<Rigidbody>();
                    if (coreRb != null)
                    {
                        coreRb.constraints = RigidbodyConstraints.FreezeAll;
                    }
                }
                break;

            case TeleporterState.CoreApproaching:
                canMove = true;
                
                if (HasCoreReachedTarget())
                {
                    currentState = TeleporterState.Teleporting;
                    animator.Play("TeleporterClose");
                    canMove = false;
                    mixerController?.LowPassMusicTransition(); //Change music when core is in teleporter
                    StartCoroutine(TeleportCoreCoroutine());
                }
                break;

            case TeleporterState.Closing:
                animator.Play("TeleporterOpen");
                currentState = TeleporterState.Idle;
                break;
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            MoveCoreToHolder();
        }
    }

    private void MoveCoreToHolder()
    {
        core.position = Vector3.Lerp(core.position, coreHolder.position, 2f * Time.fixedDeltaTime);
        core.rotation = Quaternion.Lerp(core.rotation, coreHolder.rotation, 5f * Time.fixedDeltaTime);
    }

    private bool HasCoreReachedTarget()
    {
        return Vector3.Distance(core.position, coreHolder.position) < 0.01f &&
               Quaternion.Angle(core.rotation, coreHolder.rotation) < 1f;
    }

    private IEnumerator TeleportCoreCoroutine()
    {
        yield return new WaitForSeconds(1f);

        animator.Play("DoorClose");
        yield return new WaitForSeconds(1f);

        exit.StartTeleportation();
        core.position = exit.coreHolder.position;
        core.rotation = exit.coreHolder.rotation;

        yield return new WaitForSeconds(0.5f);

        currentState = TeleporterState.Closing;
    }
}
