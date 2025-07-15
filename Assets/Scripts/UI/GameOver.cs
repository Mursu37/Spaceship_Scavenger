using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private FadeOut fadeOut;
    private bool hasFadedOut = false;
    private GameOverAction currentAction = GameOverAction.None;

    private enum GameOverAction
    {
        None,
        Retry,
        Return
    }

    private void Awake()
    {
        fadeOut = GetComponent<FadeOut>();
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        currentAction = GameOverAction.Retry;
        fadeOut.StartFadeOut();
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        currentAction = GameOverAction.Return;
        fadeOut.StartFadeOut();
    }

    private void Update()
    {
        if (fadeOut.allFadedOut && !hasFadedOut)
        {
            PauseGame.Resume();
            hasFadedOut = true;

            if (currentAction == GameOverAction.Retry)
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
            else if (currentAction == GameOverAction.Return)
            {
                SceneManager.LoadSceneAsync("MainMenu");
            }
        }
    }
}
