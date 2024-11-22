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
    private LineRenderer lineRenderer;
    private Vector3 hitPosition;
    private ModeSwitch modeSwitch;
    public bool isAttracting;
    private Quaternion targetInitialRotation;
    private Quaternion playerInitialRotation;
    private Vector3 localHitOffset;
   

    public bool IsGrabbingValidObject()
    {
        return target != null && targetRb != null; // Check if a valid target is grabbed
    }


    [SerializeField] private GameObject playerObject;
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private Transform floatPoint;
    [SerializeField] private float range;
    [SerializeField] private float attractAcceleration;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private Transform p1;
    [SerializeField] private LayerMask ignoreLayerMask;

    [SerializeField] private Animator animator;

 
    [HideInInspector] public bool isGrabbling;

    // These variables can be used for the UI
    public float distanceToPlayer;
    public float objectMass;
    public float strength;


    // Start is called before the first frame update
    private void Start()
    {
        cam = Camera.main;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        modeSwitch = GetComponent<ModeSwitch>();
    }

    // Attracts things towards to defined point
    private void Attract()
    {
        RaycastHit hit;

        // If we don't have a target, try to find one
        if (target == null)
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range, ~ignoreLayerMask))
            {
                target = hit.transform.gameObject;
                targetRb = target.GetComponent<Rigidbody>();
                hitPosition = hit.point;
                floatPoint.position = hit.point;

                if (targetRb != null && !target.CompareTag("Unmovable"))
                {
                    targetRb.constraints = RigidbodyConstraints.FreezeRotation;
                    targetInitialRotation = target.transform.rotation;
                    playerInitialRotation = playerObject.transform.rotation;

                    localHitOffset = target.transform.InverseTransformPoint(hit.point);
                }
            }
        }

        if (target != null && targetRb != null && !target.CompareTag("Unmovable"))
        {
            objectMass = Mathf.Round(targetRb.mass * 10f) / 10f * 10f;

            if (playerRb.mass > targetRb.mass)
            {
                targetRb.drag = 2f;

                Vector3 grapplePoint = target.transform.TransformPoint(localHitOffset);

                // Calculate the required force based on the distance
                float distance = Vector3.Distance(grapplePoint, floatPoint.position);
                distanceToPlayer = Mathf.Round(Vector3.Distance(grapplePoint, playerRb.position) * 10f) / 10f;
                strength = Mathf.Round(attractAcceleration * distance * 10f) / 10f;

                // Calculate the direction to the floating point
                Vector3 direction = (floatPoint.position - grapplePoint).normalized;

                // Apply a force in the direction of the floatPoint with intensity decreasing as it gets closer
                targetRb.AddForce(direction * attractAcceleration * distance);

                FixedJoint fixedJoint = target.GetComponent<FixedJoint>();
                if (fixedJoint == null)
                {
                    // Calculate the difference in player rotation from the initial state
                    Quaternion playerRotationDifference = playerObject.transform.rotation * Quaternion.Inverse(playerInitialRotation);
                    // Apply this difference to the target's initial rotation
                    target.transform.rotation = playerRotationDifference * targetInitialRotation;
                }
                else
                {
                    fixedJoint.breakForce = 3f;
                }
            }
            else if (playerRb.mass < targetRb.mass)
            {
                playerRb.drag = 1f;
                isGrabbling = true;

                Vector3 grapplePoint = target.transform.TransformPoint(localHitOffset);

                float distance = Vector3.Distance(grapplePoint, playerRb.position);
                distanceToPlayer = Mathf.Round(distance * 10f) / 10f;
                strength = Mathf.Round(attractAcceleration * distance * 10f) / 10f;

                Vector3 direction = (grapplePoint - playerRb.position).normalized;

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

                Vector3 grapplePoint = target.transform.TransformPoint(localHitOffset);

                float distance = Vector3.Distance(grapplePoint, playerRb.position);
                distanceToPlayer = Mathf.Round(distance * 10f) / 10f;
                strength = Mathf.Round(attractAcceleration * distance * 10f) / 10f;

                Vector3 directionToFloatPoint = (playerRb.position - grapplePoint).normalized;
                Vector3 directionToFloatPlayer = (grapplePoint - playerRb.position).normalized;

                targetRb.AddForce(directionToFloatPoint * 0.01f * distance, ForceMode.VelocityChange);
                playerRb.AddForce(directionToFloatPlayer * 0.01f * distance, ForceMode.VelocityChange);

                if (distance <= 6f)
                {
                    targetRb.velocity = Vector3.Lerp(targetRb.velocity, Vector3.zero, 0.1f);
                    playerRb.velocity = Vector3.Lerp(playerRb.velocity, Vector3.zero, 0.1f);
                }
            }
        }
        else if (target != null && (targetRb == null || target.CompareTag("Unmovable")))
        {
            playerRb.drag = 1f;
            isGrabbling = true;

            Vector3 directionToFloatPoint = (hitPosition - playerRb.position).normalized;

            float distance = Vector3.Distance(hitPosition, playerRb.position);
            distanceToPlayer = Mathf.Round(distance * 10f) / 10f;
            strength = Mathf.Round(attractAcceleration * distance * 10f) / 10f;

            playerRb.AddForce(directionToFloatPoint * 0.02f * distance, ForceMode.VelocityChange);
        }
    }

    // Releases the object
    private void Release()
    {
        
        if (target == null)
        {
            targetRb = null;
            playerRb.drag = playerObject.GetComponent<PlayerMovement>().currentDrag;
            isGrabbling = false;
            distanceToPlayer = 0;
            strength = 0f;
            objectMass = 0f;
            return;
        }

        if (targetRb != null && !target.CompareTag("Unmovable"))
        {
            targetRb.drag = 0f;
            targetRb.constraints = RigidbodyConstraints.None;
        }

        FixedJoint joint = target.GetComponent<FixedJoint>();
        if (joint != null)
        {
            joint.breakForce = Mathf.Infinity;
        }

        target = null;
        targetRb = null;
        playerRb.drag = playerObject.GetComponent<PlayerMovement>().currentDrag;
        isGrabbling = false;
        distanceToPlayer = 0;
        strength = 0f;
        objectMass = 0f;
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

        if (scrollWheelInput > 0f && !PauseGame.isPaused)
        {
            floatPointPos.z += 4f * scrollWheelInput;
        }
        else if (scrollWheelInput < 0f && Vector3.Distance(floatPoint.position, playerRb.position) > 2f && !PauseGame.isPaused)
        {
            floatPointPos.z += 4f * scrollWheelInput;
        }

        floatPoint.localPosition = floatPointPos;

        if (Input.GetButtonDown("Fire1") && !PauseGame.isPaused)
        {
            isAttracting = true;
        }
        else if (Input.GetButtonUp("Fire1") && !PauseGame.isPaused)
        {
            isAttracting = false;
        }

        // Draws a line between the gun and the object
        if (isAttracting && target != null && targetRb != null)
        {
            lineRenderer.enabled = true;
            p1.position = shootingPoint.position + cam.transform.forward * 4f;
            Vector3 grapplePointWorldPosition = target.transform.TransformPoint(localHitOffset);
            DrawQuadraticBezierCurve(shootingPoint.position, p1.position, grapplePointWorldPosition);
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
            animator.SetBool("IsGrabbling", true);
            Attract();
        }
        else
        {
            modeSwitch.enabled = true;
            animator.SetBool("IsGrabbling", false);
            Release();
        }
    }
}
