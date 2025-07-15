using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSounds : MonoBehaviour
{
    [SerializeField] private string[] lightImpacts;   // Light impact sound names
    [SerializeField] private string[] mediumImpacts;  // Medium impact sound names
    [SerializeField] private string[] heavyImpacts;   // Heavy impact sound names

    [SerializeField] private float lightImpactThreshold = 1f;
    [SerializeField] private float mediumImpactThreshold = 5f;
    [SerializeField] private float heavyImpactThreshold = 8f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
    }

    // I'm not sure I know what I'm doing help appreciated
    private void OnCollisionEnter(Collision collision)
    {
        // Collision is detected from the parent object IS IT NOT???!!!
        if (collision.rigidbody == rb)
        {
            // Get the collision's relative velocity to determine impact strength
            float impactForce = collision.relativeVelocity.magnitude;

            if (impactForce < lightImpactThreshold)
            {
                PlayImpactSound(lightImpacts);
            }
            else if (impactForce < mediumImpactThreshold)
            {
                PlayImpactSound(mediumImpacts);
            }
            else if (impactForce >= heavyImpactThreshold)
            {
                PlayImpactSound(heavyImpacts);
            }
        }
    }

    // Method to play a random sound from the given array
    private void PlayImpactSound(string[] impactSounds)
    {
        if (impactSounds.Length == 0) return;

        // Randomly select a sound from the provided list
        string selectedSound = impactSounds[Random.Range(0, impactSounds.Length)];

        // Play the selected sound using AudioManager
        AudioManager.PlayAudio(selectedSound, 1, 1, false);
    }
}
