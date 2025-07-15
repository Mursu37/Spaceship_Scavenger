using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MixerController : MonoBehaviour
{
    //Pause menu related mixer snapshots
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private AudioMixerSnapshot pausedSnapshot;
    [SerializeField] private AudioMixerSnapshot unpausedSnapshot;

    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private AudioMixerSnapshot musicLowPass;
    [SerializeField] private AudioMixerSnapshot musicNormal;


    private void Start()
    {
        unpausedSnapshot.TransitionTo(0f);
    }

    public void PauseMixerTransition()
    {
        pausedSnapshot.TransitionTo(0.0f);
    }

    public void UnpauseMixerTransition()
    {
        unpausedSnapshot.TransitionTo(0.0f);
    }

    public void LowPassMusicTransition()
    {
        musicLowPass.TransitionTo(0.0f);
    }

    public void NormalMusicTransition()
    {
        musicNormal.TransitionTo(0.0f);
    }


}
