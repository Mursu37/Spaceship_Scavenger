using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class TogglexRay : MonoBehaviour
{
    [SerializeField] private CustomPassVolume customPassVolume;
    [SerializeField] private GameObject lowPolyLayout;

    private bool isXRayActive;

    private void Start()
    {
        isXRayActive = false;
        lowPolyLayout.SetActive(isXRayActive);
        customPassVolume.enabled = isXRayActive;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            isXRayActive = !isXRayActive;
            customPassVolume.enabled = isXRayActive;
            lowPolyLayout.SetActive(isXRayActive);

        }
    }
}
