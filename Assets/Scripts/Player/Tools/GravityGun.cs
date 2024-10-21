using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private LineRenderer lineRenderer;
    private Vector3 hitPosition;
    private ModeSwitch modeSwitch;
    private bool isAttracting;
    private Quaternion targetInitialRotation;
    private Quaternion playerInitialRotation;
    private bool hasInitialRotations;

    [SerializeField] private GameObject playerObject;
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private Transform floatPoint;
    [SerializeField] private float range;
    [SerializeField] private float attractAcceleration;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private Transform p1;

    public bool isGrabbling;

    // Start is called before the first frame update
    private void Start()
    {
        cam = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        playerMovement = playerObject.GetComponent<PlayerMovement>();
        modeSwitch = GetComponent<ModeSwitch>();
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
                    targetInitialRotation = target.transform.rotation;
                    playerInitialRotation = playerObject.transform.rotation;
                    hasInitialRotations = true;
                }
            }
        }

        if (target != null && targetRb != null && !targetRb.isKinematic)
        {
            if (playerRb.mass > targetRb.mass)
            {
                targetRb.drag = 0.5f;

                // Calculate the required force based on the distance
                float distance = Vector3.Distance(target.transform.position, floatPoint.position);
                float distanceToPlayer = Vector3.Distance(target.transform.position, playerRb.position);

                // Calculate the direction to the floating point
                Vector3 direction = (floatPoint.position - target.transform.position).normalized;

                // Apply a force in the direction of the floatPoint with intensity decreasing as it gets closer
                targetRb.AddForce(direction * attractAcceleration * distance);

                // Applies the player's rotation to the target object
                if (hasInitialRotations)
                {
                    // Calculate the difference in player rotation from the initial state
                    Quaternion playerRotationDifference = playerObject.transform.rotation * Quaternion.Inverse(playerInitialRotation);
                    // Apply this difference to the target's initial rotation
                    target.transform.rotation = playerRotationDifference * targetInitialRotation;
                }
            }
            else if (playerRb.mass < targetRb.mass)
            {
                playerRb.drag = 1f;
                isGrabbling = true;

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
                isGrabbling = true;

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
        else if (target != null && (targetRb == null || targetRb.isKinematic))
        {
            playerRb.drag = 1f;
            isGrabbling = true;

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
        isGrabbling = false;
    }

    // Draws curved line (Bézier Curve) between points
    // Source: https://www.codinblack.com/how-to-draw-lines-circles-or-anything-else-using-linerenderer/
    private void DrawQuadraticBezierCurve(Vector3 point0, Vector3 point1, Vector3 point2)
    {
        lineRenderer.positionCount = 200;
        float t = 0f;
        Vector3 B = new Vector3(0, 0, 0);
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            B = (1 - t) * (1 - t) * point0 + 2 * (1 - t) * t * point1 + t * t * point2;
            lineRenderer.SetPosition(i, B);
            t += (1 / (float)lineRenderer.positionCount);
        }
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

        // Draws a line between the gun and the object
        if (isAttracting && target != null && targetRb != null)
        {
            lineRenderer.enabled = true;
            p1.position = shootingPoint.position + cam.transform.forward * 4f;
            DrawQuadraticBezierCurve(shootingPoint.position, p1.position, target.transform.position);
        }
        else if (isAttracting && target != null && targetRb == null)
        {
            lineRenderer.enabled = true;
            p1.position = shootingPoint.position + cam.transform.forward;
            DrawQuadraticBezierCurve(shootingPoint.position, p1.position, hitPosition);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (isAttracting)
        {
            modeSwitch.enabled = false;
            Attract();
        }
        else
        {
            modeSwitch.enabled = true;
            Release();
        }
    }
}
