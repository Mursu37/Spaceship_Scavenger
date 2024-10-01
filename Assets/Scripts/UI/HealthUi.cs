using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthUi : MonoBehaviour
{
    private TextMeshProUGUI healthText;
    private PlayerHealth playerHealth;
    private float healthCount;

    [SerializeField] private GameObject playerObject;

    private void Awake()
    {
        healthText = GetComponent<TextMeshProUGUI>();

        playerHealth = playerObject.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    private void Update()
    {
        healthCount = playerHealth.currentHealth;
        healthText.text = "Health: " + healthCount;
    }
}
