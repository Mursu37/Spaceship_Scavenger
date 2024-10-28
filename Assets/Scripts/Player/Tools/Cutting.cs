using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutting : MonoBehaviour
{
    private Collider cuttableObject;
    private float tolerance = 6f;

    private void Update()
    {
        if (cuttableObject == null || !Input.GetButtonDown("Fire1"))
            return; // Early exit for invalid cases

        if (cuttableObject.CompareTag("Cuttable") && AreAnglesClose(transform, cuttableObject.transform, tolerance))
        {
            Destroy(cuttableObject.gameObject);
        }

        if (cuttableObject.CompareTag("Explosive"))
        {
            Explosives explosives = cuttableObject.GetComponent<Explosives>();
            explosives.Explode();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
        if (other.CompareTag("Cuttable") || other.CompareTag("Explosive"))
        {
            cuttableObject = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (cuttableObject == other)
        {
            cuttableObject = null;
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
