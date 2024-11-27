using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private FadeIn fadeIn;

    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject blackPanel;

    private void Awake()
    {
        CheckpointManager.ResetCheckpoints();

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        fadeIn = blackPanel.GetComponent<FadeIn>();
    }

    public void Play()
    {
        blackPanel.SetActive(true);
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
    private void Update()
    {
        if (fadeIn.allFadedIn)
        {
            SceneManager.LoadSceneAsync("IntroCutscene");
        }
    }
}
