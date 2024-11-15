using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeltdownMusic : MonoBehaviour
{
    [SerializeField] private string AlarmMusicIntro;
    [SerializeField] private string AlarmMusicLayer1;
    [SerializeField] private string AlarmMusicLayer2;
    [SerializeField] private string AlarmMusicLayer3;

    private EnergyCore core;

    private double bpm = 125;
    private int barsInIntro = 2;

    private double secondsPerBeat;
    private double introLength;

    private bool Layer2FadingIn = false;
    private bool Layer3FadingIn = false;

    private float fadeDuration = 2f;

    // Start is called before the first frame update
    private void Start()
    {
        AssignCoreReference();
        this.enabled = false;

        secondsPerBeat = 60.0 / bpm;
        introLength = secondsPerBeat * 4 * barsInIntro;
    }

    private void OnEnable()
    {
        AssignCoreReference();
    }


    private void AssignCoreReference()
    {
        core = FindObjectOfType<EnergyCore>();
        if (core == null)
        {
            Debug.LogWarning("EnergyCore not found in the scene!");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (core == null)
        {
            AssignCoreReference();
            if (core == null)
            {
                return;
            }
        }
        
        float heatPercent = core.heatAmount / core.maxHeat;

        if (heatPercent > 0.33f && !Layer2FadingIn)
        {
            StartCoroutine(FadeInLayer(AlarmMusicLayer2));
            Layer2FadingIn = true;
        } 

        if (heatPercent > 0.66f && !Layer3FadingIn)
        {
            StartCoroutine(FadeInLayer(AlarmMusicLayer3));
            Layer3FadingIn = true;
        } 
    }

    public void ActivateMeltdownMusic()
    {
        this.enabled = true;
        Invoke(nameof(ControlMeltdownMusic), 2f);
    }

    private void ControlMeltdownMusic()
    {
        AudioManager.PlayAudio(AlarmMusicIntro, 1, 1, false, null, true);

        double scheduledStartTime = AudioSettings.dspTime + introLength;

        AudioManager.PlayAudio(AlarmMusicLayer1, 1, 1, true, scheduledStartTime, true);
        AudioManager.PlayAudio(AlarmMusicLayer2, 0, 1, true, scheduledStartTime, true);
        AudioManager.PlayAudio(AlarmMusicLayer3, 0, 1, true, scheduledStartTime, true);

    }

    private IEnumerator FadeInLayer(string layername)
    {
        Sound sound = AudioManager.GetSound(layername);
        if (sound == null || sound.source == null)
        {
            Debug.LogWarning("Sound: " + layername + " not found or AudioSource is null!");
            yield break;
        }

        AudioSource layerSource = sound.source;
        float startVolume = layerSource.volume;
        float targetVolume = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            layerSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / fadeDuration);
            yield return null;
        }

        layerSource.volume = targetVolume;  // Ensure volume reaches target value
    }
}

