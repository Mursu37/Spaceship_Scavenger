using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

[CustomEditor(typeof(AlternativeBakedLightGrouped))]
public class Editor_AlternateBakedLightGrouped : Editor
{
    private AlternativeBakedLightGrouped myTarget;
    private Vector3[] probes_Pos;
    bool EditProbesMode;
    private Vector3 positionHandle;
    private float selectRange;

    private void OnEnable()
    {
        myTarget = (AlternativeBakedLightGrouped)target;
        if (myTarget == null)
        {
            return;
        }

            probes_Pos = LightmapSettings.lightProbes.positions;
        myTarget.tempList?.Clear();

        if (myTarget.linkedLightProbes != null && myTarget.linkedLightProbes.Length != 0)
        {
            foreach (var probe in myTarget.linkedLightProbes)
            {
                myTarget.tempList?.Add(probe);
            }
        }
    }

    private void OnDisable()
    {
        myTarget.inRangeProbes?.Clear();
        myTarget.tempList?.Clear();
    }

    public override void OnInspectorGUI()
    {
        myTarget = (AlternativeBakedLightGrouped)target;
        if (myTarget == null) return;

        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;

        if (!EditProbesMode)
        {
            if (GUILayout.Button("Edit Linked Probes"))
            {
                EditProbesMode = true;
            }
            if (GUILayout.Button("Refresh Lightmap(Runtime)"))
            {
                myTarget.ChangeLightState(myTarget.currentLightState);
            }
        }
        else
        {
            if (GUILayout.Button("Stop Edit Linked Probes"))
            {
                EditProbesMode = false;
            }

            GUILayout.Space(5);
            GUILayout.Label("Add or Remove Probes", style);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Selected"))
            {
                foreach (var probe in myTarget.inRangeProbes)
                {
                    if (!myTarget.tempList.Contains(probe))
                    {
                        myTarget.tempList.Add(probe);
                    }
                }
                myTarget.linkedLightProbes = myTarget.tempList.ToArray();
            }
            if (GUILayout.Button("Remove Selected"))
            {
                foreach (var probe in myTarget.inRangeProbes)
                {
                    myTarget.tempList.Remove(probe);
                }
                myTarget.linkedLightProbes = myTarget.tempList.ToArray();
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Clear All Linked Probes"))
            {
                myTarget.tempList.Clear();
                myTarget.linkedLightProbes = myTarget.tempList.ToArray();
            }
            GUILayout.Space(5);
            if (GUILayout.Button("Center Selection Tool"))
            {
                positionHandle = SceneView.lastActiveSceneView.camera.transform.position;
            }
        }
        base.OnInspectorGUI();
    }

    void OnSceneGUI()
    {
        if (EditProbesMode)
        {
            // Draw selection sphere handle
            Handles.color = Color.blue;
            Handles.DrawWireDisc(positionHandle, Vector3.up, selectRange);
            positionHandle = Handles.PositionHandle(positionHandle, Quaternion.identity);
            selectRange = Handles.RadiusHandle(Quaternion.identity, positionHandle, selectRange);
            selectRange = Mathf.Clamp(selectRange, 0.5f, 1000);

            // Clear previous range selection to refresh the list
            myTarget.inRangeProbes?.Clear();

            Event guiEvent = Event.current;

            // Iterate through probes to check for selection and handle interaction
            for (int i = 0; i < probes_Pos.Length; i++)
            {
                Vector3 probePos = probes_Pos[i];

                // Check if probe is within the selectRange
                if (Vector3.Distance(probePos, positionHandle) < selectRange)
                {
                    // Highlight the probe as selectable (green)
                    Handles.color = Color.green;
                    Handles.SphereHandleCap(0, probePos, Quaternion.identity, 0.4f, EventType.Repaint);

                    // Add probe index to inRangeProbes list
                    if (myTarget != null)
                    myTarget.inRangeProbes.Add(i);

                    // Check for left mouse click to toggle probe selection
                    if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0)
                    {
                        if (HandleUtility.DistanceToCircle(probePos, 0.5f) < 5f)
                        {
                            if (myTarget.tempList != null && myTarget.tempList.Contains(i))
                            {
                                myTarget.tempList.Remove(i);  // Deselect if already selected
                            }
                            else if (myTarget.tempList != null)
                            {
                                myTarget.tempList.Add(i);  // Select if not in list
                            }
                            // Refresh the editor view
                            EditorUtility.SetDirty(myTarget);
                            SceneView.RepaintAll();
                        }
                    }
                }
                else
                {
                    // Display the probe in red if not in selection range
                    Handles.color = Color.red;
                    Handles.SphereHandleCap(0, probePos, Quaternion.identity, 0.4f, EventType.Repaint);
                }
            }

            // Mark the array of linked probes to be updated after selection changes
            if (myTarget.tempList != null)
            {
                myTarget.linkedLightProbes = myTarget.tempList.ToArray();
            }
        }
        else
        {
                if (myTarget.tempList != null)
                {
                    // Display linked probes in yellow when not in edit mode
                    Handles.color = Color.yellow;
                    foreach (int probeIndex in myTarget.tempList)
                    {
                        Handles.SphereHandleCap(0, probes_Pos[probeIndex], Quaternion.identity, 0.2f, EventType.Repaint);
                    }
                }
            }
        }
}