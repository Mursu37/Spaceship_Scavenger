using UnityEngine;

public enum SoundType
{
    Movemement,
    Multitool,
    Environment
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    private AudioSource audioSource;

    [SerializeField] private AudioClip[] soundList;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Plays one sound from the sould list
    public static void PlaySound(AudioSource source, int sound, float volume = 1, float pitch = 1)
    {
        source.clip = instance.soundList[sound];
        source.volume = volume;
        source.pitch = pitch;
        if (!source.isPlaying)
        {
            source.Play();
        }
    }

    // Plays the sound only once
    public static void PlaySoundOnce(AudioSource source, int sound, float volume = 1, float pitch = 1)
    {
        source.PlayOneShot(instance.soundList[sound], volume);
        source.pitch = pitch;
    }

    public static void StopSound(AudioSource source)
    {
        if (source.isPlaying)
        {
            source.Stop();
        }
    }

    public static bool IsPlaying(AudioSource source)
    {
        return source.isPlaying;
    }
}
