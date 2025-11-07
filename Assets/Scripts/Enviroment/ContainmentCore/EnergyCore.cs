using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnergyCore : MonoBehaviour, IHealth
{
    private Vector3 relativeVelocity;
    private float collisionForce;
    private bool hasExploded = false;
    private bool heatingStarted = false;


    public float heatAmount;
    public float maxHeat;
    public float heatIncreaseTime = 8f;

    [SerializeField] private ParticleSystem waveExplosion;
    [SerializeField] private CoreDamageIcons damageIcons;

    private void OnEnable()
    {
        GameManager.OnPhaseChanged += OnGameChanged;
    }

    private void OnDisable()
    {
        GameManager.OnPhaseChanged -= OnGameChanged;
    }

    private void OnGameChanged(Phase newPhase)
    {
        if (newPhase == Phase.Meltdown)
        {
            StartHeating();
        }
    }

    public void StartHeating()
    {
        if (!heatingStarted)
        {
            heatingStarted = true;
            StartCoroutine(HeatIncrease());
        }
    }

    private void Update()
    {
        if (heatAmount >= maxHeat && !hasExploded)
        {
            hasExploded = true;
            waveExplosion.Play();
        }
    }

    public IEnumerator HeatIncrease()
    {
        while (heatAmount < maxHeat)
        {
            yield return new WaitForSeconds(heatIncreaseTime);
            heatAmount += 1f;
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
            damageIcons.ShowIcons();
        }
    }

    public void Damage(float amount, float shakeAmount)
    {
        heatAmount += amount;
    }

    public void Heal(float amount)
    {
        return;
    }
}
