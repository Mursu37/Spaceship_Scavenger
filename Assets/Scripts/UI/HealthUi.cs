using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUi : MonoBehaviour
{
    private PlayerHealth playerHealth;
    private TextMeshProUGUI healthText;
    private float healthCount;

    [SerializeField] private Image meter;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject healthObject;

    private void Awake()
    {
        healthText = healthObject.GetComponent<TextMeshProUGUI>();

        playerHealth = playerObject.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    private void Update()
    {
        healthCount = playerHealth.currentHealth;
        meter.fillAmount = healthCount / 100f;
        healthText.text = healthCount + "/100";

        float healthPercent = healthCount / 100f;
        if (healthPercent >= 0.5f)
        {
            meter.color = Color.Lerp(Color.yellow, Color.green, (healthPercent - 0.5f) * 2);
        }
        else
        {
            meter.color = Color.Lerp(Color.red, Color.yellow, healthPercent * 2);
        }
    }
}
