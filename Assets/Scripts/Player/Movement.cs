using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody rb;
    private float verticalMove;
    private float horizontalMove;
    private float rollInput;
    private float ascendInput;
    private float momentumInput;
    private float mouseInputX;
    private float mouseInputY;

    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float angleAcceleration;
    [SerializeField] private float rollAcceleration;
    [SerializeField] private float maxSpeed;
    //[SerializeField] private float maxRollSpeed;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    private void Update()
    {
        verticalMove = Input.GetAxis("Vertical");
        horizontalMove = Input.GetAxis("Horizontal");
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
        rb.AddForce(rb.transform.TransformDirection(Vector3.forward) * verticalMove * acceleration * Time.fixedDeltaTime);
        rb.AddForce(rb.transform.TransformDirection(Vector3.right) * horizontalMove * acceleration * Time.fixedDeltaTime);

        rb.AddTorque(rb.transform.right * angleAcceleration * mouseInputY * -1 * Time.fixedDeltaTime, ForceMode.VelocityChange);
        rb.AddTorque(rb.transform.up * angleAcceleration * mouseInputX * Time.fixedDeltaTime, ForceMode.VelocityChange);

        // Rotates player
        rb.AddTorque(rb.transform.forward * rollAcceleration * rollInput * Time.fixedDeltaTime);

        // Ascends / descends player
        rb.AddForce(rb.transform.TransformDirection(Vector3.up) * ascendInput * acceleration * Time.fixedDeltaTime);

        // Makes player lose all momentum
        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.fixedDeltaTime * 2f * momentumInput);

        // Check if the player's speed exceeds the maxium speed
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        // TODO: Add max speed for roll without affecting the mouse inputs
    }
}
