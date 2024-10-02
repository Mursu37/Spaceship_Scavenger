using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultitoolSounds : MonoBehaviour
{
//Audio Sources multitool gravity mode, assign in inspector
[SerializeField] private AudioSource gravityTransient1;
[SerializeField] private AudioSource gravityTransient2;
[SerializeField] private AudioSource gravityTransient3;
[SerializeField] private AudioSource gravityHum;
[SerializeField] private AudioSource gravityEnd1;
[SerializeField] private AudioSource gravityEnd2;
[SerializeField] private AudioSource gravityEnd3;

private LineRenderer lineRenderer; 

//Pitch modulation range
private float minPitch = 0.8f;
private float maxPitch = 1.2f;

private AudioSource[] transientSounds;
private AudioSource[] endSounds;


    // Start is called before the first frame update
    private void Start()
    {
        lineRenderer = GetComponentInParent<LineRenderer>();

        if(lineRenderer == null)
        {
            Debug.LogWarning("No LineRenderer found in the parent object.");
        }

        // Initialize arrays with the transient and end sounds
        transientSounds = new AudioSource[] { gravityTransient1, gravityTransient2, gravityTransient3 };
        endSounds = new AudioSource[] { gravityEnd1, gravityEnd2, gravityEnd3 };

    }

    // Update is called once per frame
    private void Update()
    {
        //Check if line renderer is active
        if(lineRenderer != null && lineRenderer.enabled)
        {
            PlayGravityStartSounds();
        }
        else
        {
            PlayGravityEndSounds();
        }
    }

    //Play gravity start sounds and loop
    private void PlayGravityStartSounds()
    {
        if(!gravityHum.isPlaying)
        {
            AudioSource selectedTransient = GetRandomSound(transientSounds);
            selectedTransient.pitch = Random.Range(minPitch, maxPitch); //Pitch modulation
            selectedTransient.Play();

            gravityHum.Play();
        }
    }
    //Play end sounds and stop loop
    private void PlayGravityEndSounds()
    {
        if(gravityHum.isPlaying)
        {
            AudioSource selectedEndSound = GetRandomSound(endSounds);
            selectedEndSound.pitch = Random.Range(minPitch, maxPitch); // Pitch modulation
            selectedEndSound.Play();

            gravityHum.Stop();
        }
    }

    // Helper method to select a random sound from an array
    private AudioSource GetRandomSound(AudioSource[] sounds)
    {
        int randomIndex = Random.Range(0, sounds.Length);
        return sounds[randomIndex];
    }

}
