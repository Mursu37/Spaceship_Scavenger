using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutting : MonoBehaviour
{
    private float tolerance = 6f;
    private float rayDistance = 2f; // Distance of the raycast
    private LayerMask layerMask; // Define a LayerMask to specify layers for Cuttable and Explosive

    private void Start()
    {
        // You can specify layers for Cuttable and Explosive objects
        layerMask = LayerMask.GetMask("Ignore Raycast");
    }

    private void Update()
    {
        // Check if Fire1 is pressed
        if (!Input.GetButtonDown("Fire1"))
            return;

        // Cast a ray forward from the object's position
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        // Check if the ray hit something within specified layers and distance
        if (Physics.Raycast(ray, out hit, rayDistance, ~layerMask))
        {
            Transform hitTransform = hit.transform;
            Debug.Log(hitTransform.name);

            if (Input.GetButtonDown("Fire1"))
            {
                if (hitTransform.CompareTag("Cuttable") && AreAnglesClose(transform, hitTransform, tolerance))
                {
                    Destroy(hitTransform.gameObject);
                }
                else if (hitTransform.CompareTag("Explosive"))
                {
                    Explosives explosives = hitTransform.GetComponent<Explosives>();
                    explosives.Explode();
                }
            }
        }
    }

    // Function to check if the angles of two objects are close
    bool AreAnglesClose(Transform obj1, Transform obj2, float angleTolerance)
    {
        Vector3 angleA = obj1.eulerAngles;
        Vector3 angleB = obj2.eulerAngles;

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
}
