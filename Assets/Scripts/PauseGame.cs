using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PauseGame;

public class PauseGame : MonoBehaviour
{
    public static PauseGame instance;
    public static bool isPaused;

    private MixerController mixerController;

    public enum TransitionType
    {
        None,
        PauseMixer,
        UnpauseMixer,
        LowPassMusic,
        NormalMusic
    }

    private void Start()
    {
        instance = this;
        mixerController = FindObjectOfType<MixerController>();
    }

    public static void Pause(TransitionType transitionType = TransitionType.None)
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        AudioListener.pause = true;

        isPaused = true;

        instance.HandleTransition(transitionType);
    }

    public static void Resume(TransitionType transitionType = TransitionType.None)
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        AudioListener.pause = false;

        isPaused = false;

        instance.HandleTransition(transitionType);
    }

    private void HandleTransition(TransitionType transitionType)
    {
        switch (transitionType)
        {
            case TransitionType.PauseMixer:
                instance.mixerController?.PauseMixerTransition();
                break;
            case TransitionType.UnpauseMixer:
                instance.mixerController?.UnpauseMixerTransition();
                break;
            case TransitionType.LowPassMusic:
                instance.mixerController?.LowPassMusicTransition();
                break;
            case TransitionType.NormalMusic:
                instance.mixerController?.NormalMusicTransition();
                break;
        }
    }
}
