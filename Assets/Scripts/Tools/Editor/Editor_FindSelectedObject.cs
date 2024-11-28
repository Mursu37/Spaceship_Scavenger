using UnityEditor;
using UnityEngine;

public class HighlightSelectedObject : MonoBehaviour
{
    [MenuItem("Tools/Highlight Selected Object")]
    private static void HighlightSelectedObjectInHierarchy()
    {
        // Get the currently selected objects in the Unity Editor
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            Debug.Log("No objects selected in the Hierarchy.");
            return;
        }

        foreach (GameObject obj in selectedObjects)
        {
            Debug.Log($"Highlighting Object: {obj.name}, Path: {GetFullPath(obj)}");

            // Highlight the object in the Hierarchy
            EditorGUIUtility.PingObject(obj);
        }
    }

    // Helper method to get the full hierarchy path of the object
    private static string GetFullPath(GameObject obj)
    {
        string path = obj.name;
        Transform current = obj.transform.parent;

        while (current != null)
        {
            path = current.name + "/" + path;
            current = current.parent;
        }

        return path;
    }
}