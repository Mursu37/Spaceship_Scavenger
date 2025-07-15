using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceManager : MonoBehaviour
{
[SerializeField] private string derelictAmbience;
[SerializeField] private string meltdownAmbience;

private MeltdownPhase meltdownPhase;
private bool ambienceChanged = false;

    // Start is called before the first frame update
    private void Start()
    {
        meltdownPhase = FindObjectOfType<MeltdownPhase>();
        if (meltdownPhase == null)
        {
            Debug.LogWarning("MeldownPhase not found");
        }

        AudioManager.PlayAudio(derelictAmbience, 1, 1, true, null, true);
    }

    // Update is called once per frame
    private void Update()
    {
        if (meltdownPhase != null && meltdownPhase.enabled && !ambienceChanged)
        {
            ChangeAmbience();
            ambienceChanged = true;
        }
    }

    private void ChangeAmbience()
    {
        StartCoroutine(CrossfadeAmbience(5f));
    }

    private IEnumerator CrossfadeAmbience(float duration) //ChatGPT AHH generated crossfade
    {
        float elapsedTime = 0f;

        // Start playing the meltdown ambience at 0 volume if not already playing
        if (!AudioManager.IsPlaying(meltdownAmbience))
        {
            AudioManager.PlayAudio(meltdownAmbience, 0, 1, true, null, true);
        }

        // Initial volume values
        float initialVolumeDerelict = 1f; // Assuming the initial volume is 1
        float targetVolumeMeltdown = 1f;  // Target volume for the new ambience

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Gradually decrease the volume of the derelict ambience and increase the meltdown ambience
            float derelictVolume = Mathf.Lerp(initialVolumeDerelict, 0, t);
            float meltdownVolume = Mathf.Lerp(0, targetVolumeMeltdown, t);

            AudioManager.SetVolume(derelictAmbience, derelictVolume);
            AudioManager.SetVolume(meltdownAmbience, meltdownVolume);

            yield return null; // Wait for the next frame
        }

        // Ensure final values are set
        AudioManager.SetVolume(derelictAmbience, 0);
        AudioManager.SetVolume(meltdownAmbience, targetVolumeMeltdown);

        // Stop the derelict ambience after crossfade
        AudioManager.StopAudio(derelictAmbience);
}

    public void StopAmbience()
    {
        AudioManager.StopAudio(derelictAmbience);
        AudioManager.StopAudio(meltdownAmbience);
    }
}
