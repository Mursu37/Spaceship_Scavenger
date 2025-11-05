using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class CoreTrigger : MonoBehaviour
{
    [SerializeField] private GameObject victoryScreen;

    private FadeIn fadeIn;

    private bool hasFadeIn = false;

    private void Start()
    {
        fadeIn = victoryScreen.GetComponent<FadeIn>();
    }

    public void DisableInputs()
    {
        GameManager.isPaused = true;
        FindObjectOfType<PauseMenu>().enabled = false;
    }

    public void MissionCompleted()
    {
        victoryScreen.SetActive(true);
        fadeIn.StartFadeIn();
    }

    private void Update()
    {
        if (fadeIn.allFadedIn && !hasFadeIn)
        {
            GameManager.Pause();

            hasFadeIn = true;
        }
    }
}
