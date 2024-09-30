using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowObjectSpeed : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float speed;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        speed = rb.velocity.magnitude;
    }
}
