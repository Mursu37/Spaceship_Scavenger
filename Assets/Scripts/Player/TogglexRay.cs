using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class TogglexRay : MonoBehaviour
{
    [SerializeField] private CustomPassVolume customPassVolume;
    [SerializeField] private GameObject lowPolyLayout;
    [SerializeField] private GameObject xRayRadius;

    private bool isXRayActive;
    private GameObject xRaySphere;

    private void Start()
    {
        
        isXRayActive = false;
        /*
        lowPolyLayout.SetActive(isXRayActive);
        */
        customPassVolume.enabled = isXRayActive;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && !isXRayActive)
        {
            xRaySphere = Instantiate(xRayRadius, transform.position, quaternion.identity);
            xRaySphere.GetComponentInChildren<XRayRadius>().SetOwner(this);
            isXRayActive = true;
            customPassVolume.enabled = true;
        }
    }

    public void XRayEnded()
    {
        isXRayActive = false;
        customPassVolume.enabled = isXRayActive;
    }
}
