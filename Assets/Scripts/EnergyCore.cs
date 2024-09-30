using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyCore : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 relativeVelocity;
    private float collisionForce;

    [SerializeField] private float health;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Get the relative velocity between your object and the other object
        relativeVelocity = collision.relativeVelocity;

        // Calculate the force of impact
        collisionForce = relativeVelocity.magnitude;

        if (collisionForce > 2)
        {
            health -= collisionForce * 2f;
        }

        // Output the object that collided and the force of impact
        Debug.Log("Collided with: " + collision.gameObject.name);
        Debug.Log("Collision force: " + collisionForce + " N");
    }
}
