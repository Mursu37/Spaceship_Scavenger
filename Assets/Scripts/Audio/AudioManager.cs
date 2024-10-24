using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;

    public static AudioManager instance;

    private void Awake()
    {
        instance = this;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.mixerGroup;

            s.source.spatialBlend = s.spatialBlend;
            s.source.minDistance = s.minDistance;
            s.source.maxDistance = s.maxDistance;
            s.source.rolloffMode = s.rolloffMode;

            s.source.bypassReverbZones = s.bypassReverbZones;
            s.source.bypassEffects = s.bypassEffects;
        }
    }

    public static void PlayAudio(string name, float volume = 1, float pitch = 1, bool loop = true)
    {
        Sound s = Array.Find(instance.sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.volume = volume;
        s.source.pitch = pitch;
        s.source.loop = loop;
        s.source.Play();
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
}
