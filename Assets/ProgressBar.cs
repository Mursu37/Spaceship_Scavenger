using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private EnergyCore core;

    [SerializeField] private Image bar; // Current meltdown bar
    [SerializeField] private Image delayedBar; // Delayed meltdown bar
    [SerializeField] private TMP_Text stageText; // Text to display the current stage
    [SerializeField] private GameObject coreObject;
    [SerializeField] private float lerpSpeed = 0.02f; 
    [SerializeField] private float delayInterval = 1.0f; 

    private float delayTimer;

    private void Start()
    {
        UpdateStageText();

        if (coreObject != null)
        {
            core = coreObject.GetComponent<EnergyCore>();
        }
    }

    private void Update()
    {
        if (core != null)
        {
            float fillAmount = core.heatAmount / core.maxHeat;

            bar.fillAmount = fillAmount;

            if (delayedBar != null)
            {
                delayTimer += Time.deltaTime;
                if (delayTimer >= delayInterval)
                {
                    delayTimer = 0f;

                    delayedBar.fillAmount = Mathf.Lerp(
                        delayedBar.fillAmount,
                        bar.fillAmount,
                        lerpSpeed
                    );
                }

                if (delayedBar.fillAmount > bar.fillAmount)
                {
                    delayedBar.fillAmount = bar.fillAmount;
                }
            }

            UpdateStageText();
        }
    }

    private void UpdateStageText()
    {
        if (core == null) return;

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
