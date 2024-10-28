using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator animator;
    private GameObject[] cuttingPoints;

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        cuttingPoints = new GameObject[3];

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("Cuttable"))
            {
                cuttingPoints[i] = transform.GetChild(i).gameObject;
            }
        }
    }

    private void Update()
    {
        if (AreCuttingPointsNull())
        {
            if (animator != null)
            {
                animator.Play("DoorOpening");
            }
        }
    }

    private bool AreCuttingPointsNull()
    {
        foreach (GameObject cuttingPoint in cuttingPoints)
        {
            if (cuttingPoint != null)
            {
                return false;
            }
        }
        return true;
    }
}
