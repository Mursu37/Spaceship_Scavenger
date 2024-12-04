using System;
//using Palmmedia.ReportGenerator.Core.CodeAnalysis;
using UnityEngine;

[Serializable]
public struct ScanSettings
{
    public ScanSettings(Color col, LayerMask layer, Material mat, bool overrideDefaultThreshold = false, float outlineThreshold = 0)
    {
        color = col;
        layerMask = layer;
        overrideVisual = mat;
        overrideThreshold = overrideDefaultThreshold;
        threshold = outlineThreshold;
    }

    [ColorUsage(false, true)] public Color color;
    public LayerMask layerMask;
    public Material overrideVisual;
    public bool overrideThreshold;
    public float threshold;
}
