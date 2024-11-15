using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    private float maxHealth => maxHealth;
    public float currentHealth => currentHealth;

    void Damage(float amount, float shakeAmount = 0.04f);
    void Heal(float amount);
}
