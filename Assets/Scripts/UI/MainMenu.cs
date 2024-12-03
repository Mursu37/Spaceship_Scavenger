using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject difficulty;
    [SerializeField] private GameObject credits;

    private FadeIn fadeIn;

    private void Awake()
    {
        CheckpointManager.ResetCheckpoints();

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        DifficultyManager.easyLevelSelected = false;
        DifficultyManager.difficultLevelSelected = false;

        fadeIn = credits.GetComponent<FadeIn>();
    }

    public void Play()
    {
        difficulty.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Credits()
    {
        credits.SetActive(true);
        fadeIn.StartFadeIn();
    }

    public void Settings()
    {
        settings.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Back()
    {
        settings.SetActive(false);
        gameObject.SetActive(true);
    }
}
