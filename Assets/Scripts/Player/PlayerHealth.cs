using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IHealth
{
    public float currentHealth;
    [SerializeField] private float maxHealth = 5f;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void Damage(float amount)
    {
        currentHealth -= amount;
        Camera.main.GetComponent<CameraShake>().shakeDuration = 0.2f;
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
}
