using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreTeleporterEntrance : MonoBehaviour
{
    private Animator animator;
    private GravityGun gravityGun;

    [SerializeField] private Transform teleportExit;
    [SerializeField] private Transform core;
    [SerializeField] private Transform coreHolder;
    [SerializeField] private GameObject multitool;

    private bool isCoreTeleported = false;
    private bool isTeleporterOpen = false;
    private bool isTeleporting = false;
    private bool canGrapple = false;
    private bool hasOpened = false;

    private void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();

        if (multitool != null)
        {
            gravityGun = multitool.GetComponent<GravityGun>();
        }
    }

    private void Update()
    {
        float distanceToCore = Vector3.Distance(transform.position, core.position);

        if (!isTeleporterOpen)
        {
            if (distanceToCore < 4f)
            {
                animator.Play("DoorOpen");
                hasOpened = true;
            }
            else if (hasOpened)
            {
                animator.Play("DoorClose");
            }
        }

        if (distanceToCore < 2f && !isTeleporting)
        {
            isTeleporterOpen = true;
            TeleportCoreEntrance();
        }
    }

    private void TeleportCoreEntrance()
    {
        isTeleporting = true;

        core.position = Vector3.MoveTowards(core.position, coreHolder.position, 2f * Time.deltaTime);
        core.rotation = Quaternion.RotateTowards(core.rotation, coreHolder.rotation, 180 * Time.deltaTime);

        if (!canGrapple)
        {
            gravityGun.isAttracting = false;
            canGrapple = true;
        }

        Collider coreCollider = core.GetComponent<Collider>();
        if (coreCollider != null)
        {
            coreCollider.enabled = false;
        }
        Rigidbody coreRb = core.GetComponent<Rigidbody>();
        if (coreRb != null)
        {
            coreRb.constraints = RigidbodyConstraints.FreezeAll;
        }

        if (Vector3.Distance(core.position, coreHolder.position) < 0.01f &&
            Quaternion.Angle(core.rotation, coreHolder.rotation) < 1f && !isCoreTeleported)
        {
            isCoreTeleported = true;
            animator.Play("TeleporterClose");
            StartCoroutine(TeleportCore());
        }
        else
        {
            isTeleporting = false;
        }
    }

    private IEnumerator TeleportCore()
    {
        yield return new WaitForSeconds(1f);

        animator.Play("DoorClose");
        yield return new WaitForSeconds(1f);

        core.position = teleportExit.position;
        core.rotation = teleportExit.rotation;

        yield return new WaitForSeconds(0.5f);

        animator.Play("TeleporterOpen");

        Collider coreCollider = core.GetComponent<Collider>();
        if (coreCollider != null)
        {
            coreCollider.enabled = true;
        }
        Rigidbody coreRb = core.GetComponent<Rigidbody>();
        if (coreRb != null)
        {
            coreRb.constraints = RigidbodyConstraints.None;
        }

        isCoreTeleported = false;
        isTeleporterOpen = false;
        isTeleporting = false;
        canGrapple = false;
    }
}
