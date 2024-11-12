using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused;
    private MixerController mixerController;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject controls;

    private void Start()
    {
        mixerController = FindObjectOfType<MixerController>();
    }

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
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        mixerController?.PauseMixerTransition();
    }

    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mixerController?.UnpauseMixerTransition();
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        mixerController?.UnpauseMixerTransition();
    }

    public void Settings()
    {
        settings.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void Controls()
    {
        controls.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync("MainMenu");
        mixerController?.UnpauseMixerTransition();
    }
}
