using System.Collections;
using UnityEngine;

public class CoreTeleporterEntrance : MonoBehaviour
{
    public int id;
    private Animator animator;
    private CoreTeleporterExit exit;
    private GravityGun gravityGun;
    private bool canMove = false;
    private bool coreInSoundPlayed = false;
    public bool canTeleport = true;

    private MixerController mixerController;

    [SerializeField] private GameObject teleporterExit;
    [SerializeField] private Transform core;
    [SerializeField] private Transform coreHolder;
    [SerializeField] private GameObject multitool;
    [SerializeField] private Checkpoint checkpoint;
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

        if (CheckpointManager.teleportersUsed.Contains(id))
        {
            canTeleport = false;
        }
    }

    private void Update()
    {
        float distanceToCore = Vector3.Distance(transform.position, core.position);

        if (canTeleport)
        {
            switch (currentState)
            {
                case TeleporterState.Idle:
                    if (distanceToCore < 4f)
                    {
                        animator.Play("DoorOpen");
                        currentState = TeleporterState.Opening;
                        if (!AudioManager.IsPlaying("TeleporterOpen"))
                        {
                            AudioManager.PlayModifiedClipAtPoint("TeleporterOpen", transform.position, 1, 1, 1, 1000);
                        }
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
                        StartCoroutine(TeleportCoreCoroutine());
                    }
                    break;

                case TeleporterState.Closing:
                    animator.Play("TeleporterOpen");
                    currentState = TeleporterState.Idle;
                    canTeleport = false;
                    break;
            }
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
        if (!AudioManager.IsPlaying("TeleporterCoreIn") && !coreInSoundPlayed)
        {
            AudioManager.PlayModifiedClipAtPoint("TeleporterCoreIn", transform.position, 1, 1, 1, 1000);
            coreInSoundPlayed = true;
        }
    }

    private bool HasCoreReachedTarget()
    {
        return Vector3.Distance(core.position, coreHolder.position) < 0.01f &&
               Quaternion.Angle(core.rotation, coreHolder.rotation) < 1f;
    }

    private IEnumerator TeleportCoreCoroutine()
    {

        if (!AudioManager.IsPlaying("TeleporterClose"))
        {
            AudioManager.PlayModifiedClipAtPoint("TeleporterClose", transform.position, 1, 1, 1, 1000);
        }
        yield return new WaitForSeconds(1f);

        animator.Play("DoorClose");
        yield return new WaitForSeconds(1f);

        if (!AudioManager.IsPlaying("TeleporterActiveHum"))
        {
            AudioManager.PlayModifiedClipAtPoint("TeleporterActiveHum", transform.position, 1, 1, 1, 1000);
        }

        // Save data
        checkpoint?.SaveCheckpointState(id);
        CheckpointManager.lastTeleportId = exit.id;
        CheckpointManager.teleportersUsed.Add(id);

        exit.StartTeleportation();

        yield return new WaitForSeconds(0.5f);

        currentState = TeleporterState.Closing;
    }
}
