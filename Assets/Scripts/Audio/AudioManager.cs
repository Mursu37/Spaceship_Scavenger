using System;
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;

    public static AudioManager instance;

    private static List<GameObject> tempAudioObjects = new List<GameObject>();

    private void Awake()
    {
        instance = this;

        foreach (Sound s in sounds)
        {
            GameObject sourceObject = null;

            if (s.sourceObject != null)
            {
                // Try to find the specified object by name in the scene or as a child
                sourceObject = s.sourceObject;
            }
    
            if (sourceObject == null)
            {
                sourceObject = gameObject; // Default to AudioManager if no specific source object is found
            }
            s.source = sourceObject.AddComponent<AudioSource>();
            
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.mixerGroup;

            s.source.spatialBlend = s.spatialBlend;
            s.source.minDistance = s.minDistance;
            s.source.maxDistance = s.maxDistance;
            s.source.rolloffMode = s.rolloffMode;

            s.source.bypassReverbZones = s.bypassReverbZones;
            s.source.bypassEffects = s.bypassEffects;

            s.source.playOnAwake = false;
            s.source.loop = s.shouldLoop;
            //s.source.volume = s.volume;
        }
    }

    public static void PlayAudio(string name, float volume = 1, float pitch = 1, bool loop = true, double? scheduledStartTime = null, bool ignorePause = false)
    {
        if (instance == null)
        {
            Debug.LogWarning("AudioManager instance is not set!");
            return;
        }

        Sound s = Array.Find(instance.sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        if (s.source == null)
        {
            Debug.LogWarning("AudioSource for sound: " + name + " is not assigned!");
            return;
        }
        s.source.volume = volume;
        s.source.pitch = pitch;
        s.source.loop = loop;
        s.source.ignoreListenerPause = ignorePause;

        if (scheduledStartTime.HasValue)
        {
            s.source.PlayScheduled(scheduledStartTime.Value);
        }
        else
        {
           s.source.Play(); 
        }
    }

    //Create a temp GameObject for a sound in space, good for one shot sounds that don't need to be moved. //Use with AudioManager.PlayModifiedClipAtPoint(); in a script attached  to a gameobject or child.
    public static void PlayModifiedClipAtPoint(string clipName, Vector3 position, float volume = 1f, float spatialBlend = 1f, float minDistance = 1f, float maxDistance = 500f, bool loop = false)
    {
        Sound s = Array.Find(instance.sounds, sound => sound.name == clipName);

        if (s == null || s.clip == null)
        {
            Debug.LogWarning($"Sound {clipName} not found!");
            return;
        }

        // Create a new GameObject to play the sound
        GameObject tempAudioObject = new GameObject("TempAudio_" + clipName);
        AudioSource tempAudioSource = tempAudioObject.AddComponent<AudioSource>();

        tempAudioSource.clip = s.clip;
        tempAudioSource.volume = volume;
        tempAudioSource.spatialBlend = spatialBlend;
        tempAudioSource.minDistance = minDistance;
        tempAudioSource.maxDistance = maxDistance;
        tempAudioSource.rolloffMode = s.rolloffMode; 
        tempAudioSource.loop = false;
        tempAudioSource.outputAudioMixerGroup = s.mixerGroup;

        // Position the GameObject at the desired location and play
        tempAudioObject.transform.position = position;
        tempAudioObjects.Add(tempAudioObject);

        tempAudioSource.Play();

        // If the sound is not looping, destroy IT!!
        if (!loop)
        {
            UnityEngine.Object.Destroy(tempAudioObject, s.clip.length / tempAudioSource.pitch);
        }
    }

    //Creates a temp object for sound playback that follows the parent //Use with AudioManager.PlayFollowedAudio(); in a script attached to a gameobject or child
    public static void PlayFollowedAudio(string clipName, GameObject parentObject, float volume = 1f, float spatialBlend = 1f, bool loop = false)
    {
        Sound s = Array.Find(instance.sounds, sound => sound.name == clipName);

        if (s == null || s.clip == null)
        {
            Debug.LogWarning($"Sound {clipName} not found!");
            return;
        }

        // Create a temporary GameObject for the audio and set its parent
        GameObject tempAudioObject = new GameObject("TempAudio_" + clipName);
        tempAudioObject.transform.SetParent(parentObject.transform);
        tempAudioObjects.Add(tempAudioObject);
    
        tempAudioObject.transform.localPosition = Vector3.zero;

        AudioSource tempAudioSource = tempAudioObject.AddComponent<AudioSource>();

        tempAudioSource.clip = s.clip;
        tempAudioSource.volume = volume;
        tempAudioSource.spatialBlend = spatialBlend;
        tempAudioSource.minDistance = s.minDistance;
        tempAudioSource.maxDistance = s.maxDistance;
        tempAudioSource.rolloffMode = s.rolloffMode;
        tempAudioSource.loop = loop;
        tempAudioSource.outputAudioMixerGroup = s.mixerGroup;

        tempAudioSource.Play();

        // If the sound is not looping, destroy IT!!
        if (!loop)
        {
            UnityEngine.Object.Destroy(tempAudioObject, s.clip.length / tempAudioSource.pitch);
        }
    }

    public static void StopAudio(string name)
    {
        Sound s = Array.Find(instance.sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    public static bool IsPlaying(string name)
    {
        Sound s = Array.Find(instance.sounds, sound => sound.name == name);
        return s.source.isPlaying;
    }

    public static void SetVolume(string name, float volume)
    {
        Sound s = Array.Find(instance.sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.volume = volume;
    }

    public static Sound GetSound(string name)
    {
        return Array.Find(instance.sounds, sound => sound.name == name);
    }


    public static void StopAllAudio()
    {
        if (instance == null)
        {
            Debug.LogWarning("AudioManager instance is not set!");
            return;
        }

        foreach (Sound s in instance.sounds)
        {
            if (s.source != null && s.source.isPlaying)
            {
                s.source.Stop();
            }
        }

        foreach (var tempObj in tempAudioObjects)
        {
            if (tempObj != null)
            {
                Destroy(tempObj);
            }
        }
        tempAudioObjects.Clear();
    }
}
