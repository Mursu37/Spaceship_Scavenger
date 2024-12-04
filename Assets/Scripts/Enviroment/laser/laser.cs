using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class laser : MonoBehaviour
{
    private LineRenderer lineRenderer;

    private float damageCooldown;
    private float damageCooldownTimer;

    private bool inCollision = false;
    private Vector3 collisionForce = Vector3.zero;

    // Ignore tool and cutting colliders
    [SerializeField] private LayerMask layerMask = Physics.AllLayers;

    [SerializeField] private float damageAmount = 2f;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        damageCooldown = 0.1f;
        damageCooldownTimer = 0;
    }

    private void Update()
    {
        if (Physics.Raycast(new Ray(transform.position, transform.forward), out RaycastHit hit, 100f, layerMask))
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hit.point);

            if (hit.rigidbody != null)
            {
                if (hit.rigidbody.GetComponent<IHealth>() != null && damageCooldownTimer <= 0)
                {
                    hit.rigidbody.GetComponent<IHealth>().Damage(damageAmount);
                    Vector3 currentVelocity = hit.rigidbody.velocity;
                    Debug.Log(currentVelocity);
                    float scale = 0.75f;
                    float x = (currentVelocity.x < 0) ? scale : -scale;
                    float y = (currentVelocity.y < 0) ? scale : -scale;
                    float z = (currentVelocity.z < 0) ? scale : -scale;
                    //hit.rigidbody.AddExplosionForce(500, hit.point, 10);
                    if (inCollision)
                    {
                        Debug.Log("still in collision");
                        collisionForce *= 1.1f;
                    }
                    else
                    {
                    collisionForce = new Vector3(currentVelocity.x * -1.25f + x, currentVelocity.y * -1.25f + y,
                        currentVelocity.z * -1.25f + z);
                    }
                    hit.rigidbody.AddForce(collisionForce,
                        ForceMode.VelocityChange);
                    damageCooldownTimer = damageCooldown;
                    inCollision = true;
                }

                if (hit.rigidbody.CompareTag("Explosive"))
                {
                    Explosives explosives = hit.rigidbody.GetComponent<Explosives>();
                    if (explosives != null)
                    {
                        explosives.Explode();
                    }
                }
            }
        }
        else
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + transform.forward * 100f);
        }

        if (damageCooldownTimer > 0)
        {
            damageCooldownTimer -= Time.deltaTime;
        }
        else
        {
            inCollision = false;
        }
    }
}
