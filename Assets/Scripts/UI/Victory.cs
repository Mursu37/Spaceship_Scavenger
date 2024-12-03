using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour
{
    [SerializeField] private GameObject credits;

    private FadeOut fadeOut;
    private bool hasFadedOut = false;
    private bool creditsActive = false;

    private void Start()
    {
        fadeOut = GetComponent<FadeOut>();
    }

    public void Credits()
    {
        credits.SetActive(true);
        creditsActive = true;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        fadeOut.StartFadeOut();
    }

    private void Update()
    {
        if (fadeOut.allFadedOut && !hasFadedOut)
        {
            PauseGame.Resume();
            hasFadedOut = true;
            SceneManager.LoadSceneAsync("MainMenu");
        }

        if (Input.GetButtonDown("Cancel") && creditsActive)
        {
            credits.SetActive(false);
            creditsActive = false;
        }
    }
}
