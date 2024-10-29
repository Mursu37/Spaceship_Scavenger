using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternativeBakedLightGrouped : MonoBehaviour
{
    private MeshRenderer[] rends;
    [SerializeField]
    public int currentLightState;
    [SerializeField]
    public int[] linkedLightProbes;

    private AlternativeLightsManager manager;


    // Start is called before the first frame update
    void Start()
    {
        rends = GetComponentsInChildren<MeshRenderer>();
        manager = FindObjectOfType<AlternativeLightsManager>();

        ChangeLightState(currentLightState, rends);
    }

    //Call the Manager script to change the linked lightprobes and then changes the objects lightmap texture locally
    public void ChangeLightState(int Value, MeshRenderer[] _rends)
    {
        //Make sure target lightmap texture is in the valid range
        Value = Mathf.Clamp(Value, 0, manager.maxStatesCount);
        currentLightState = Value;

        //Call the manager to change the selected lightprobes settings
        manager.AssignLightProbesSegment(linkedLightProbes, currentLightState);

        foreach (MeshRenderer rend in _rends)
        {
            //Set lightprobe and lightmap at start of game
            //Switch to a diffrent lightmap texture
            if (rend.gameObject.isStatic)
            rend.lightmapIndex = currentLightState;
        }
    }

        public void ChangeLightState(int Value)
    {
        //Make sure target lightmap texture is in the valid range
        Value = Mathf.Clamp(Value, 0, manager.maxStatesCount);
        currentLightState = Value;

        //Call the manager to change the selected lightprobes settings
        manager.AssignLightProbesSegment(linkedLightProbes, currentLightState);
            foreach (MeshRenderer rend in rends)

            {
                //Switch to a diffrent lightmap texture
                if (rend.gameObject.isStatic)
                rend.lightmapIndex = currentLightState;
        }
    }

    #region Interaction

    public void ToggleLights()
    {
        currentLightState += 1;
        if (currentLightState > manager.maxStatesCount)
        {
            currentLightState = 0;
        }
        ChangeLightState(currentLightState);
    }

    public void ToggleLights(int i)
    {
        currentLightState = i;
        if (currentLightState > manager.maxStatesCount)
        {
            currentLightState = manager.maxStatesCount;
        }

        if (currentLightState < 0)
        {
            currentLightState = 0;
        }

        ChangeLightState(currentLightState);
    }
    #endregion Interaction

#if UNITY_EDITOR
    //Used by the editor script for the selection tool
    [HideInInspector]
    public List<int> inRangeProbes;
    [HideInInspector]
    public List<int> tempList;
#endif
}
