using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PauseGame;

public class PauseGame : MonoBehaviour
{
    public static PauseGame instance;
    public static bool isPaused;


    private void Start()
    {
        instance = this;
    }

    public static void Pause()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        isPaused = true;

    }

    public static void Resume()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isPaused = false;
    }

}
