using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestEditorButtonDispatcher))]
public class Editor_TestEditorButtonDispatcher : Editor
{
    private TestEditorButtonDispatcher dispatcher;


    private void OnEnable()
    {
        dispatcher = target as TestEditorButtonDispatcher;
        if (target == null) return;
    }

    // Start is called before the first frame update
    public override void OnInspectorGUI()
    {

        if (GUILayout.Button("Dispatch Event"))
        {
            if (dispatcher != null)
            {
                dispatcher.DispatchEvent();
            }
        }

        base.OnInspectorGUI();
    }

}
