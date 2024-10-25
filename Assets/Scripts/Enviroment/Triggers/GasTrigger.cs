using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasTrigger : MonoBehaviour
{
    private Rigidbody rb;
    private bool canPush = false;
    private bool canDamage = false;

    private void OnTriggerEnter(Collider other)
    {
        rb = other.transform.parent.GetComponent<Rigidbody>();
        canPush = true;
        canDamage = true;
    }

    private void OnTriggerExit(Collider other)
    {
        canPush = false;
    }

    private void FixedUpdate()
    {
        if (canPush && rb != null && rb.CompareTag("Player"))
        {
            if (canDamage)
            {
                rb.GetComponent<IHealth>().Damage(5f);
                canDamage = false;
            }
            rb.AddForce(-rb.transform.forward * 500);
        }
    }
}
