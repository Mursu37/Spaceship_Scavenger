using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTest : MonoBehaviour 
{
    public float Radius;
    public float Force;

    public bool Active;
    public GameObject ExplosionEffect;

    private void Update()
    {
        if (Active)
        {
            ExplodeWithDelay();
        }
    }

    public void ExplodeWithDelay() {

        if (_explosionDone) return;
        _explosionDone = true;
        Invoke("Explode", 0.2f);
        GetComponent<Renderer>().material.color = Color.red;
    }

    private bool _explosionDone;


    /*private void Start()
    {
        Explode();
    }
    */

    // Start is called before the first frame update
    public void Explode()
    {

        if (_explosionDone) return;
        _explosionDone = true;
        Collider[] overlappedColliders = Physics.OverlapSphere(transform.position, Radius);
        for (int i = 0; i < overlappedColliders.Length; i++)
        {
            Rigidbody rigidbody = overlappedColliders[i].attachedRigidbody;
            if (rigidbody)
            {
                rigidbody.AddExplosionForce(Force, transform.position, Radius, 1f);

                ExplosionTest explosion = rigidbody.GetComponent<ExplosionTest>();
                if (explosion != null) {

                    if (Vector3.Distance(transform.position, rigidbody.position) < Radius / 2f) {
                    
                        explosion.ExplodeWithDelay();
                    }
                }
            }
        }

        Destroy(gameObject);
        Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
    }
}
