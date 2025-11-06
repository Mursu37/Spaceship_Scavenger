using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private FadeIn fadeIn;
    private bool hasFadeIn = false;
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
        fadeIn = GetComponent<FadeIn>();
        fadeOut = GetComponent<FadeOut>();
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState gameState)
    {
        if (gameState == GameState.GameOver)
        {
            if (GameManager.instance != null && GameManager.instance.instantGameOverRequested)
            {

            }
            else
            {
                fadeIn.StartFadeIn();
            }
        }
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
        if (fadeIn.allFadedIn && !hasFadeIn)
        {
            GameManager.Pause();
            hasFadeIn = true;
        }

        if (fadeOut.allFadedOut && !hasFadedOut)
        {
            GameManager.Resume();
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
