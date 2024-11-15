using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IHealth
{
    private FadeIn fadeIn;
    private AmbientMusic ambientMusic;
    private float previousHealth;
    private bool hasDied = false;
    private bool hasFadeIn = false;

    public float currentHealth;
    [SerializeField] private float maxHealth = 5f;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject visor;
    [SerializeField] private Sprite visorImage100;
    [SerializeField] private Sprite visorImage50;
    [SerializeField] private Sprite visorImage25;

    private void Awake()
    {
        currentHealth = maxHealth;
        previousHealth = currentHealth;

        if (visorImage25 != null && visorImage50 != null)
        {
            UpdateHealthUI(); // UI
        }

        fadeIn = gameOver.GetComponent<FadeIn>();
    }

    public void Damage(float amount, float shakeAmount = 0.04f)
    {
        currentHealth -= amount;
        Camera.main.GetComponent<CameraShake>().shakeDuration = 0.2f;
        Camera.main.GetComponent<CameraShake>().shakeAmount = shakeAmount;
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

    private void Update()
    {
        // Reload the scene when health goes zero or below
        if (currentHealth <= 0)
        {
            if (!hasDied)
            {
                AudioManager.PlayAudio("GameOverSound", 1, 1, false, null, true);
                AudioListener.pause = true;
                PauseGame.isPaused = true;
                GetComponent<PlayerMovement>().enabled = false;
                gameOver.SetActive(true);
                fadeIn.StartFadeIn();

                ambientMusic = FindObjectOfType<AmbientMusic>();
                if (ambientMusic != null)
                {
                    ambientMusic.StopAmbientMusic();
                }

                hasDied = true;
            }

            if (fadeIn.allFadedIn && !hasFadeIn)
            {
                PauseGame.Pause();
                hasFadeIn = true;
            }
        }
        
        if (previousHealth != currentHealth)
        {
            UpdateHealthUI();
            previousHealth = currentHealth;
        }
    }

    private void UpdateHealthUI()
    {
        // Showing the appropriate health UI images based on the current health
        if (currentHealth <= maxHealth * 0.25f)
        {
            visor.GetComponent<Image>().sprite = visorImage25;
        }
        else if (currentHealth <= maxHealth * 0.50f)
        {
            visor.GetComponent<Image>().sprite = visorImage50;
        }
        else
        {
            visor.GetComponent<Image>().sprite = visorImage100;
        }
    }
}
