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
