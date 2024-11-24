using UnityEngine;
using UnityEngine.UI;

public class HealthUi : MonoBehaviour
{
    private PlayerHealth playerHealth;
    private float healthCount;

    [SerializeField] private Image currentHealthMeter;
    [SerializeField] private Image delayedHealthMeter;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private float lerpSpeed = 5f; 

    private void Awake()
    {
        if (playerObject != null)
        {
            playerHealth = playerObject.GetComponent<PlayerHealth>();
        }
    }

    private void Update()
    {
        if (playerHealth == null) return;

        healthCount = playerHealth.currentHealth;
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

