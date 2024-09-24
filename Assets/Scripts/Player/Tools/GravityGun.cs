using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GravityGun : MonoBehaviour
{
    private GameObject target;
    private Rigidbody targetRb;
    private Camera cam;
    private PlayerMovement playerMovement;
    private LineRenderer line;
    private bool isAttracting;

    [SerializeField] private GameObject playerObject;
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private Transform floatPoint;
    [SerializeField] private float range;
    [SerializeField] private float attractAcceleration;
    [SerializeField] private Transform linePosition1;
    private Transform linePosition2;

    // Start is called before the first frame update
    private void Start()
    {
        cam = Camera.main;
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
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
                linePosition2 = target.transform;
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
                Vector3 directionToFloatPoint = (floatPoint.position - target.transform.position).normalized;

                // Apply a force in the direction of the floatPoint with intensity decreasing as it gets closer
                targetRb.AddForce(directionToFloatPoint * attractAcceleration * distance);
            }
            else if (playerRb.mass < targetRb.mass)
            {
                playerRb.drag = 1f;
                playerMovement.maxSpeed = 100f;

                Vector3 directionToFloatPoint = (target.transform.position - floatPoint.position).normalized;

                playerRb.AddForce(directionToFloatPoint * 0.02f * distance, ForceMode.VelocityChange);
            }
        }
        else if (target != null && targetRb == null)
        {
            playerRb.drag = 1f;
            playerMovement.maxSpeed = 100f;

            Vector3 directionToFloatPoint = (target.transform.position - floatPoint.position).normalized;

            float distance = Vector3.Distance(target.transform.position, floatPoint.position);

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
        if (Input.GetButtonDown("Fire1"))
        {
            isAttracting = true;
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            isAttracting = false;
        }

        // Sets a line between the gun and the object
        if (isAttracting && target != null)
        {
            line.enabled = true;
            line.SetPosition(0, linePosition1.position);
            line.SetPosition(1, target.transform.position);
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
