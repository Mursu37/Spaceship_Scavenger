using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallExplosionSounds : MonoBehaviour
{
    //This is an experiment and a lazy workaround, and it does not work like I want it to pls ignore

    [SerializeField] private AudioClip[] smallExplosions;
    [SerializeField] private float volume = 1.0f;

    private void Awake()
    {
        if (smallExplosions.Length == 0)
        {
            Debug.LogWarning("No explosion clips assigned to SmallExplosionSounds.");
            return;
        }

        int randomIndex = Random.Range(0, smallExplosions.Length);
        AudioClip selectedClip = smallExplosions[randomIndex];

        AudioSource.PlayClipAtPoint(selectedClip, transform.position, volume);
    }
}
