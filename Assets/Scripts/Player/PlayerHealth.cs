using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth
{
    private float currentHealth;
    private float maxHealth = 5f;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void Damage(float amount)
    {
        currentHealth -= amount;
    }
}
