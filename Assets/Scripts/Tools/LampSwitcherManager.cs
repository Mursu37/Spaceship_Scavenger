using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LampSwitcherManager : MonoBehaviour
{
    [SerializeField]
    public GameObject[] lamps;
    [SerializeField]
    public GameObject[] alarmLamps;
    [HideInInspector]
    public List<Light> lights;

    public List<MeshRenderer> alarmLampMeshRenderers;
    [SerializeField]
    public Material lampMaterialOrdinary;
    [SerializeField]
    public Material lampMaterialAlarm;

    public int materialSlotIndex = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetupLamps()
    {
        foreach (GameObject lamp in lamps)
        {
            Light[] _lights = lamp.GetComponentsInChildren<Light>();

            lights.AddRange(_lights);

        }
    }

    public void SetupAlarmLamps()
    {
        foreach (GameObject alarmlamp in alarmLamps)
        {
            MeshRenderer renderer = alarmlamp.GetComponentInChildren<MeshRenderer>();

            if (renderer.sharedMaterials.GetValue(1) != null && !alarmLampMeshRenderers.Contains(renderer))
            {
                alarmLampMeshRenderers.Add(renderer);
            }
        }

    }

    // Update is called once per frame
    public void ToggleLampsOff()
    {
        foreach (Light light in lights)
        {
            light.enabled = false;
        }

    }

    public void ToggleLampsOn()
    {
        foreach (Light light in lights)
        {
            light.enabled = true;
        }

    }

    public void SetOrdinaryLampMaterials()
    {
        foreach (MeshRenderer a_lamp in alarmLampMeshRenderers)
        {
            Material[] materials = a_lamp.sharedMaterials;
            if (materialSlotIndex < materials.Length)
            {
                materials[materialSlotIndex] = lampMaterialOrdinary;
            }

            a_lamp.sharedMaterials = materials;

#if UNITY_EDITOR
            EditorUtility.SetDirty(a_lamp.gameObject);
#endif
        }
    }

    public void SetAlarmLampMaterials()
    {
        foreach (MeshRenderer a_lamp in alarmLampMeshRenderers)
        {
            Debug.Log(a_lamp);

            Material[] materials = a_lamp.sharedMaterials;

            if (materialSlotIndex < materials.Length)
            {
                materials[materialSlotIndex] = lampMaterialAlarm;
            }

            a_lamp.sharedMaterials = materials;

#if UNITY_EDITOR
            EditorUtility.SetDirty(a_lamp.gameObject);
#endif
        }

    }


}
