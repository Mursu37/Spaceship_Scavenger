using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject spawnPoint;

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
}
