using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutting : MonoBehaviour
{
    private Collider cuttableObject;
    private float tolerance = 6f;

    private void Update()
    {
        if (cuttableObject == null || !Input.GetButtonDown("Fire1") || !AreAnglesClose(transform, cuttableObject.transform, tolerance))
            return; // Early exit for invalid cases

        FixedJoint[] allJoints = FindObjectsOfType<FixedJoint>();
        FixedJoint[] cuttableJoints = cuttableObject.GetComponents<FixedJoint>();

        foreach (FixedJoint joint in cuttableJoints)
        {
            if (joint.connectedBody == null)
                continue; // Skip joints with no connectedBody

            // Check if any other object is using the same connected body
            if (!IsBodyUsedByOtherObjects(joint, allJoints))
            {
                joint.connectedBody.isKinematic = false;
            }
            Destroy(joint);
        }
        Destroy(cuttableObject.gameObject);
    }

    // Check if the connected body is used by other joints
    private bool IsBodyUsedByOtherObjects(FixedJoint currentJoint, FixedJoint[] allJoints)
    {
        foreach (FixedJoint otherJoint in allJoints)
        {
            // Skip the current joint and check for the same connected body
            if (otherJoint != currentJoint && otherJoint.connectedBody == currentJoint.connectedBody)
            {
                return true;
            }
        }
        return false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Cuttable"))
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

        float diffY = Mathf.Abs(Mathf.DeltaAngle(angleA.y + 180f, angleB.y));

        bool isZClose = diffZ <= angleTolerance || diffZRotated <= angleTolerance;
        bool isZCloseNegative = diffZNegative <= angleTolerance || diffZRotatedNegative <= angleTolerance;
        bool isYClose = diffY <= angleTolerance;

        if (isZCloseNegative || isZClose)
        {
            return true;
        }
        return false;
    }
}
