using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Explosives : MonoBehaviour
{
    [SerializeField] private int damage = 20;
    [SerializeField] private float explodeRadius = 15f;
    [SerializeField] private float explosionForce = 15f;
    [SerializeField] private GameObject explodeEffect;
    [SerializeField] private GameObject fragments;


    public void Explode()
    {
        if (gameObject.GetComponent<Collider>() != null)
        {
            gameObject.GetComponent<Collider>().enabled = false;
        }

        Instantiate(fragments, transform.position, Quaternion.identity);
        Collider[] colliderHit = Physics.OverlapSphere(transform.position, explodeRadius);

        foreach (Collider collider in colliderHit)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explodeRadius, 1f, ForceMode.Impulse);
            }

            if (collider.CompareTag("Explodable"))
            {
                Destroy(collider.gameObject);
            }

            if (collider.CompareTag("Explosive"))
            {
                collider.GetComponent<Explosives>().Explode();
            }

            if (collider.transform.parent != null && collider.transform.parent.CompareTag("Player"))
            {
                PlayerHealth player = collider.transform.parent.GetComponent<PlayerHealth>();
                Rigidbody playerRb = collider.transform.parent.GetComponent<Rigidbody>();
                if (player != null)
                {
                    if (damage <= 20f)
                    {
                        player.Damage(damage, 0.1f);
                    }
                    else
                    {
                        player.Damage(damage, 0.4f);
                    }
                }

                if (playerRb != null)
                {
                    playerRb.AddExplosionForce(10, transform.position, explodeRadius, 1f, ForceMode.Impulse);
                }
            }
        }

        Instantiate(explodeEffect, transform.position, Quaternion.identity);

        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
