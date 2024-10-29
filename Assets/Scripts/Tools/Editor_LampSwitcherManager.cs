using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

[CustomEditor(typeof(LampSwitcherManager))]
public class Editor_LampSwitcherManager : Editor
{
    private LampSwitcherManager lampManager;

    private void OnEnable()
    {
        lampManager = (LampSwitcherManager)target;
        if (lampManager == null)
        {
            return;
        }

        lampManager.SetupLamps();
        lampManager.SetupAlarmLamps();
    }

    // Update is called once per frame
    private void OnDisable()
    {

    }

    public override void OnInspectorGUI()
    {
        //Editor Styling & Text Alignment
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;



        GUILayout.Space(25);
        GUILayout.Label("Toggle Light mode between lit and unlit.");

        GUILayout.Space(5);

        //Toggle Button
        if (GUILayout.Button("Toggle Lights On"))
        {
            Debug.Log("toggled lights on");
            lampManager.ToggleLampsOn();
            EditorUtility.SetDirty(lampManager);
        }

        if (GUILayout.Button("Toggle Lights Off"))
        {
            Debug.Log("toggled lights off");
            lampManager.ToggleLampsOff();
            EditorUtility.SetDirty(lampManager);
        }

        GUILayout.Label("Toggle Selected Lamps between Ordinary and Alarm Materials.");

        if (GUILayout.Button("Set Ordinary Lamp Materials"))
        {
            Debug.Log("toggled Ordinary Lamp Materials");
            lampManager.SetOrdinaryLampMaterials();
            EditorUtility.SetDirty(lampManager);
        }

        if (GUILayout.Button("Set Alarm Lamp Materials"))
        {
            Debug.Log("toggled Alarm lamp Materials");
            lampManager.SetAlarmLampMaterials();
            EditorUtility.SetDirty(lampManager);
        }

        GUILayout.Space(25);


        // Here add also the original GUI content of the base class
        base.OnInspectorGUI();

    }


}
