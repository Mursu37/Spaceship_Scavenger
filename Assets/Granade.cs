using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
    {
        // Private variables
        [SerializeField] private float delay = 1.0f;
        [SerializeField] private float radius = 5.0f;
        [SerializeField] private float force = 1500.0f;
        [SerializeField] private bool explodeOnCollision = false;
        [SerializeField] private GameObject effectsPrefab;
        [SerializeField] private float effectDisplayTime = 3.0f;

        private float delayTimer;

        // Unity methods
        private void Awake()
        {
            delayTimer = 0.0f;
        }

        private void Update()
        {
            delayTimer += Time.deltaTime;

            if (delayTimer >= delay && !explodeOnCollision)
            {
                DoExplosion();
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (explodeOnCollision && enabled)
            {
                DoExplosion();
                Destroy(gameObject);
            }
        }

        // Helper methods
        private void DoExplosion()
        {
            HandleEffects();
            HandleDestruction();
        }

        private void HandleEffects()
        {
            if (effectsPrefab != null)
            {
                GameObject effect = Instantiate(effectsPrefab, transform.position, Quaternion.identity);
                Destroy(effect, effectDisplayTime);
            }
        }

        private void HandleDestruction()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

            foreach (Collider collider in colliders)
            {
                Rigidbody rigidbody = collider.GetComponent<Rigidbody>();

                if (rigidbody != null)
                {
                    rigidbody.AddExplosionForce(force, transform.position, radius);
                }
            }
        }
    }