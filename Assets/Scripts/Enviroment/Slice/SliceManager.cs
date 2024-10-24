using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SliceManager : MonoBehaviour
{
    // List to store all the objects with the "Slice" component
    [HideInInspector] public List<Slice> sliceObjects;

    public bool isUpdating = false;

    private void Start()
    {
        UpdateList();
    }

    public void UpdateList()
    {
        isUpdating = true;

        // Find all objects in the scene that have the "Slice" component
        Slice[] slices = FindObjectsOfType<Slice>();

        // Convert the array to a List (optional, but gives more flexibility)
        sliceObjects = new List<Slice>(slices);

        // Example: Print the names of the objects with the Slice component
        foreach (Slice slice in sliceObjects)
        {
            slice.AddComponent<SlicedFragment>();
            slice.GetComponent<SlicedFragment>().canCollide = true;
        }
        
        isUpdating = false;
    }
}
