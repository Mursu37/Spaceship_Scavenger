using System.Collections;
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
    private bool hasColorChanged = false; // To track if the color has already changed
    private bool hasBarReachedFull = false; // Tracks if the bar has already reached full

    [SerializeField] private Color fullDelayedBarColor;
    [SerializeField] private float fullBarTransitionDuration = 0.5f;
    private bool isColorChanging = false; // To prevent overlapping color transitions
    private Coroutine colorTransitionCoroutine; // Reference to the color transition coroutine


    [SerializeField] private Color waveColor;
    [SerializeField] private float waveDuration = 0.5f;
    [SerializeField] private float waveThreshold = 0.5f;

    private bool isWaveActive = false;
    private Coroutine waveCoroutine;

    private bool update = true;
    private float delayTimer = 0f; // Timer for the delay
    public float delayDuration = 0.5f; // Duration of the delay in seconds
    private float lastTargetFillAmount = 0f; // Tracks the last target fill amount


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
        if (core != null && update)
        {
            float targetFillAmount = core.heatAmount / core.maxHeat;

            // Update the main bar
            bar.fillAmount = Mathf.Lerp(bar.fillAmount, targetFillAmount, Time.deltaTime * lerpSpeed);

            if (delayedBar != null)
            {
                if (Mathf.Abs(targetFillAmount - lastTargetFillAmount) > waveThreshold)
                {
                    delayTimer = 0f; // Reset the delay timer
                    lastTargetFillAmount = targetFillAmount; // Update the last target

                    // Trigger the wave effect
                    if (!isWaveActive && !hasBarReachedFull)
                    {
                        if (waveCoroutine != null) StopCoroutine(waveCoroutine); // Stop any existing wave
                        waveCoroutine = StartCoroutine(PlayColorWave());
                    }
                }


                // Check if the target fill amount has significantly changed
                if (Mathf.Abs(targetFillAmount - lastTargetFillAmount) > 0.01f) // Small threshold to avoid noise
                {
                    delayTimer = 0f; // Reset the delay timer
                    lastTargetFillAmount = targetFillAmount; // Update the last target
                }
                else
                {
                    delayTimer += Time.deltaTime; // Increment the delay timer
                }

                // Only move the delayed bar if the delay duration has passed
                if (delayTimer >= delayDuration)
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
                            bar.fillAmount - maxLagDistance,
                            Time.deltaTime * delayedBarLerpSpeed
                        );
                    }
                }

                // Trigger color change if delayed bar is full
                if (!hasBarReachedFull && Mathf.Approximately(bar.fillAmount, 1.0f))
                {
                    hasBarReachedFull = true; // Mark the bar as full

                    // If the color has not been changed yet
                    if (!hasColorChanged)
                    {
                        hasColorChanged = true; // Mark that the color has been changed
                        if (colorTransitionCoroutine != null) StopCoroutine(colorTransitionCoroutine);
                        colorTransitionCoroutine = StartCoroutine(GradualColorChange());
                    }
                }

            }

            UpdateStageText();
        }
    }

    private IEnumerator GradualColorChange()
    {
        isColorChanging = true;

        Image delayedBarImage = delayedBar.GetComponent<Image>(); // Assuming delayedBar has an Image component
        if (delayedBarImage == null)
        {
            isColorChanging = false;
            yield break; // Exit if no Image component
        }

        Color originalColor = delayedBarImage.color; // Store the original color
        float elapsedTime = 0f;

        while (elapsedTime < fullBarTransitionDuration)
        {
            float t = elapsedTime / fullBarTransitionDuration;
            delayedBarImage.color = Color.Lerp(originalColor, fullDelayedBarColor, t); // Smoothly transition to the target color

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        delayedBarImage.color = fullDelayedBarColor; // Ensure it ends on the target color
        isColorChanging = false; // Reset the flag
    }

    private IEnumerator PlayColorWave()
    {
        isWaveActive = true;
        float elapsedTime = 0f;

        Image delayedBarImage = delayedBar.GetComponent<Image>(); // Assuming it's an Image
        if (delayedBarImage == null) yield break; // Exit if no Image component

        Color originalColor = delayedBarImage.color;

        while (elapsedTime < waveDuration)
        {
            float t = elapsedTime / waveDuration;
            delayedBarImage.color = Color.Lerp(waveColor, originalColor, t); // Interpolate from waveColor to originalColor

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Restore the original color
        delayedBarImage.color = originalColor;
        isWaveActive = false;
    }

    private IEnumerator BarFillCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);


        yield return new WaitForEndOfFrame();
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
