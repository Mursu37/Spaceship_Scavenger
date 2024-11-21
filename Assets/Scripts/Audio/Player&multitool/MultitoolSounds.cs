using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultitoolSounds : MonoBehaviour
{

private LineRenderer lineRenderer; 
private LineRenderer[] laserRenderers;

//Pitch modulation range
[SerializeField] private float minPitch = 0.8f;
[SerializeField] private float maxPitch = 1.2f;

//names arrays for audio manager
[SerializeField] private string[] gravityTransients;
[SerializeField] private string[] gravityEndSounds;
[SerializeField] private string gravityHumSound;

[SerializeField] private string[] cuttingSounds;

[SerializeField] private Animator animator;
private AnimatorStateInfo previousStateInfo;
private bool hasInitialized = false;

private bool isCuttingSoundPlaying = false;

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


        laserRenderers = this.transform.parent.GetComponentsInChildren<LineRenderer>();

        StartCoroutine(InitializeAnimatorState());

    }

    private IEnumerator InitializeAnimatorState()
    {
        // Wait until the end of frame to ensure Animator has fully set up its initial state
        yield return new WaitForEndOfFrame();

        if (animator != null)
        {
            previousStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            hasInitialized = true;
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

        // Check if any of the laserRenderers are active for cutting sounds
        bool anyLaserActive = false;
        foreach (LineRenderer laser in laserRenderers)
        {
            if (laser != lineRenderer && laser.enabled)
            {
                anyLaserActive = true;
                break;
            }
        }

        if (anyLaserActive)
        {
            PlayCuttingSound();
        }
        else
        {
            isCuttingSoundPlaying = false;  // Reset flag when no laser is active
        }

        if (animator != null && hasInitialized)
        {
            AnimatorStateInfo currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            
                if ((currentStateInfo.IsName("HorizontalCut") && (previousStateInfo.IsName("VerticalCut") || previousStateInfo.IsName("Idle"))) ||
                    (currentStateInfo.IsName("VerticalCut") && (previousStateInfo.IsName("HorizontalCut") || previousStateInfo.IsName("Idle"))))
                {
                    PlayCuttingModeSwitchSound();
                }
            

            previousStateInfo = currentStateInfo;
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

    // Play cutting sound
    private void PlayCuttingSound()
    {
        if (!isCuttingSoundPlaying)
        {
            string selectedCuttingSound = GetRandomSoundName(cuttingSounds);
            ///float randomPitch = Random.Range(minPitch, maxPitch);

            AudioManager.PlayAudio(selectedCuttingSound, 1, 1, false);
            isCuttingSoundPlaying = true;
        }
    }


    // Helper method to select a random sound from a string array
    private string GetRandomSoundName(string[] soundNames)
    {
        int randomIndex = Random.Range(0, soundNames.Length);
        return soundNames[randomIndex];
    }


    private void PlayCuttingModeSwitchSound()
    {
        float randomPitch = Random.Range(0.95f, 1.05f);
        AudioManager.PlayAudio("MultitoolAngleSwitch", 1, randomPitch, false);
    }

}
