using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnergyCore : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 relativeVelocity;
    private float collisionForce;

    [SerializeField] private float heatAmount;
    [SerializeField] private float maxHeat;
    [SerializeField] private GameObject gameState;
    [SerializeField] private Image meter;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        meter.fillAmount = heatAmount / maxHeat;

        float healthPercent = heatAmount / maxHeat;
        if (healthPercent <= 0.5f)
        {
            meter.color = Color.Lerp(Color.green, Color.yellow, healthPercent * 2);
        }
        else
        {
            meter.color = Color.Lerp(Color.yellow, Color.red, (healthPercent - 0.5f) * 2);
        }

        if (heatAmount >= maxHeat)
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    public IEnumerator HeatIncrease()
    {
        while (heatAmount < maxHeat)
        {
            yield return new WaitForSeconds(8f);
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
        }
    }
}
