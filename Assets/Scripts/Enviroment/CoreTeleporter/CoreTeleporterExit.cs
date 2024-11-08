using UnityEngine;

public class CoreTeleporterExit : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject core;
    public Transform coreHolder;
    private Vector3 targetPosition;

    private enum TeleporterState
    {
        Idle,
        Teleporting,
        PlayerNear,
        CoreMoving,
        Closing
    }

    private TeleporterState currentState = TeleporterState.Idle;

    private void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
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
                if (Vector3.Distance(transform.position, player.position) < 5f)
                {
                    animator.Play("TeleporterOpen");
                    targetPosition = core.transform.position + core.transform.right * -2f;
                    currentState = TeleporterState.CoreMoving;
                }
                break;

            case TeleporterState.CoreMoving:
                if (Vector3.Distance(core.transform.position, targetPosition) > 0.01f)
                {
                    core.transform.position = Vector3.Lerp(core.transform.position, targetPosition, 4f * Time.deltaTime);
                }
                else
                {
                    currentState = TeleporterState.Closing;
                }
                break;

            case TeleporterState.Closing:
                animator.Play("DoorClose");
                core.GetComponent<Collider>().enabled = true;
                Rigidbody coreRbFinal = core.GetComponent<Rigidbody>();
                if (coreRbFinal != null)
                {
                    coreRbFinal.constraints = RigidbodyConstraints.None;
                }
                currentState = TeleporterState.Idle;
                break;

            default:
                break;
        }
    }

    public void StartTeleportation()
    {
        if (currentState == TeleporterState.Idle)
        {
            currentState = TeleporterState.Teleporting;
        }
    }
}
