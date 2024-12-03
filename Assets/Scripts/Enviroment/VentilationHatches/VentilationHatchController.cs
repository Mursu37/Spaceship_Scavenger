using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentilationHatchController : CuttingPointManager
{
    private Animator animator;
    [SerializeField]
    private float ForceOnRelease = 0.01f;

    public int id;
    public bool doorOpened = false;
    private Rigidbody rbody;

    // Start is called before the first frame update
    private void Start()
    {
        FindCuttableObjects(transform);

        if (CheckpointManager.doorsOpened.Contains(id))
        {
            doorOpened = true;
            DestroyCuttingPoints();
        }
    }

    private void OpenHatch()
    {
        if (AreCuttingPointsNull() && !doorOpened)
        {
            rbody = gameObject.AddComponent<Rigidbody>();
            rbody.mass = 1.5f;
            rbody.drag = 0;
            rbody.angularDrag = 0.05f;
            rbody.isKinematic = false;
            rbody.useGravity = false;

            doorOpened = true;
            rbody.AddRelativeForce(new Vector3(ForceOnRelease, 0f, 0f), ForceMode.Impulse);
        }
    }

    private void Update()
    {
        OpenHatch();
    }
}
