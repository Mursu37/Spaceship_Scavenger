using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficlultySelection : MonoBehaviour
{
    private FadeIn fadeIn;

    [SerializeField] private GameObject blackPanel;
    [SerializeField] private GameObject mainMenu;

    private void Start()
    {
        fadeIn = blackPanel.GetComponent<FadeIn>();
    }

    public void SelectEasy()
    {
        DifficultyManager.easyLevelSelected = true;
        blackPanel.SetActive(true);
        fadeIn.StartFadeIn();
    }

    public void SelectDifficult()
    {
        DifficultyManager.difficultLevelSelected = true;
        blackPanel.SetActive(true);
        fadeIn.StartFadeIn();
    }

    public void Back()
    {
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (fadeIn.allFadedIn)
        {
            SceneManager.LoadSceneAsync("IntroCutscene");
        }
    }
}
