using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth
{
    private float previousHealth;

    public float currentHealth;
    [SerializeField] private float maxHealth = 100f;

    public static event Action<float, float> OnHealthChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
        previousHealth = currentHealth;
    }

    public void Damage(float amount, float shakeAmount = 0.04f)
    {
        currentHealth -= amount;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

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
        GameManager.instance.TriggerGameOver();
    }
}
