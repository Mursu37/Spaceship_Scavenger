using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private FadeIn fadeIn;

    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject blackPanel;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        fadeIn = blackPanel.GetComponent<FadeIn>();
    }

    public void Play()
    {
        PlayerPrefs.SetFloat("PlayerPosX", spawnPoint.transform.position.x);
        PlayerPrefs.SetFloat("PlayerPosY", spawnPoint.transform.position.y);
        PlayerPrefs.SetFloat("PlayerPosZ", spawnPoint.transform.position.z);

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

    private void Update()
    {
        if (fadeIn.allFadedIn)
        {
            SceneManager.LoadSceneAsync("IntroCutscene");
        }
    }
}
