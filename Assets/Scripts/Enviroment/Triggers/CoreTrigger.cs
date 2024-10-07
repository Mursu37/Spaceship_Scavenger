using UnityEngine;
using UnityEngine.SceneManagement;

public class CoreTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Core")
        {
            PlayerPrefs.SetFloat("PlayerPosX", 17.5f);
            PlayerPrefs.SetFloat("PlayerPosY", -0.884f);
            PlayerPrefs.SetFloat("PlayerPosZ", 14.3f);

            SceneManager.LoadSceneAsync("MainMenu");
        }
    }
}
