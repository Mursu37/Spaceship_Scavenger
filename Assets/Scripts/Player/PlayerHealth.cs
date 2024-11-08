using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IHealth
{
    public float currentHealth;
    [SerializeField] private float maxHealth = 5f;
    [SerializeField] private GameObject healthImage50; // for UI
    [SerializeField] private GameObject healthImage25;

    private void Awake()
    {
        currentHealth = maxHealth;
        if (healthImage25 != null && healthImage50 != null)
        {
            UpdateHealthUI(); // UI
        }
    }

    public void Damage(float amount)
    {
        currentHealth -= amount;
        Camera.main.GetComponent<CameraShake>().shakeDuration = 0.2f;
        if (healthImage25 != null && healthImage50 != null)
        {
            UpdateHealthUI();
        }
    }

    private void Update()
    {
        // Reload the scene when health goes zero or below
        if (currentHealth <= 0)
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    private void UpdateHealthUI()
    {
        // Showing the appropriate health UI images based on the current health
        if (currentHealth <= maxHealth * 0.25f)
        {
            healthImage25.SetActive(true);
            healthImage50.SetActive(false);
        }
        else if (currentHealth <= maxHealth * 0.50f)
        {
            healthImage25.SetActive(false);
            healthImage50.SetActive(true);
        }
        else
        {
            healthImage25.SetActive(false);
            healthImage50.SetActive(false);
        }
    }
}
