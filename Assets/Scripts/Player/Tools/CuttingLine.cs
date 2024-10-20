using System.Collections.Generic;
using UnityEngine;

public class CuttingLine : MonoBehaviour
{
    // List to track objects currently inside the trigger
    private List<Collider> collidersInsideTrigger = new List<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        // Add the object to the list of colliders inside the trigger
        if (!collidersInsideTrigger.Contains(other))
        {
            collidersInsideTrigger.Add(other);
        }

        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        // Remove the object from the list when it exits the trigger
        if (collidersInsideTrigger.Contains(other))
        {
            collidersInsideTrigger.Remove(other);
        }
    }

    private void Update()
    {
        // Clean up disabled objects from the list
        collidersInsideTrigger.RemoveAll(collider => collider == null || !collider.gameObject.activeInHierarchy);

        // Disable the child object if no active colliders are left
        if (collidersInsideTrigger.Count == 0)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        // Clear the list when this object is disabled
        collidersInsideTrigger.Clear();
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
