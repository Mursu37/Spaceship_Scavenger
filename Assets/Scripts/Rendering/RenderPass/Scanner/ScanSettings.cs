using System;
using UnityEngine;

[Serializable]
public struct ScanSettings
{
    public ScanSettings(Color col, LayerMask layer, Material mat)
    {
        color = col;
        layerMask = layer;
        overrideVisual = mat;
    }

    [ColorUsage(false, true)] public Color color;
    public LayerMask layerMask;
    public Material overrideVisual;
}
