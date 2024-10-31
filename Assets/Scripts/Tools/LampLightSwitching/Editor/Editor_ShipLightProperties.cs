using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShipLightProperties))]
public class Editor_ShipLightProperties : Editor
{
    private ShipLightProperties _GUItarget;


    // Start is called before the first frame update
    void OnEnable()
    {

    }

    // Update is called once per frame
    void OnDisable()
    {

    }

    public override void OnInspectorGUI()
    {
        SerializedProperty alarm_toggleProperty = serializedObject.FindProperty("isAlarm");
        SerializedProperty alwaysOn_toggleProperty = serializedObject.FindProperty("isAlwaysOn");

        GUILayout.Space(25);

        GUILayout.Label("Toggle on if the light should be using red alarm lights.");

        GUILayout.Space(5);

        EditorGUILayout.PropertyField(alarm_toggleProperty, new GUIContent("is Alarm Light"));

        EditorGUILayout.PropertyField(alwaysOn_toggleProperty, new GUIContent("Light is Always On"));

        serializedObject.ApplyModifiedProperties();

        /*
        if (isAlarm)
        {
            _GUItarget.SetIsAlarmLight(true);
        }
        else
        {
            _GUItarget.SetIsAlarmLight(false);
        }*/


        base.OnInspectorGUI();
    
    }
}
