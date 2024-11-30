using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmSounds : MonoBehaviour
{

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
