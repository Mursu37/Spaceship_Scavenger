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
using Random = UnityEngine.Random;

public class Cutting : MonoBehaviour
{
    [SerializeField] private GameObject CuttingTrailPrefab;
    [SerializeField] private GameObject trailParticlePrefab;
    private bool isCutting = false;

    private LayerMask layerMask;
    private Transform cuttingPoint;
    private Vector3 hitPoint;
    private float diffY;
    private CuttableType currentType = CuttableType.None;
    private Quaternion initialRotation;
    ParticleSystem rightSpark;
    ParticleSystem leftSpark;
    ParticleSystem rightBeamEnd;
    ParticleSystem leftBeamEnd;

    [SerializeField] private float range = 2f;
    [SerializeField] private float angleTolerance = 6f;

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private GameObject horizontalCrosshair;
    [SerializeField] private GameObject verticalCrosshair;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private LineRenderer rightmostLaser;
    [SerializeField] private LineRenderer leftmostLaser;
    [SerializeField] private ParticleSystem sparkEffect;
    [SerializeField] private ParticleSystem beamSource;
    [SerializeField] private ParticleSystem beamEnd;

    [HideInInspector] public bool isVerticalCut = false;
    private bool hasPlayedBlockedSound = false;
    private bool isSoundCoroutineRunning = false;

    //Arina UI 
    public bool IsObjectDetected()
    {
        return cuttingPoint != null;
    }

    public bool IsCutAligned()
    {
        // Check alignment 
        return cuttingPoint != null && AreAnglesClose(transform, cuttingPoint, angleTolerance);
    }

    private enum CuttableType
    {
        None,
        Normal,
        Explosive
    }

    private void Start()
    {
        layerMask = LayerMask.GetMask("Player") | LayerMask.GetMask("CutThrough") | LayerMask.GetMask("FirstPersonView");
        rightmostLaser.enabled = false;
        leftmostLaser.enabled = false;
    }

