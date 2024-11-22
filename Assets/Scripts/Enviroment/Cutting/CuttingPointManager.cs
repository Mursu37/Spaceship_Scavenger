using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CuttingPointManager : MonoBehaviour
{
    private List<GameObject> cuttingPoints = new List<GameObject>();

    protected void FindCuttableObjects(Transform parent)
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

    protected bool AreCuttingPointsNull()
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

    protected void DestroyCuttableObjects(Transform parent)
    {
        if (cuttingPoints.Count > 0 && cuttingPoints != null)
        {
            foreach (GameObject cuttingPointToDestroy in cuttingPoints)
            {
                Destroy(cuttingPointToDestroy);
            }
        }
    }
}
