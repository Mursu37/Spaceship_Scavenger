using EzySlice;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; //Arina UI
using TMPro;

public class Cutting : MonoBehaviour
{
    
    private float rayDistance = 2f; // Distance of the raycast
    private LayerMask layerMask; // Define a LayerMask to specify layers for Cuttable and Explosive
    private Transform cuttingPoint;
    private Vector3 hitPoint;
    private CuttableType currentType = CuttableType.None;

    [SerializeField] private float angleTolerance = 6f;
    [SerializeField] private LineRenderer rightmostLaser;
    [SerializeField] private LineRenderer leftmostLaser;
    [SerializeField] private Animator animator;
    public bool isVerticalCut = false;
    [SerializeField] private GameObject slicerObject;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private GameObject horizontalCrosshair;
    [SerializeField] private GameObject verticalCrosshair;
    [SerializeField] private GameObject playerObject;

    private enum CuttableType
    {
        None,
        Normal,
        Explosive
    }

    private void Start()
    {
        layerMask = LayerMask.GetMask("Player");
        rightmostLaser.enabled = false;
        leftmostLaser.enabled = false;
    }

    private void Update()
    {
        switch(currentType)
        {
            case CuttableType.Normal:
                if (cuttingPoint != null)
                {
                    StartCoroutine(AnimateLasers(cuttingPoint, 1f));
                }
                break;
            case CuttableType.Explosive:
                if (cuttingPoint != null)
                {
                    StartCoroutine(ExplodeObject(cuttingPoint, hitPoint));
                }
                break;
        }

        // Cast a ray forward from the object's position
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        // Check if the ray hit something within specified layers and distance
        if (Physics.Raycast(ray, out hit, rayDistance, ~layerMask))
        {
            Transform hitTransform = hit.transform;
            Debug.Log(hitTransform.name);

            if (Input.GetButtonDown("Fire1") && !PauseGame.isPaused)
            {
                if (hitTransform.CompareTag("Cuttable") && AreAnglesClose(transform, hitTransform, angleTolerance))
                {
                    cuttingPoint = hitTransform;
                    currentType = CuttableType.Normal;
                }
                else if (hitTransform.CompareTag("Explosive"))
                {
                    cuttingPoint = hitTransform;
                    hitPoint = hit.point;
                    currentType = CuttableType.Explosive;
                }
            }
        }

        if (Input.GetButtonDown("Fire2") && !PauseGame.isPaused)
        {
            isVerticalCut = !isVerticalCut;

            if (isVerticalCut)
            {
                slicerObject.transform.localRotation = Quaternion.Euler(0, 0, 90);
            }
            else
            {
                slicerObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }

        if (isVerticalCut)
        {
            animator.SetBool("IsVertical", true);
            animator.SetBool("IsHorizontal", false);
            horizontalCrosshair.SetActive(false);
            verticalCrosshair.SetActive(true);
        }
        else
        {
            animator.SetBool("IsVertical", false);
            animator.SetBool("IsHorizontal", true);
            horizontalCrosshair.SetActive(true);
            verticalCrosshair.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        FaceToCuttingPoint();
    }

    private IEnumerator AnimateLasers(Transform point, float duration)
    {
        MeshFilter meshFilter = point.GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            var mesh = meshFilter.sharedMesh;
            if (mesh != null)
            {
                // Get the center and rightmost / leftmost points in world space
                Vector3 center = point.TransformPoint(mesh.bounds.center);
                Vector3 rightmostPoint = point.TransformPoint(
                    new Vector3(mesh.bounds.max.x, mesh.bounds.center.y, mesh.bounds.center.z));
                Vector3 leftmostPoint = point.TransformPoint(
                    new Vector3(mesh.bounds.min.x, mesh.bounds.center.y, mesh.bounds.center.z));

                // Enable the line renderer and set the start position
                rightmostLaser.enabled = true;
                rightmostLaser.SetPosition(0, shootingPoint.position); // Start point
                rightmostLaser.SetPosition(1, center); // Start at the center

                leftmostLaser.enabled = true;
                leftmostLaser.SetPosition(0, shootingPoint.position); // Start point
                leftmostLaser.SetPosition(1, center); // Start at the center

                // Animate the line to the rightmost point
                float elapsedTime = 0f;

                while (elapsedTime < duration)
                {
                    // Calculate the progress of the animation
                    float t = elapsedTime / duration;

                    // Update the end position of the line renderer
                    Vector3 currentRightPoint = Vector3.Lerp(center, rightmostPoint, t);
                    rightmostLaser.SetPosition(1, currentRightPoint);

                    Vector3 currentLeftPoint = Vector3.Lerp(center, leftmostPoint, t);
                    leftmostLaser.SetPosition(1, currentLeftPoint);

                    // Increment the elapsed time
                    elapsedTime += Time.deltaTime;
                    yield return null; // Wait for the next frame
                }

                rightmostLaser.SetPosition(1, rightmostPoint);
                leftmostLaser.SetPosition(1, leftmostPoint);
            }
        }

        rightmostLaser.enabled = false;
        leftmostLaser.enabled = false;
        currentType = CuttableType.None;

        if (cuttingPoint != null)
        {
            if (point != null)
            {
                Destroy(point.gameObject);
            }
            cuttingPoint = null;
        }
    }

    private IEnumerator ExplodeObject(Transform hitTransform, Vector3 hitPoint)
    {
        // Animate the laser
        rightmostLaser.enabled = true;
        rightmostLaser.SetPosition(0, shootingPoint.position);
        rightmostLaser.SetPosition(1, hitPoint);

        yield return new WaitForSeconds(0.5f);

        rightmostLaser.enabled = false;

        if (hitTransform != null)
        {
            // Ensure the object has the Explosives component
            Explosives explosives = hitTransform.GetComponent<Explosives>();
            if (explosives != null)
            {
                explosives.Explode(); // Execute the explosion logic
            }

            currentType = CuttableType.None;
        }
    }

    // Function to check if the angles of two objects are close
    public bool AreAnglesClose(Transform obj1, Transform obj2, float angleTolerance)
    {
        Vector3 angleA = obj1.eulerAngles;
        Vector3 angleB = obj2.eulerAngles;

        if (isVerticalCut)
        {
            angleA.z += 90f;
        }

        float diffZ = Mathf.Abs(Mathf.DeltaAngle(angleA.z, angleB.z));
        float diffZRotated = Mathf.Abs(Mathf.DeltaAngle(angleA.z + 180f, angleB.z));
        float diffZNegative = Mathf.Abs(Mathf.DeltaAngle(angleA.z * -1, angleB.z));
        float diffZRotatedNegative = Mathf.Abs(Mathf.DeltaAngle(angleA.z * -1 + 180f, angleB.z));

        float diffY = Mathf.Abs(Mathf.DeltaAngle(angleA.y, angleB.y));

        bool isZClose = diffZ <= angleTolerance || diffZRotated <= angleTolerance;
        bool isZCloseNegative = diffZNegative <= angleTolerance || diffZRotatedNegative <= angleTolerance;

        if (diffY <= 90f)
        {
            if (isZClose)
            {
                return true;
            }
        }
        else
        {
            if (isZCloseNegative)
            {
                return true;
            }
        }

        return false;
    }

    private void FaceToCuttingPoint()
    {
        if (cuttingPoint == null || playerObject == null) return;

        // Determine the direction vector to the cutting point
        Vector3 directionToTarget = (cuttingPoint.position - playerObject.transform.position).normalized;

        // Calculate the rotation to face the target
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);

        // Smoothly rotate the player to face the target
        playerObject.transform.rotation = Quaternion.Lerp(playerObject.transform.rotation, targetRotation, Time.deltaTime * 10f);
    }

    public float Tolerance => angleTolerance;
    public float RayDistance => rayDistance;

    private void OnDisable()
    {
        animator.SetBool("IsVertical", true);
        animator.SetBool("IsHorizontal", false);
    }
}
