using UnityEngine;
using UnityEngine.SceneManagement;

public class CoreTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Core")
        {
            SceneManager.LoadSceneAsync("MainMenu");
        }
    }
}
