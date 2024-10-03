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

    public static void PlaySound(int sound, float volume = 1)
    {
        instance.audioSource.clip = instance.soundList[sound];
        instance.audioSource.volume = volume;
        if (!instance.audioSource.isPlaying)
        {
            instance.audioSource.Play();
        }
    }

    public static void StopSound()
    {
        if (instance.audioSource.isPlaying)
        {
            instance.audioSource.Stop();
        }
    }
}
