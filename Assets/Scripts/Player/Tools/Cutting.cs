using EzySlice;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; //Arina UI
using TMPro;
using UnityEngine.UIElements;
using System.Drawing;
using Unity.Mathematics;

public class Cutting : MonoBehaviour
{
    [SerializeField] private LayerMask mask;

    [SerializeField] private GameObject CuttingTrailPrefab;
    private GameObject cuttingTrailRight;
    private GameObject cuttingTrailLeft;
    
    private LayerMask layerMask;
    private Transform cuttingPoint;
    private Vector3 hitPoint;
    private float diffY;
    private CuttableType currentType = CuttableType.None;
    ParticleSystem rightSpark;
    ParticleSystem leftSpark;

    [SerializeField] private float range = 2f;
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
    [SerializeField] private ParticleSystem sparkEffect;

    private bool currentlyCutting = false;

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
        Debug.Log(mask);
    }

    private void Update()
    {
        if (!currentlyCutting)
        {
            switch (currentType)
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
        }

        // Cast a ray forward from the object's position
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        // Check if the ray hit something within specified layers and distance
        if (Physics.Raycast(ray, out hit, range, ~layerMask))
        {
            Transform hitTransform = hit.transform;
            Debug.Log(hitTransform.name);

            if (Input.GetButtonDown("Fire1") && !PauseGame.isPaused)
            {
                if (hitTransform.CompareTag("Cuttable") && AreAnglesClose(transform, hitTransform, angleTolerance))
                {
                    cuttingPoint = hitTransform;
                    hitPoint = hit.point;
                    currentType = CuttableType.Normal;

                    rightSpark = Instantiate(sparkEffect, transform.position, Quaternion.LookRotation(hit.normal));
                    leftSpark = Instantiate(sparkEffect, transform.position, Quaternion.LookRotation(hit.normal));
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
        FaceToCuttingPoint(hitPoint);
    }

    private void AddPoint(LineRenderer renderer, Vector3 point)
    {
        renderer.positionCount += 1;
        renderer.SetPosition(renderer.positionCount-1, point);
    }

    private IEnumerator AnimateLasers(Transform point, float duration)
    {
        currentlyCutting = true;
        MeshFilter meshFilter = point.GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            var mesh = meshFilter.sharedMesh;
            if (mesh != null)
            {
                Vector3 rightmostPoint;
                Vector3 leftmostPoint;

                // Get the rightmost / leftmost points in world space
                // Check from which direction the player will cut and adjust the z bounds accordingly
                if (diffY <= 90f)
                {
                    rightmostPoint = point.TransformPoint(
                        new Vector3(mesh.bounds.max.x, mesh.bounds.center.y, mesh.bounds.min.z));
                    leftmostPoint = point.TransformPoint(
                        new Vector3(mesh.bounds.min.x, mesh.bounds.center.y, mesh.bounds.min.z));
                }
                else
                {
                    rightmostPoint = point.TransformPoint(
                        new Vector3(mesh.bounds.max.x, mesh.bounds.center.y, mesh.bounds.max.z));
                    leftmostPoint = point.TransformPoint(
                        new Vector3(mesh.bounds.min.x, mesh.bounds.center.y, mesh.bounds.max.z));
                }

                // Enable the line renderer and set the start position
                rightmostLaser.enabled = true;
                rightmostLaser.SetPosition(0, shootingPoint.position); // Start point

                leftmostLaser.enabled = true;
                leftmostLaser.SetPosition(0, shootingPoint.position); // Start point

                // Animate the line to the rightmost point
                float elapsedTime = 0f;

                while (elapsedTime < duration)
                {
                    // Calculate the progress of the animation
                    float t = elapsedTime / duration;

                    // Update the end position of the line renderer
                    Vector3 currentRightPoint = Vector3.Lerp(hitPoint, rightmostPoint, t);
                    rightmostLaser.SetPosition(1, currentRightPoint);
                    if (rightSpark != null)
                        rightSpark.transform.position = currentRightPoint; // Move the particle system

                    Vector3 currentLeftPoint = Vector3.Lerp(hitPoint, leftmostPoint, t);
                    leftmostLaser.SetPosition(1, currentLeftPoint);
                    if (leftSpark != null)
                        leftSpark.transform.position = currentLeftPoint; // Move the particle system

                    // Cutting burn marks
                    if (CuttingTrailPrefab != null) 
                    {
                        RaycastHit hit;
                        var l = LayerMask.NameToLayer("Obstacle");
                        if (Physics.Raycast(
                            new Ray(Camera.main.transform.position, currentRightPoint - Camera.main.transform.position),
                            out hit, range, l))
                        { 
                            Debug.Log(LayerMask.LayerToName(hit.transform.gameObject.layer));
                            if (cuttingTrailRight == null) 
                            { 
                                cuttingTrailRight = Instantiate(CuttingTrailPrefab,
                                hit.point, quaternion.identity, hit.transform);
                            }
                            else
                            {
                                AddPoint(cuttingTrailRight.GetComponent<LineRenderer>(),
                                    cuttingTrailRight.transform.InverseTransformPoint(hit.point));
                            }
                        }
                        else if (cuttingTrailRight != null)
                        {
                            cuttingTrailRight = null;
                        }

                        if (Physics.Raycast(
                            new Ray(Camera.main.transform.position, currentLeftPoint - Camera.main.transform.position),
                            out hit, range, l))
                        {
                            if (cuttingTrailLeft == null)
                            {
                                cuttingTrailLeft = Instantiate(CuttingTrailPrefab,
                                     hit.point, quaternion.identity, hit.transform);
                            }
                            else
                            {
                                AddPoint(cuttingTrailLeft.GetComponent<LineRenderer>(),
                                    cuttingTrailLeft.transform.InverseTransformPoint(hit.point));
                            }
                        }
                        else if (cuttingTrailLeft != null)
                        {
                            cuttingTrailLeft = null;
                        }
                    }

                    // Increment the elapsed time
                    elapsedTime += Time.deltaTime;
                    yield return null; // Wait for the next frame
                }

                // Set final positions of the lasers
                rightmostLaser.SetPosition(1, rightmostPoint);
                leftmostLaser.SetPosition(1, leftmostPoint);

                // Destroy the particle systems after animation ends
                if (rightSpark != null)
                    Destroy(rightSpark.gameObject);
                if (leftSpark != null)
                    Destroy(leftSpark.gameObject);
            }
        }

        // Disable the lasers after animation is complete
        rightmostLaser.enabled = false;
        leftmostLaser.enabled = false;
        
        cuttingTrailRight = null;
        cuttingTrailLeft = null;

        // Reset the cutting state
        currentType = CuttableType.None;

        currentlyCutting = false;

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

        diffY = Mathf.Abs(Mathf.DeltaAngle(angleA.y, angleB.y));

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

    private void FaceToCuttingPoint(Vector3 hitPoint)
    {
        if (cuttingPoint == null || playerObject == null) return;

        // Determine the direction vector to the cutting point
        Vector3 directionToTarget = (hitPoint - playerObject.transform.position).normalized;

        // Calculate the rotation to face the target
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);

        // Smoothly rotate the player to face the target
        playerObject.transform.rotation = Quaternion.Lerp(playerObject.transform.rotation, targetRotation, Time.deltaTime * 10f);
    }

    public float Tolerance => angleTolerance;
    public float RayDistance => range;

    private void OnDisable()
    {
        animator.SetBool("IsVertical", true);
        animator.SetBool("IsHorizontal", false);
    }
}
