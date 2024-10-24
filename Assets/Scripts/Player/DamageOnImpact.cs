using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnImpact : MonoBehaviour
{
    private Rigidbody rb;
    private Rigidbody otherRb;
    private Vector3 relativeVelocity;
    private PlayerHealth playerHealth;
    private float collisionForce;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Get the Rigidbody of the object the player collides with
        otherRb = collision.collider.GetComponent<Rigidbody>();

        if (otherRb != null)
        {
            // Get the relative velocity between your object and the other object
            relativeVelocity = collision.relativeVelocity;

            // Calculate the force of impact using the other object's mass and relative velocity
            collisionForce = relativeVelocity.magnitude * otherRb.mass;

            float amount = Mathf.RoundToInt(collisionForce * 0.8f);

            if (collisionForce > 8)
            {
                rb.GetComponent<IHealth>().Damage(amount);
            }
        }
    }
}
