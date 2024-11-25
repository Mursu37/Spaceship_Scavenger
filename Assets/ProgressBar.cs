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
    [SerializeField] private float lerpSpeed = 5.0f; // Speed for main bar
    [SerializeField] private float delayedBarLerpSpeed = 2.0f; // Smoother speed for delayed bar
    [SerializeField] private float maxLagDistance = 0.1f; // Maximum allowed difference between bars

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
            float targetFillAmount = core.heatAmount / core.maxHeat;

            bar.fillAmount = Mathf.Lerp(bar.fillAmount, targetFillAmount, Time.deltaTime * lerpSpeed);

            if (delayedBar != null)
            {
                delayedBar.fillAmount = Mathf.Lerp(
                    delayedBar.fillAmount,
                    bar.fillAmount,
                    Time.deltaTime * delayedBarLerpSpeed
                );

                if (delayedBar.fillAmount > bar.fillAmount + maxLagDistance)
                {
                    delayedBar.fillAmount = Mathf.Lerp(
                        delayedBar.fillAmount,
                        bar.fillAmount + maxLagDistance,
                        Time.deltaTime * delayedBarLerpSpeed
                    );
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
