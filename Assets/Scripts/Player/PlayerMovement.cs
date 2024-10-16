using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Inputs
    private float verticalInput;
    private float horizontalInput;
    private float rollInput;
    private float ascendInput;
    private float momentumInput;
    private float mouseInputX;
    private float mouseInputY;

    private Rigidbody rb;

    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float rollAcceleration;

    public float maxSpeed;
    public float mouseSensitivity;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
    }

    private void HandleMovement()
    {
        rb.AddForce(rb.transform.TransformDirection(Vector3.forward) * verticalInput * acceleration, ForceMode.VelocityChange);
        rb.AddForce(rb.transform.TransformDirection(Vector3.right) * horizontalInput * acceleration, ForceMode.VelocityChange);

        rb.AddTorque(rb.transform.right * mouseSensitivity * mouseInputY * -1, ForceMode.VelocityChange);
        rb.AddTorque(rb.transform.up * mouseSensitivity * mouseInputX, ForceMode.VelocityChange);

        // Rotates player
        rb.AddTorque(rb.transform.forward * rollAcceleration * rollInput, ForceMode.VelocityChange);

        // Ascends / descends player
        rb.AddForce(rb.transform.TransformDirection(Vector3.up) * ascendInput * acceleration, ForceMode.VelocityChange);

        // Makes player lose all momentum
        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, acceleration * momentumInput);

        

        //Check if the player's speed exceeds the maxium speed
        if (rb.velocity.magnitude > maxSpeed && !GameObject.Find("Multitool").GetComponent<GravityGun>().isGrabbling)
        {
            // Define the range between maxSpeed and the desired upper limit
            float speedRatio = Mathf.InverseLerp(maxSpeed, 10f, rb.velocity.magnitude);
            // Adjust the Lerp factor based on the speed ratio.
            float dampingFactor = Mathf.Lerp(0.02f, 0.01f, speedRatio);
            // Apply stronger damping the closer the velocity is to the max speed
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, dampingFactor);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Get the inputs
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        ascendInput = Input.GetAxis("Ascend");
        rollInput = Input.GetAxis("Roll");
        momentumInput = Input.GetAxis("Lose Momentum");
        mouseInputX = Input.GetAxis("Mouse X");
        mouseInputY = Input.GetAxis("Mouse Y");

        // Gets the current speed
        speed = rb.velocity.magnitude;
        rotationSpeed = rb.angularVelocity.magnitude;
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }
}