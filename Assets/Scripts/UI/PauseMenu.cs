using UnityEngine;
using System;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject controlsPanel;


    private bool isPaused;
    private bool isViewingControls = false;

    public static event Action OnPause;
    public static event Action onResume;

    private void Update()
    {
        HandlePauseInput();
        HandleControlsInput();
    }

    private void HandlePauseInput()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    private void HandleControlsInput()
    {
        if (Input.GetButtonDown("OpenControls"))
        {
            if (!isPaused)
            {
                OpenControlsFromGame();
            } else if (isViewingControls)
            {
                CloseControls();
            }
        }
    }

    public void Pause()
    {
        if (isPaused) return;

        isPaused = true;
        pausePanel.SetActive(true);
        PauseGame.Pause(PauseGame.TransitionType.PauseMixer);
        ShowPanel(pauseMenu);
        OnPause?.Invoke();
    }

    public void Resume()
    {
        if (!isPaused) return;

        isPaused = false;
        pausePanel.SetActive(false);
        PauseGame.Resume(PauseGame.TransitionType.UnpauseMixer);
        onResume?.Invoke();
    }

    public void OpenControlsFromGame()
    {
        isViewingControls = true;
        Pause();
        ShowPanel(controlsPanel);
    }

    public void CloseControls()
    {
        isViewingControls = false;
        ShowPanel(pauseMenu);
        Resume();
    }

    public void Settings()
    {
        ShowPanel(settingsPanel);
    }

    public void Controls()
    {
        ShowPanel(controlsPanel);
    }

    private void ShowPanel(GameObject panelToShow)
    {
        //Deactivate all panels
        pauseMenu.SetActive(false);
        settingsPanel.SetActive(false);
        controlsPanel.SetActive(false);

        //Activate the submitted panel
        panelToShow.SetActive(true);
    }

    public void BackToPauseMenu()
    {
        ShowPanel(pauseMenu);
    }

    public void Retry()
    {
        Time.timeScale = 1; // ensure the game is unpaused
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        PauseGame.Resume(PauseGame.TransitionType.UnpauseMixer);
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        PauseGame.Resume(PauseGame.TransitionType.UnpauseMixer);
    }

}