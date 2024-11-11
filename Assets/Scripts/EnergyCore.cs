using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnergyCore : MonoBehaviour, IHealth
{
    private Vector3 relativeVelocity;
    private float collisionForce;

    private CoreSounds coreSounds;

    public float heatAmount;
    public float maxHeat;

    private void OnEnable()
    {
        StartCoroutine(HeatIncrease());
    }

    private void Start()
    {
        coreSounds = GetComponentInChildren<CoreSounds>();
        gameObject.SetActive(false);
    }

    public IEnumerator HeatIncrease()
    {
        while (heatAmount < maxHeat)
        {
            yield return new WaitForSeconds(8f);
            heatAmount += 1f;
            coreSounds?.PlayRandomDamageSound();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Get the relative velocity between your object and the other object
        relativeVelocity = collision.relativeVelocity;

        // Calculate the force of impact
        collisionForce = relativeVelocity.magnitude;

        if (collisionForce > 2)
        {
            heatAmount += collisionForce * 2f;
            coreSounds?.PlayRandomDamageSound();
        }
    }

    public void Damage(float amount)
    {
        heatAmount += amount;
        coreSounds?.PlayRandomDamageSound();
    }

    public void Heal(float amount)
    {
        return;
    }
}
