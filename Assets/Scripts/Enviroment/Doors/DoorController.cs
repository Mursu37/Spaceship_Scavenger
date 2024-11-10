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
        FindCuttableObjects(transform);
    }

    public void HackOpen()
    {
        if (animator != null)
        {
            animator.Play("DoorOpening");
        }
    }

    private void FindCuttableObjects(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);

            if (child.CompareTag("Cuttable"))
            {
                cuttingPoints.Add(child.gameObject);

            }

            FindCuttableObjects(child);
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
