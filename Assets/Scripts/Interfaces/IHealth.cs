using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    private float maxHealth => maxHealth;
    public float currentHealth => currentHealth;

    void Damage(float amount);
    void Heal(float amount);
}
