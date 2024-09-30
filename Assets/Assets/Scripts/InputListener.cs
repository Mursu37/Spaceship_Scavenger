using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class InputListener : MonoBehaviour
{
    [SerializeField] private UniversalRendererData _renderer;
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            foreach (var feature in _renderer.rendererFeatures)
            {
                Debug.Log(feature.name);
                if (feature.name != "xray") continue;
                feature.SetActive(!feature.isActive);
            }
            
        }
    }
}
