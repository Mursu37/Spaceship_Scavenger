using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private Transform playerTransform;

    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    private void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate camera
        playerTransform.Rotate(Vector3.up * mouseX);
        playerTransform.Rotate(Vector3.left * mouseY);

        // Keep the z-axis at 0 by resetting z-rotation
        Vector3 rotation = playerTransform.localEulerAngles;
        rotation.z = 0f; // Reset z-axis rotation to 0
        playerTransform.localEulerAngles = rotation;
    }
}
