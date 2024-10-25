using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Explosives : MonoBehaviour
{
    public int damage = 20;
    public float explodeDelay = 2f;
    public float explodeRadius = 15f;
    public float explosionForce = 15f;
    public GameObject explodeEffect;

    private float collDelay = 0.2f;

    void Start()
    {
        Invoke("EnableCollider", collDelay);
        Invoke("Explode", explodeDelay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void EnableCollider()
    {
        GetComponent<SphereCollider>().isTrigger = false;
    }

    private void Explode()
    {
        Collider[] colliderHit = Physics.OverlapSphere(transform.position, explodeRadius);

        foreach (Collider collider in colliderHit)
        { 
            Rigidbody rigid = collider.GetComponent<Rigidbody>();
            if (rigid != null) rigid.AddExplosionForce(explosionForce, transform.position, explodeRadius, 1f, ForceMode.Impulse);
         
            PlayerHealth player = collider.GetComponent<PlayerHealth>();
            if (player != null) player.Damage(damage);

            Instantiate(explodeEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
