using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private EnergyCore core;
    [SerializeField] private Image bar; // Progress bar GameObject
    [SerializeField] private TMP_Text stageText; // Text to display the current stage
    [SerializeField] private GameObject coreObject;

    private void Start()
    {
        UpdateStageText(); // Update the stage text on start

        if (coreObject != null )
        {
            core = coreObject.GetComponent<EnergyCore>();
        }
    }

    private void Update()
    {
        if (core != null)
        {
            bar.fillAmount = core.heatAmount / core.maxHeat;
            UpdateStageText();
        }
    }

    private void UpdateStageText()
    {
        if (core == null)
            return;

        float heatPercent = core.heatAmount / core.maxHeat;
        if (stageText != null)
        {
            if (heatPercent <= 0.33f)
            {
                stageText.text = "[ STAGE 1 ]";
            }
            else if (heatPercent > 0.33f && heatPercent <= 0.66f)
            {
                stageText.text = "[ STAGE 2 ]";
            }
            else
            {
                stageText.text = "[ STAGE 3 ]";
            }
        }
    }
}
