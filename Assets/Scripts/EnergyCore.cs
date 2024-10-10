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

    [SerializeField] private float heathAmount;
    [SerializeField] private float maxHeath;
    [SerializeField] private GameObject gameState;
    [SerializeField] private Image meter;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        meter.fillAmount = heathAmount / maxHeath;

        float healthPercent = heathAmount / maxHeath;
        if (healthPercent <= 0.5f)
        {
            meter.color = Color.Lerp(Color.green, Color.yellow, healthPercent * 2);
        }
        else
        {
            meter.color = Color.Lerp(Color.yellow, Color.red, (healthPercent - 0.5f) * 2);
        }

        if (heathAmount >= maxHeath)
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    public IEnumerator HeathIncrease()
    {
        while (heathAmount < maxHeath)
        {
            yield return new WaitForSeconds(3f);
            heathAmount += 1f;
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
            heathAmount += collisionForce * 2f;
        }
    }
}
