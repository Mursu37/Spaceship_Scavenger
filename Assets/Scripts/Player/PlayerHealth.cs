using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth
{
    private float previousHealth;

    public float currentHealth;
    [SerializeField] private float maxHealth = 5f;

    public static event Action<float> OnHealthChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
        previousHealth = currentHealth;
    }

    public void Damage(float amount, float shakeAmount = 0.04f)
    {
        currentHealth -= amount;
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }

        if (Camera.main.TryGetComponent<CameraShake>(out CameraShake cameraShake))
        {
            cameraShake.shakeDuration = 0.2f;
            cameraShake.shakeAmount = shakeAmount;
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

    private void Die()
    {
        GameManager.instance.UpdateGameState(GameState.GameOver);
    }

    private void Update()
    {        
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
            VisorChange.UpdateVisor(VisorChange.Visor.BadlyDamaged);
        }
        else if (currentHealth <= maxHealth * 0.50f)
        {
            VisorChange.UpdateVisor(VisorChange.Visor.MildlyDamaged);
        }
        else
        {
            VisorChange.UpdateVisor(VisorChange.Visor.Default);
        }
    }
}
