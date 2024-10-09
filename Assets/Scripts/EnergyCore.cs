using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnergyCore : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 relativeVelocity;
    private float collisionForce;

    [SerializeField] private float heath;
    [SerializeField] private float maxHeath;
    [SerializeField] private GameObject gameState;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (heath >= maxHeath)
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    public IEnumerator HeathIncrease()
    {
        while (heath < maxHeath)
        {
            yield return new WaitForSeconds(2f);
            heath += 1f;
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
            heath += collisionForce * 2f;
        }
    }
}
