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
    private bool isAttracting;

    [SerializeField] private GameObject playerObject;
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private Transform floatPoint;
    [SerializeField] private float range;
    [SerializeField] private float attractAcceleration;
    [SerializeField] private Transform shootingPoint;

    // Start is called before the first frame update
    private void Start()
    {
        cam = Camera.main;
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.enabled = false;
        playerMovement = playerObject.GetComponent<PlayerMovement>();
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
            }
        }

        if (target != null && targetRb != null)
        {
            // Calculate the required force based on the distance to floatPoint
            float distance = Vector3.Distance(target.transform.position, floatPoint.position);

            if (playerRb.mass > targetRb.mass)
            {
                targetRb.drag = 0.5f;

                // Calculate the direction to the floating point
                Vector3 direction = (floatPoint.position - target.transform.position).normalized;

                // Apply a force in the direction of the floatPoint with intensity decreasing as it gets closer
                targetRb.AddForce(direction * attractAcceleration * distance);
            }
            else if (playerRb.mass < targetRb.mass)
            {
                playerRb.drag = 1f;
                playerMovement.maxSpeed = 100f;

                Vector3 direction = (target.transform.position - floatPoint.position).normalized;

                playerRb.AddForce(direction * 0.02f * distance, ForceMode.VelocityChange);

                // Makes player and the object lose momentum when they're near each other
                if (distance <= 3f)
                {
                    Release();
                }
            }
            else if (playerRb.mass == targetRb.mass)
            {
                playerRb.drag = 0.5f;
                targetRb.drag = 0.5f;
                playerMovement.maxSpeed = 100f;

                Vector3 directionToFloatPoint = (floatPoint.position - target.transform.position).normalized;
                Vector3 directionToFloatPlayer = (target.transform.position - floatPoint.position).normalized;

                targetRb.AddForce(directionToFloatPoint * 0.01f * distance, ForceMode.VelocityChange);
                playerRb.AddForce(directionToFloatPlayer * 0.01f * distance, ForceMode.VelocityChange);

                if (distance <= 3f)
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

            Vector3 directionToFloatPoint = (hitPosition - floatPoint.position).normalized;

            float distance = Vector3.Distance(hitPosition, floatPoint.position);

            playerRb.AddForce(directionToFloatPoint * 0.02f * distance, ForceMode.VelocityChange);
        }
    }

    // Releases the object
    private void Release()
    {
        if (target != null && targetRb != null)
        {
            targetRb.drag = 0f;
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
        floatPointPos.z += 2f * scrollWheelInput;
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
            Attract();
        }
        else
        {
            Release();
        }
    }
}
