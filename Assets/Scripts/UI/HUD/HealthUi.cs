using UnityEngine;
using UnityEngine.UI;

public class HealthUi : MonoBehaviour
{
    private PlayerHealth playerHealth;
    private float healthCount = 100f; // Assuming max health is 100

    [SerializeField] private Image currentHealthMeter;
    [SerializeField] private Image delayedHealthMeter;
    [SerializeField] private float lerpSpeed = 5f;

    private void OnEnable()
    {
        PlayerHealth.OnHealthChanged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(float newHealth)
    {
        healthCount = newHealth;
    }

    private void Update()
    {
        float targetFillAmount = healthCount / 100f;

        currentHealthMeter.fillAmount = Mathf.Lerp(
            currentHealthMeter.fillAmount,
            targetFillAmount,
            Time.deltaTime * lerpSpeed
        );

        if (delayedHealthMeter.fillAmount > currentHealthMeter.fillAmount)
        {
            delayedHealthMeter.fillAmount = Mathf.Lerp(
                delayedHealthMeter.fillAmount,
                currentHealthMeter.fillAmount,
                Time.deltaTime * lerpSpeed
            );
        }
        else
        {
            delayedHealthMeter.fillAmount = currentHealthMeter.fillAmount;
        }
    }
}

