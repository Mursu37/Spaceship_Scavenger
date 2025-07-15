using UnityEngine;
using UnityEditor;

public class AudioComponentRemover : Editor
{
    [MenuItem("Tools/Remove All Audio Components in Selected Hierarchy")]
    private static void RemoveAudioComponents()
    {
        GameObject selected = Selection.activeGameObject;

        if (selected == null)
        {
            Debug.LogWarning("No GameObject selected. Please select a GameObject in the hierarchy.");
            return;
        }

        int sourcesRemoved = 0;
        int listenersRemoved = 0;

        // Remove AudioSources
        AudioSource[] sources = selected.GetComponentsInChildren<AudioSource>(true);
        foreach (AudioSource source in sources)
        {
            Undo.DestroyObjectImmediate(source);
            sourcesRemoved++;
        }

        // Remove AudioListeners
        AudioListener[] listeners = selected.GetComponentsInChildren<AudioListener>(true);
        foreach (AudioListener listener in listeners)
        {
            Undo.DestroyObjectImmediate(listener);
            listenersRemoved++;
        }

        Debug.Log($"Removed {sourcesRemoved} AudioSource(s) and {listenersRemoved} AudioListener(s) from '{selected.name}' and its children.");
    }
}
