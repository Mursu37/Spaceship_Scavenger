using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 inputKey;
    private Rigidbody rb;
    private Transform cameraTransform;

    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float rotatieionAcceleration;
    [SerializeField] private float maxRotationSpeed;

    // Start is called before the first frame update
    private void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void MovePlayer()
    {
        // Get input from keyboard
        inputKey = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // Get camera forward and right directions
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calculate movement direction relative to camera
        Vector3 moveDirection = (cameraForward * inputKey.z + cameraRight * inputKey.x).normalized;

        // Ascends the player
        if (Input.GetKey(KeyCode.Space))
        {
            moveDirection += cameraTransform.up;
        }

        // Descends the player
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveDirection -= cameraTransform.up;
        }

        // Apply force in the direction of movement
        rb.AddForce(moveDirection * acceleration * Time.fixedDeltaTime);

        // Check if the player's speed exceeds maxSpeed
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    private void RotatePlayer()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            rb.AddTorque(transform.forward * rotatieionAcceleration * Time.fixedDeltaTime);

            if (rb.angularVelocity.magnitude > maxRotationSpeed)
            {
                rb.angularVelocity = rb.angularVelocity.normalized * maxRotationSpeed;
            }
        }

        if (Input.GetKey(KeyCode.E))
        {
            rb.AddTorque(transform.forward * rotatieionAcceleration * Time.fixedDeltaTime * -1f);

            if (rb.angularVelocity.magnitude > maxRotationSpeed)
            {
                rb.angularVelocity = rb.angularVelocity.normalized * maxRotationSpeed;
            }
        }
    }

    private void StopMomentum()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.fixedDeltaTime * 2f);

            rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, Time.fixedDeltaTime * 2f);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        // Gets the current speed
        speed = rb.velocity.magnitude;
        rotationSpeed = rb.angularVelocity.magnitude;

        MovePlayer();
        RotatePlayer();
        StopMomentum();
    }
}