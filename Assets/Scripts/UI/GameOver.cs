using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void Retry()
    {
        PauseGame.Resume();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void ReturnToMainMenu()
    {
        PauseGame.Resume();
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
