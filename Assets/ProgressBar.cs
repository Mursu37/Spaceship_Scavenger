using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image bar; // Progress bar GameObject
    [SerializeField] private TMP_Text stageText; // Text to display the current stage
    [SerializeField] private int time = 60; // Time for the bar to fill (s)

    private float heatLevel;

    private void Start()
    {
        heatLevel = 0f; // Initialize heat level
        UpdateStageText(); // Update the stage text on start
        StartCoroutine(RunTimer()); // Start the progress bar timer
    }

    private IEnumerator RunTimer()
    {
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            float progress = elapsedTime / time;
            heatLevel = Mathf.Lerp(0, 100, progress);

            if (bar != null && bar.type == Image.Type.Filled)
            {
                bar.fillAmount = progress; // Set the fill amount directly
            }

            UpdateStageText(); // Update the stage text based on the current heat level

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        heatLevel = 100f; // Ensure heat level is maxed out at the end
        if (bar != null)
        {
            bar.fillAmount = 1f; // Ensure the fill amount is maxed out
        }
        UpdateStageText(); // Update stage text one last time
    }

    private void UpdateStageText()
    {
        if (stageText != null)
        {
            if (heatLevel <= 30)
            {
                stageText.text = "[ STAGE 1 ]";
            }
            else if (heatLevel > 30 && heatLevel <= 60)
            {
                stageText.text = "[ STAGE 2 ]";
            }
            else
            {
                stageText.text = "[ STAGE 3 ]";
            }
        }
    }

    public void SetHeatLevel(float newHeatLevel)
    {
        heatLevel = newHeatLevel; // Update the heat level
        UpdateStageText(); // Update the stage text based on the new heat level
    }
}
