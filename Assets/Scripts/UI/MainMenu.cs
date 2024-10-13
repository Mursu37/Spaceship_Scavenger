using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject settings;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void Play()
    {
        PlayerPrefs.SetFloat("PlayerPosX", spawnPoint.transform.position.x);
        PlayerPrefs.SetFloat("PlayerPosY", spawnPoint.transform.position.y);
        PlayerPrefs.SetFloat("PlayerPosZ", spawnPoint.transform.position.z);

        SceneManager.LoadSceneAsync("MainGame");
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
}
