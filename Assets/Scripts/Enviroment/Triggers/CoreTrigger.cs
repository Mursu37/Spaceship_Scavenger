using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class CoreTrigger : MonoBehaviour
{
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] AudioMixer audioMixer;

    private FadeIn fadeIn;

    private bool hasFadeIn = false;

    private void Start()
    {
        fadeIn = victoryScreen.GetComponent<FadeIn>();
    }

    public void DisableInputs()
    {
        PauseGame.isPaused = true;
        FindObjectOfType<PauseMenu>().enabled = false;
    }

    public void MissionCompleted()
    {
        AudioManager.PlayAudio("MissionComplete", 1, 1, false); // This should play on contract complete screen if/when there is one
        victoryScreen.SetActive(true);
        fadeIn.StartFadeIn();
        audioMixer.SetFloat("Music", -80);
        audioMixer.SetFloat("Ambience", -80);
        audioMixer.SetFloat("Sound", -80);
    }

    private void Update()
    {
        if (fadeIn.allFadedIn && !hasFadeIn)
        {
            PauseGame.Pause();

            hasFadeIn = true;
        }
    }
}
