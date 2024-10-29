using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator animator;
    private List<GameObject> cuttingPoints = new List<GameObject>();

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("Cuttable"))
            {
                cuttingPoints.Add(transform.GetChild(i).gameObject);
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
