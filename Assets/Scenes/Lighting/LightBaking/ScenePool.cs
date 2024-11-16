using UnityEngine;

[CreateAssetMenu(menuName = "Scene Management/Scene Pool")]
public class ScenePool : ScriptableObject
{
    public string poolName;
    public string[] sceneNames; // Add scene names manually or via script
}