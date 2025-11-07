using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.Controls.AxisControl;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class LampSwitcherManager : MonoBehaviour
{
    [SerializeField]
    public ShipLightProperties[] lights;

    private bool isAlarmState;

    [SerializeField]
    private int poolIndex_dark;
    [SerializeField]
    private int poolIndex_ordinary;
    [SerializeField]
    private int poolIndex_alarm;

    private void OnEnable()
    {
        GameManager.OnPhaseChanged += OnGameChanged;
    }

    private void OnDisable()
    {
        GameManager.OnPhaseChanged -= OnGameChanged;
    }

    private void OnGameChanged(Phase newPhase)
    {
        if (newPhase == Phase.Meltdown)
        {
            SetAlarmOn();
        }
    }

    public void SetAlarmState(bool _bool)
    {
        isAlarmState = _bool;  
    }

    public bool GetAlarmState()
    {
        return isAlarmState;
    }

    public void SetupLamps()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].SetupLampLights();
            lights[i].SetupRenderer();
        }

    }

    public void ToggleLamps(bool _bool)
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].ToggleLight(_bool, isAlarmState);
        }

        if (_bool == false)
        {
            GetComponent<LightMapSceneLoader>()?.LoadLightMapScene(poolIndex_dark);
        }

    }

    public void SetAlarmOn()
    {
        SetAlarmState(true);

        if (lights != null)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                if (lights[i] != null)
                {
                    lights[i].SetAlarmState(true);
                }
            }

            for (int i = 0; i < lights.Length; i++)
            {
                if (lights[i] != null)
                {
                    lights[i].SetAlarmLights();
                }
            }
        }
        

        GetComponent<LightMapSceneLoader>()?.LoadLightMapScene(poolIndex_alarm);
    }

    public void SetOrdinaryLights()
    {
        SetAlarmState(false);

        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].SetOrdinaryLights();
        }

        GetComponent<LightMapSceneLoader>()?.LoadLightMapScene(poolIndex_ordinary);

    }


}
