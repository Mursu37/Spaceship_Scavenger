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

    [SerializeField] private float damageAmount = 1f;

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
                    hit.rigidbody.GetComponent<IHealth>().Damage(damageAmount);
                    Vector3 currentVelocity = hit.rigidbody.velocity;
                    Debug.Log(currentVelocity);
                    hit.rigidbody.AddForce(currentVelocity.x * -5f, 0, currentVelocity.z * -5f,
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
