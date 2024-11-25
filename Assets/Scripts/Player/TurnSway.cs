using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSway : MonoBehaviour
{
    private float mouseX;
    private float mouseY;

    [SerializeField] private float smooth;
    [SerializeField] private float swayMultiplier;
    [SerializeField] private float maxSwayAngleX;
    [SerializeField] private float maxSwayAngleY;

    private void Update()
    {
        mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
        mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;

        mouseX = Mathf.Clamp(mouseX, -maxSwayAngleX, maxSwayAngleX);
        mouseY = Mathf.Clamp(mouseY, -maxSwayAngleY, maxSwayAngleY);

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
    }
}
