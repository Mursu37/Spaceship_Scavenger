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

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        damageCooldown = 0.1f;
        damageCooldownTimer = 0;
    }

    private void Update()
    {
        if (Physics.Raycast(new Ray(transform.position, transform.forward), out RaycastHit hit))
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hit.point);

            if (hit.rigidbody != null)
            {
                if (hit.rigidbody.GetComponent<IHealth>() != null && damageCooldownTimer <= 0)
                {
                    hit.rigidbody.GetComponent<IHealth>().Damage(1f);
                    Vector3 currentVelocity = hit.rigidbody.velocity;
                    Debug.Log(currentVelocity);
                    //hit.rigidbody.AddExplosionForce(500, hit.point, 10);
                    hit.rigidbody.AddForce(currentVelocity.x * -2f, currentVelocity.y * -2f, currentVelocity.z * -2f,
                        ForceMode.VelocityChange);
                    damageCooldownTimer = damageCooldown;
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
    }
}
