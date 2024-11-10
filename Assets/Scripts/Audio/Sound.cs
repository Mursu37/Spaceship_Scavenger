using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public AudioMixerGroup mixerGroup;

    [Range(0f, 1f)] public float spatialBlend = 0f; // 0 = 2D, 1 = 3D
    public float minDistance = 1f;
    public float maxDistance = 500f;
    public AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;

    public bool bypassReverbZones = false;
    public bool bypassEffects = false;
    public bool shouldLoop = true;

    public GameObject sourceObject;

    [HideInInspector]
    public AudioSource source;
}
