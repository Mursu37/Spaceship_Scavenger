using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialPopUp : MonoBehaviour
{
    public static TutorialPopUp instance;

    public GameObject[] tutorialWindows;

    private GameObject currentTutorial;
    private PauseMenu pauseMenu;

    private void Awake()
    {
        instance = this;
        pauseMenu = FindObjectOfType<PauseMenu>();
        StartCoroutine(ShowFirstTutorial());
    }

    public static void ShowTutorial(GameObject tutorial)
    {
        if (instance.tutorialWindows.Contains(tutorial))
        {
            if (!CheckpointManager.tutorialsShowed.Contains(tutorial.name))
            {
                tutorial.SetActive(true);
                instance.pauseMenu.enabled = false;
                PauseGame.Pause();
                instance.currentTutorial = tutorial;
                CheckpointManager.tutorialsShowed.Add(tutorial.name);
            }
        }
    }

    private IEnumerator ShowFirstTutorial()
    {
        yield return new WaitForSeconds(4f);
        ShowTutorial(tutorialWindows[0]);
    }

    private void Update()
    {
        if(Input.GetButtonDown("Submit"))
        {
            if (currentTutorial != null)
            {
                currentTutorial.SetActive(false);
                instance.pauseMenu.enabled = true;
                PauseGame.Resume();
                if (currentTutorial == tutorialWindows[0])
                {
                    PauseGame.isPaused = true;
                    instance.pauseMenu.enabled = false;
                }
                currentTutorial = null;
            }
        }
    }
}
