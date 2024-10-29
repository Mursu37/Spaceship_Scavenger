using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


[CreateAssetMenu(fileName = "AlternativeLightData", menuName = "Swapping Lightmaps/AlternativeLightData", order = 1)]

public class AlternativeLightingData : ScriptableObject
{
    [Header ("Stored Light Maps")]
    [SerializeField]
    protected Texture2D[] l_Light = new Texture2D[1];
    [SerializeField]
    protected Texture2D[] l_Dir = new Texture2D[1];

    [Header("Stored Light Probes Settings")]
    [SerializeField]
    protected SphericalHarmonicsL2[] lightProbesData;

    // [SerializeField]
    // public LightmapData refLightmapData;

    public Texture2D[] getLight()
    {
        return l_Light;
    }

    public Texture2D[] getDir()
    {
        return l_Dir;
    }

    public void setLight(Texture2D[] light)
    {
        this.l_Light = light;
    }

    public void setDir(Texture2D[] dir)
    {
        this.l_Dir = dir;
    }

    public SphericalHarmonicsL2[] getProbes()
    {
        return lightProbesData;
    }

    public void setProbes(SphericalHarmonicsL2[] probes)
    {
        this.lightProbesData = probes;
    }

#if UNITY_EDITOR
    //The EditorWindow_GetAlternativeLightingData script uses these value to temporary store the found textures
    [HideInInspector]
    public List<Texture2D> l_LightTemp;
    [HideInInspector]
    public List<Texture2D> l_DirTemp;
#endif
}
