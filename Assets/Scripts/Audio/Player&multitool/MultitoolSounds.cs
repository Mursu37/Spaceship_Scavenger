using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultitoolSounds : MonoBehaviour
{

private LineRenderer lineRenderer; 

//Pitch modulation range
[SerializeField] private float minPitch = 0.8f;
[SerializeField] private float maxPitch = 1.2f;

//names arrays for audio manager
[SerializeField] private string[] gravityTransients;
[SerializeField] private string[] gravityEndSounds;
[SerializeField] private string gravityHumSound;


    // Start is called before the first frame update
    private void Start()
    {
        lineRenderer = GetComponentInParent<LineRenderer>();

        if(lineRenderer == null)
        {
            Debug.LogWarning("No LineRenderer found in the parent object.");
        }

        // Ensure arrays are set
        if(gravityTransients.Length == 0 || gravityEndSounds.Length == 0)
        {
            Debug.LogWarning("Gravity transient or end sounds not set in inspector.");
        }

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
        if(!AudioManager.IsPlaying(gravityHumSound))
        {
            string selectedTransient = GetRandomSoundName(gravityTransients);
            float randomPitch = Random.Range(minPitch, maxPitch);

            AudioManager.PlayAudio(selectedTransient, 1, randomPitch, false);
            AudioManager.PlayAudio(gravityHumSound, 1, 1, true);
        }
    }
    //Play end sounds and stop loop
    private void PlayGravityEndSounds()
    {
        if(AudioManager.IsPlaying(gravityHumSound))
        {
            string selectedEndSound = GetRandomSoundName(gravityEndSounds);
            float randomPitch = Random.Range(minPitch, maxPitch);

            AudioManager.PlayAudio(selectedEndSound, 1, randomPitch, false);
            AudioManager.StopAudio(gravityHumSound); 
        }
    }

    // Helper method to select a random sound from a string array
    private string GetRandomSoundName(string[] soundNames)
    {
        int randomIndex = Random.Range(0, soundNames.Length);
        return soundNames[randomIndex];
    }

}
