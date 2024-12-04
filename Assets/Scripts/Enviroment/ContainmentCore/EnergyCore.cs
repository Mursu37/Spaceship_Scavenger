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
    private AmbientMusic ambientMusic;
    private MeltdownMusic meltdownMusic;
    private AmbienceManager ambienceManager;
    private bool heatingStarted = false;

    private CoreSounds coreSounds;

    public float heatAmount;
    public float maxHeat;
    public float heatIncreaseTime = 8f;

    [SerializeField] private ParticleSystem waveExplosion;

    public void StartHeating()
    {
        if (!heatingStarted)
        {
            heatingStarted = true;
            StartCoroutine(HeatIncrease());
        }
    }

    private void Start()
    {
        coreSounds = GetComponentInChildren<CoreSounds>();
    }

    private void Update()
    {
        if (heatAmount >= maxHeat && !hasExploded)
        {
            hasExploded = true;
            waveExplosion.Play();
            coreSounds.PlayExplosionSounds(GameObject.Find("CoreAudio"));

            ambientMusic = FindObjectOfType<AmbientMusic>();
            if (ambientMusic != null)
            {
                ambientMusic.StopAmbientMusic();
            }

            meltdownMusic = FindObjectOfType<MeltdownMusic>();
            if (meltdownMusic != null)
            {
                meltdownMusic.StopMeltdownMusic();
            }

            //Scene scene = SceneManager.GetActiveScene();
            //SceneManager.LoadScene(scene.name);
        }
    }

    public IEnumerator HeatIncrease()
    {
        while (heatAmount < maxHeat)
        {
            yield return new WaitForSeconds(heatIncreaseTime);
            heatAmount += 1f;
            //coreSounds?.PlayRandomDamageSound();
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

    public void Damage(float amount, float shakeAmount)
    {
        heatAmount += amount;
        coreSounds?.PlayRandomDamageSound();
    }

    public void Heal(float amount)
    {
        return;
    }
}
