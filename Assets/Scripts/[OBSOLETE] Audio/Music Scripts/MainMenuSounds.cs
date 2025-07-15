using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSounds : MonoBehaviour
{
    [SerializeField] private string mainMenuMusic;
    [SerializeField] private string mainMenuAmbience; 

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            AudioManager.PlayAudio(mainMenuMusic, 1f, 1f, false);
            AudioManager.PlayAudio(mainMenuAmbience, 1f, 1f, true);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            AudioManager.StopAudio(mainMenuMusic);
            AudioManager.StopAudio(mainMenuAmbience);
            this.enabled = false;
        }
    }
}
