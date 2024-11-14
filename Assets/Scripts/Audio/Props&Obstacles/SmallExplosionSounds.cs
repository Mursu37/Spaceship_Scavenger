using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallExplosionSounds : MonoBehaviour
{
    [SerializeField] private string[] smallExplosions;

    private void Awake()
    {
        if (smallExplosions.Length == 0)
        {
            Debug.LogWarning("No clip names assigned to SmallExplosionSounds.");
            return;
        }

        int randomIndex = Random.Range(0, smallExplosions.Length);
        string selectedClip = smallExplosions[randomIndex];

        AudioManager.PlayModifiedClipAtPoint(
            selectedClip,
            transform.position,
            1f,
            1f,
            1f,
            300f
        );

    }
}
