using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSounds : MonoBehaviour
{

[SerializeField] private string[] explosionSounds; 

    private void Awake()
    {
        if (explosionSounds != null && explosionSounds.Length > 0)
        {
            // Select a random explosion sound
            int randomIndex = Random.Range(0, explosionSounds.Length);
            string selectedSound = explosionSounds[randomIndex];
            
            // Play explosion
            AudioManager.PlayAudio(selectedSound, 1, 1, false);
        }
        else
        {
            Debug.LogWarning("ExplosionSounds array is empty or not assigned!");
        }
    }
}