    private void Update()
    {
        switch(currentType)
        {
            case CuttableType.Normal:
                if (cuttingPoint != null && !isCutting)
                {
                    beamSource.Play();
                    StartCoroutine(AnimateLasers(cuttingPoint, 1f));
                }
                break;
            case CuttableType.Explosive:
                if (cuttingPoint != null)
                {
                    beamSource.Play();
                    StartCoroutine(ExplodeObject(cuttingPoint, hitPoint));
                }
                break;
        }

        //Modified order to accomodate sound playback
        if (Input.GetButtonDown("Fire1") && !PauseGame.isPaused)
        {
            // Cast a ray forward from the camera's position
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hit;

            initialRotation = playerObject.transform.rotation;

            // Check if the ray hit something within specified layers and distance
            if (Physics.Raycast(ray, out hit, range, ~layerMask))
            {
                Transform hitTransform = hit.transform;
                Debug.Log(hitTransform.name);

                if (hitTransform.CompareTag("Cuttable") && AreAnglesClose(transform, hitTransform, angleTolerance))
                {
                    cuttingPoint = hitTransform;
                    hitPoint = hit.point;
                    currentType = CuttableType.Normal;

                    if (rightSpark == null && leftSpark == null && rightBeamEnd == null && leftBeamEnd == null)
                    {
                        rightSpark = Instantiate(sparkEffect, transform.position, Quaternion.LookRotation(hit.normal));
                        leftSpark = Instantiate(sparkEffect, transform.position, Quaternion.LookRotation(hit.normal));
                        rightBeamEnd = Instantiate(beamEnd, transform.position, Quaternion.identity);
                        leftBeamEnd = Instantiate(beamEnd, transform.position, Quaternion.identity);
                    }

                    // Reset the blocked sound flag since we have a valid action now
                    hasPlayedBlockedSound = false;
                }
                else if (hitTransform.CompareTag("Explosive"))
                {
                    cuttingPoint = hitTransform;
                    hitPoint = hit.point;
                    currentType = CuttableType.Explosive;

                    hasPlayedBlockedSound = false;
                }
                // If hit object is neither "Cuttable" nor "Explosive"
                else
                {
                    PlayActionBlockedSound();
                }
            }
            else
            {
                // If the raycast does not hit anything
                PlayActionBlockedSound();
            }
        }

        if (Input.GetButtonDown("Fire2") && !PauseGame.isPaused)
        {
            isVerticalCut = !isVerticalCut;
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

    private void PlayActionBlockedSound()
    {
        if (!hasPlayedBlockedSound)
        {
            AudioManager.PlayAudio("MultitoolActionBlocked", 1, 1, false);
            hasPlayedBlockedSound = true;
        }
            if (!isSoundCoroutineRunning)
            {
                StartCoroutine(ResetBlockedSoundFlag());
            }
    }
    private IEnumerator ResetBlockedSoundFlag()
    {
        isSoundCoroutineRunning = true;
        float clipduration = 1f;
        yield return new WaitForSeconds(clipduration);
        hasPlayedBlockedSound = false;
        isSoundCoroutineRunning = false;
    }

    private void FixedUpdate()
    {
        if (currentType != CuttableType.None)
        {
            KeepAngle();
        }
    }

    private IEnumerator AnimateLasers(Transform point, float duration)
    {
        isCutting = true;
        GameObject cuttingTrailRight = null;
        GameObject cuttingTrailLeft = null;
        Collider cuttingColliderRight = null;
        Collider cuttingColliderLeft = null;
        
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

                    Vector3 currentLeftPoint = Vector3.Lerp(hitPoint, leftmostPoint, t);
                    leftmostLaser.SetPosition(1, currentLeftPoint);

                    // Cutting burn marks
                    if (CuttingTrailPrefab != null) 
                    {
                        RaycastHit hit;
                        var l = LayerMask.GetMask("CutThrough");
                        if (Physics.Raycast(
                            new Ray(Camera.main.transform.position, currentRightPoint - Camera.main.transform.position),
                            out hit, range, l))
                        {
                            if (rightSpark != null)
                                rightSpark.transform.position = hit.point; // Move the particle system
                            if (rightBeamEnd != null)
                                rightBeamEnd.transform.position = hit.point;

                            Debug.Log(LayerMask.LayerToName(hit.transform.gameObject.layer));
                            if (cuttingTrailRight == null) 
                            { 
                                cuttingTrailRight = Instantiate(CuttingTrailPrefab,
                                hit.point, quaternion.identity, hit.transform);
                                cuttingColliderRight = hit.collider;
                                Destroy(cuttingTrailRight, 15);
                            }
                            else if (cuttingColliderRight == hit.collider)
                            {
                                AddPoint(cuttingTrailRight.GetComponent<LineRenderer>(),
                                    cuttingTrailRight.transform.InverseTransformPoint(hit.point));
                            }
                            else
                            {
                                cuttingTrailRight = Instantiate(CuttingTrailPrefab,
                                    hit.point, quaternion.identity, hit.transform);
                                cuttingColliderRight = hit.collider;
                                Destroy(cuttingTrailRight, 15);
                            }
                            if (Random.Range(0, 5) > 3f)
                            {
                                Instantiate(trailParticlePrefab, hit.point, quaternion.identity, hit.transform);
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
                            if (leftSpark != null)
                                leftSpark.transform.position = hit.point;  // Move the particle system
                            if (leftBeamEnd != null)
                                leftBeamEnd.transform.position = hit.point;

                            if (cuttingTrailLeft == null)
                            {
                                cuttingTrailLeft = Instantiate(CuttingTrailPrefab,
                                     hit.point, quaternion.identity, hit.transform);
                                cuttingColliderLeft = hit.collider;
                                Destroy(cuttingTrailLeft, 15);
                            }
                            else if (cuttingColliderLeft == hit.collider)
                            {
                                AddPoint(cuttingTrailLeft.GetComponent<LineRenderer>(),
                                    cuttingTrailLeft.transform.InverseTransformPoint(hit.point));
                            }
                            else
                            {
                                cuttingTrailLeft = Instantiate(CuttingTrailPrefab,
                                    hit.point, quaternion.identity, hit.transform);
                                cuttingColliderLeft = hit.collider;
                                Destroy(cuttingTrailLeft, 15);
                            }

                            if (Random.Range(0, 5) > 3f)
                            {
                                Instantiate(trailParticlePrefab, hit.point, quaternion.identity, hit.transform);
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
                if (rightBeamEnd != null)
                    Destroy(rightBeamEnd.gameObject);
                if (leftSpark != null)
                    Destroy(leftSpark.gameObject);
                if (leftBeamEnd != null)
                    Destroy(leftBeamEnd.gameObject);
            }

            isCutting = false;
        }

        // Disable the lasers after animation is complete
        rightmostLaser.enabled = false;
        leftmostLaser.enabled = false;

        // Reset the cutting state
        currentType = CuttableType.None;

        beamSource.Stop();

        if (cuttingPoint != null)
        {
            if (point != null)
            {
                Destroy(point.gameObject);
            }
            cuttingPoint = null;
        }
    }
    
    private void AddPoint(LineRenderer renderer, Vector3 point)
    {
        renderer.positionCount += 1;
        renderer.SetPosition(renderer.positionCount-1, point);
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

            beamSource.Stop();
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

    private void KeepAngle()
    {
        // Smoothly rotate the player to face the target
        playerObject.transform.rotation = Quaternion.Lerp(playerObject.transform.rotation, initialRotation, Time.deltaTime * 10f);
    }

    public float Tolerance => angleTolerance;
    public float RayDistance => range;

    private void OnDisable()
    {
        animator.SetBool("IsVertical", true);
        animator.SetBool("IsHorizontal", false);
    }
}