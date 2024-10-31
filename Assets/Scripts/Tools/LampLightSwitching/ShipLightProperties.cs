using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ShipLightProperties : MonoBehaviour
{
    private List<Light> lights;
    private List<Light> alarmLights;
    private MeshRenderer mRenderer;
    [SerializeField]
    private int materialSlotIndex = 1;
    [HideInInspector]
    public bool isAlarm;
    private bool isAlarmState;
    [HideInInspector]
    public bool isAlwaysOn;

    public Material lightMaterialOrdinary;
    public Material lightMaterialAlarm;
    public Material lightMaterialOff;


    // Start is called before the first frame update
    void Start()
    {
        SetupLampLights();
        SetupRenderer();
    }

    public void SetupLampLights()
    {
        Debug.Log("Setupping Light");

        lights = new List<Light>();
        alarmLights = new List<Light>();
        lights.AddRange(GetComponentsInChildren<Light>());

        List<Light> lightsToAlarm = new List<Light>(lights);
        for (int i = 0; i < lightsToAlarm.Count; i++)
        {
            var light = lightsToAlarm[i];

            if (light.gameObject.CompareTag("Alarm"))
            {
                lights.Remove(light);
                alarmLights.Add(light);
            }
        }
    }

    public void SetupRenderer()
    {
        mRenderer = GetComponentInChildren<MeshRenderer>();

    }

    public void SetAlarmState(bool _bool)
    {
        isAlarmState = _bool;
    }

    public void SetIsAlarmLight(bool _bool)
    {
        isAlarm = _bool;
    }


    public void ToggleLight(bool _bool, bool _alarmState)
    {
        isAlarmState = _alarmState;

        if (_bool)
        {
            //Toggle Light Here
            if (isAlarmState)
            {
                SetAlarmLights();
            }
            else
            {
                SetOrdinaryLights();
            }

        }
        else
        {
            if (!isAlwaysOn)
            {
                //Toggle Light Off

                Material[] materials = mRenderer.sharedMaterials;

                if (materialSlotIndex < materials.Length)
                {
                    materials[materialSlotIndex] = lightMaterialOff;
                }

                // Apply the copy of sharedMaterials to the MeshRenderer
                mRenderer.sharedMaterials = materials;
            }

#if UNITY_EDITOR
            EditorUtility.SetDirty(this.gameObject);
#endif

            if (!isAlwaysOn)
            {
                //Toggle light components off
                ToggleLightComponents_Ordinary(false);
                ToggleLightComponents_Alarm(false);
            }

        }
    }

    public void SetOrdinaryLights()
    {
        // Set the ordinary Light materials on here

        Material[] materials = mRenderer.sharedMaterials;

        if (materialSlotIndex < materials.Length)
        {
            materials[materialSlotIndex] = lightMaterialOrdinary;
        }

        // Apply the copy of sharedMaterials to the MeshRenderer
        mRenderer.sharedMaterials = materials;

#if UNITY_EDITOR
        EditorUtility.SetDirty(this.gameObject);
#endif

        ToggleLightComponents_Ordinary(true);
        ToggleLightComponents_Alarm(false);

    }

    public void SetAlarmLights()
    {
        // Set alarm materials on here
        if (isAlarm)
        {
            Material[] materials = mRenderer.sharedMaterials;

            if (materialSlotIndex < materials.Length)
            {
                materials[materialSlotIndex] = lightMaterialAlarm;
            }

            // Apply the copy of sharedMaterials to the MeshRenderer
            mRenderer.sharedMaterials = materials;

#if UNITY_EDITOR
            EditorUtility.SetDirty(this.gameObject);
#endif

            ToggleLightComponents_Alarm(true);
            ToggleLightComponents_Ordinary(false);

        }
        else
        {
            // If not an alarm light, set as ordinary light materials
            SetOrdinaryLights();
        }
    }

    public void ToggleLightComponents_Alarm(bool _bool)
    {
        for (int i = 0; i < alarmLights.Count; i++)
        {
            alarmLights[i].enabled = _bool;
        }
    }

    public void ToggleLightComponents_Ordinary(bool _bool)
    {
        for (int i = 0; i < lights.Count; i++)
        {
            lights[i].enabled = _bool;
        }
    }
}
