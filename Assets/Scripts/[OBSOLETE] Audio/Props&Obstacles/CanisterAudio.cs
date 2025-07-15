using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CanisterAudio : MonoBehaviour
{
    [SerializeField] private string fuzeSound; 
    public void PlayFuzeSound(GameObject parentObject)
    {
        AudioManager.PlayFollowedAudio(fuzeSound, parentObject, 1, 1, false);
    }
}
