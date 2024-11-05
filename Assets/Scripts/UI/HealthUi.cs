using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUi : MonoBehaviour
{
    private PlayerHealth playerHealth;
    private float healthCount;

    [SerializeField] private Image meter;
    [SerializeField] private GameObject playerObject;

    private void Awake()
    {
        playerHealth = playerObject.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    private void Update()
    {
        healthCount = playerHealth.currentHealth;
        meter.fillAmount = healthCount / 100f;
    }
}
