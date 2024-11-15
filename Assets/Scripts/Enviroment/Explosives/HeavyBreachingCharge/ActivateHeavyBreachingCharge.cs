using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateHeavyBreachingCharge : CuttingPointManager
{
    [SerializeField] private GameObject canisters;

    private void Start()
    {
        FindCuttableObjects(transform);
    }

    private void Update()
    {
        if (canisters == null)
        {
            return;
        }

        if (AreCuttingPointsNull())
        {
            if (canisters.transform != null)
            {
                canisters.transform.SetParent(null);
                canisters.GetComponent<CanisterDetonation>().enabled = true;
            }
            Rigidbody rb = canisters.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = canisters.AddComponent<Rigidbody>();
                rb.interpolation = RigidbodyInterpolation.Interpolate;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

                rb.AddForce(rb.transform.forward * 10f);
            }
        }
    }
}
