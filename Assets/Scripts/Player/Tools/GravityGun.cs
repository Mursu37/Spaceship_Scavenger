using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GravityGun : MonoBehaviour
{
    // Inputs
    private float scrollWheelInput;

    private GameObject target;
    private Rigidbody targetRb;
    private Camera cam;
    private PlayerMovement playerMovement;
    private LineRenderer line;
    private Vector3 hitPosition;
    private WeaponSwitch weaponSwitch;
    private bool isAttracting;
    private MeltdownPhase meltdownPhase;
    private EnergyCore energyCore;

    [SerializeField] private GameObject playerObject;
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private Transform floatPoint;
    [SerializeField] private float range;
    [SerializeField] private float attractAcceleration;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private GameObject weaponSwitchObject;
    [SerializeField] private GameObject gameState;

    // Start is called before the first frame update
    private void Start()
    {
        cam = Camera.main;
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.enabled = false;
        playerMovement = playerObject.GetComponent<PlayerMovement>();
        weaponSwitch = weaponSwitchObject.GetComponent<WeaponSwitch>();

        if (gameState != null)
        {
            meltdownPhase = gameState.GetComponent<MeltdownPhase>();
        }
    }

    // Attracts things towards the player
    private void Attract()
    {
        RaycastHit hit;

        // If we don't have a target, try to find one
        if (target == null)
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
            {
                target = hit.transform.gameObject;
                targetRb = target.GetComponent<Rigidbody>();
                hitPosition = hit.point;
                floatPoint.position = hit.point;

                if (targetRb != null)
                {
                    targetRb.constraints = RigidbodyConstraints.FreezeRotation;
                }
            }
        }

        if (target != null && targetRb != null)
        {
            if (playerRb.mass > targetRb.mass)
            {
                if (targetRb.tag == "Core" && !meltdownPhase.enabled)
                {
                    meltdownPhase.enabled = true;
                    targetRb.constraints = RigidbodyConstraints.None;
                    energyCore = targetRb.GetComponent<EnergyCore>();
                    StartCoroutine(energyCore.HeathIncrease());
                }

                targetRb.drag = 0.5f;

                // Calculate the required force based on the distance
                float distance = Vector3.Distance(target.transform.position, floatPoint.position);
                float distanceToPlayer = Vector3.Distance(target.transform.position, playerRb.position);

                // Calculate the direction to the floating point
                Vector3 direction = (floatPoint.position - target.transform.position).normalized;

                // Apply a force in the direction of the floatPoint with intensity decreasing as it gets closer
                targetRb.AddForce(direction * attractAcceleration * distance);

                // Apply the player's rotation to the target
                target.transform.rotation = Quaternion.Slerp(target.transform.rotation, playerObject.transform.rotation, 0.1f);

            }
            else if (playerRb.mass < targetRb.mass)
            {
                playerRb.drag = 1f;
                playerMovement.maxSpeed = 100f;

                float distance = Vector3.Distance(target.transform.position, playerRb.position);

                Vector3 direction = (target.transform.position - playerRb.position).normalized;

                playerRb.AddForce(direction * 0.02f * distance, ForceMode.VelocityChange);

                // Makes player and the object lose momentum when they're near each other
                if (distance <= 4f)
                {
                    Release();
                }
            }
            else if (playerRb.mass == targetRb.mass)
            {
                playerRb.drag = 0.5f;
                targetRb.drag = 0.5f;
                playerMovement.maxSpeed = 100f;

                float distance = Vector3.Distance(target.transform.position, playerRb.position);

                Vector3 directionToFloatPoint = (playerRb.position - target.transform.position).normalized;
                Vector3 directionToFloatPlayer = (target.transform.position - playerRb.position).normalized;

                targetRb.AddForce(directionToFloatPoint * 0.01f * distance, ForceMode.VelocityChange);
                playerRb.AddForce(directionToFloatPlayer * 0.01f * distance, ForceMode.VelocityChange);

                if (distance <= 6f)
                {
                    targetRb.velocity = Vector3.Lerp(targetRb.velocity, Vector3.zero, 0.1f);
                    playerRb.velocity = Vector3.Lerp(playerRb.velocity, Vector3.zero, 0.1f);
                }
            }
        }
        else if (target != null && targetRb == null)
        {
            playerRb.drag = 1f;
            playerMovement.maxSpeed = 100f;

            Vector3 directionToFloatPoint = (hitPosition - playerRb.position).normalized;

            float distance = Vector3.Distance(hitPosition, playerRb.position);

            playerRb.AddForce(directionToFloatPoint * 0.02f * distance, ForceMode.VelocityChange);
        }
    }

    // Releases the object
    private void Release()
    {
        if (target != null && targetRb != null)
        {
            targetRb.drag = 0f;
            targetRb.constraints = RigidbodyConstraints.None;
        }

        target = null;
        targetRb = null;
        playerRb.drag = 0f;
        playerMovement.maxSpeed = 2.5f;
    }

    // Update is called once per frame
    private void Update()
    {
        scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");

        Vector3 floatPointPos = floatPoint.localPosition;

        if (scrollWheelInput > 0f)
        {
            floatPointPos.z += 4f * scrollWheelInput;
        }
        else if (scrollWheelInput < 0f && Vector3.Distance(floatPoint.position, playerRb.position) > 2f)
        {
            floatPointPos.z += 4f * scrollWheelInput;
        }

        floatPoint.localPosition = floatPointPos;

        if (Input.GetButtonDown("Fire1"))
        {
            isAttracting = true;
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            isAttracting = false;
        }

        // Sets a line between the gun and the object
        if (isAttracting && target != null && targetRb != null)
        {
            line.enabled = true;
            line.SetPosition(0, shootingPoint.position);
            line.SetPosition(1, target.transform.position);
        }
        else if (isAttracting && target != null && targetRb == null)
        {
            line.enabled = true;
            line.SetPosition(0, shootingPoint.position);
            line.SetPosition(1, hitPosition);
        }
        else
        {
            line.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (isAttracting)
        {
            weaponSwitch.enabled = false;
            Attract();
        }
        else
        {
            weaponSwitch.enabled = true;
            Release();
        }
    }
}
