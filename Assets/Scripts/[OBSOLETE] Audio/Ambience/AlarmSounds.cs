using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmSounds : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.OnPhaseChanged += OnPhaseChanged;
    }

    private void OnDisable()
    {
        GameManager.OnPhaseChanged -= OnPhaseChanged;
    }

    private void OnPhaseChanged(Phase newPhase)
    {
        if (newPhase == Phase.Meltdown)
        {
            ActivateAlarmSounds();
        }
    }

    public void ActivateAlarmSounds()
    {
        AudioSource alarmAudioSource = GetComponent<AudioSource>();

        if (alarmAudioSource != null)
        {
            alarmAudioSource.Play();
        }
     else
        {
         Debug.LogError("No AudioSource found on this GameObject!");
        }
    }

}
