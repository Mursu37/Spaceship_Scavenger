using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    private float maxHealth => maxHealth;
    private float currentHealth => currentHealth;

    void Damage(float amount);
}
