using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject controls;

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        PauseGame.Pause(PauseGame.TransitionType.PauseMixer);
    }

    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        PauseGame.Resume(PauseGame.TransitionType.UnpauseMixer);
    }

    public void Retry()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        PauseGame.Resume(PauseGame.TransitionType.UnpauseMixer);
    }

    public void Settings()
    {
        settings.SetActive(true);
        controls.SetActive(false);
        pauseMenu.SetActive(false);
    }

    public void Controls()
    {
        controls.SetActive(true);
        settings.SetActive(false);
        pauseMenu.SetActive(false);
    }

    public void Back()
    {
        settings.SetActive(false);
        controls.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void Quit()
    {
        SceneManager.LoadSceneAsync("MainMenu");
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        PauseGame.Resume(PauseGame.TransitionType.UnpauseMixer);
    }
}
