using Unity.VisualScripting;
using UnityEngine;

public class CoreTeleporterExit : MonoBehaviour
{
    public int id;
    private Animator animator;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject core;
    public Transform coreHolder;
    private Vector3 targetPosition;
    private bool canMove = false;

    private MixerController mixerController;

    public enum TeleporterState
    {
        Idle,
        Teleporting,
        PlayerNear,
        CoreMoving,
        Closing
    }

    [HideInInspector] public TeleporterState currentState = TeleporterState.Idle;

    private void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();

        mixerController = FindObjectOfType<MixerController>(); //For changing audio mixer snapshots
    }

    private void Update()
    {
        switch (currentState)
        {
            case TeleporterState.Teleporting:
                animator.Play("TeleporterClose");
                animator.Play("DoorOpen");
                currentState = TeleporterState.PlayerNear;
                break;

            case TeleporterState.PlayerNear:
                if (Vector3.Distance(transform.position, player.position) < 4f)
                {
                    //TeleporterRelease();  <-- this has been changed to be triggered by a lever.
                }
                break;

            case TeleporterState.CoreMoving:
                if (Vector3.Distance(core.transform.position, targetPosition) > 0.01f)
                {
                    canMove = true;
                }
                else
                {
                    canMove = false;
                    currentState = TeleporterState.Closing;
                }
                break;

            case TeleporterState.Closing:
                animator.Play("DoorClose");
                if (!AudioManager.IsPlaying("TeleporterClose"))
                {
                    AudioManager.PlayModifiedClipAtPoint("TeleporterClose", transform.position, 1, 1, 1, 1000);
                }
                core.GetComponent<Collider>().enabled = true;
                Rigidbody coreRbFinal = core.GetComponent<Rigidbody>();
                if (coreRbFinal != null)
                {
                    coreRbFinal.constraints = RigidbodyConstraints.None;
                }

                core.GetComponent<EnergyCore>().heatIncreaseTime = 8f;

                currentState = TeleporterState.Idle;
                break;

            default:
                break;
        }
    }


    public void TeleporterRelease()
    {
        animator.Play("TeleporterOpen");
        if (!AudioManager.IsPlaying("TeleporterOpen"))
        {
            AudioManager.PlayModifiedClipAtPoint("TeleporterOpen", transform.position, 1, 1, 1, 1000);
        }
        targetPosition = core.transform.position + core.transform.right * -2f;
        currentState = TeleporterState.CoreMoving;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            core.transform.position = Vector3.Lerp(core.transform.position, targetPosition, 2f * Time.fixedDeltaTime);
        }
    }

    public void StartTeleportation()
    {
        if (currentState == TeleporterState.Idle)
        {
            Debug.Log("Teleportation started.");

            if (core != null)
            {
                core.transform.position = coreHolder.position;
                core.transform.rotation = coreHolder.rotation;

                currentState = TeleporterState.Teleporting;
                core.GetComponent<EnergyCore>().heatIncreaseTime = 16f;
            }
            else
            {
                Debug.LogError("Core was null.");
            }
        }
    }
}
