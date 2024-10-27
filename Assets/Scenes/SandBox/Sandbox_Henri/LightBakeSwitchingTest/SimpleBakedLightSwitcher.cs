using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.Rendering;
using UnityEngine;
//using UnityEngine.Experimental.Rendering;
using UnityEngine.InputSystem;

public class SimpleBakedLightSwitcher : MonoBehaviour
{

    public GameObject[] darkLightPrefab, brightLightPrefab;

    public Texture2D[] darkLightmapDir, darkLightmapColor;
    public Texture2D[] brightLightmapDir, brightLightmapColor;
    private LightmapData[] darkLightmap, brightLightmap;
    public ReflectionProbe[] lightReflectionProbe, darkReflectionProbe;

    // Start is called before the first frame update
    void Start()
    {
        List<LightmapData> dLightmap = new List<LightmapData>();

        for (int i = 0; i < darkLightmapDir.Length; i++)
        {
            LightmapData lmdata = new LightmapData();
            lmdata.lightmapDir = darkLightmapDir[i];
            lmdata.lightmapColor = darkLightmapColor[i];

            dLightmap.Add(lmdata);
        }

        darkLightmap = dLightmap.ToArray();

        List<LightmapData> blightmap = new List<LightmapData>();

        for (int i = 0; i < brightLightmapDir.Length; i++)
        {
            LightmapData lmdata = new LightmapData();
            lmdata.lightmapDir = brightLightmapDir[i];
            lmdata.lightmapColor = brightLightmapColor[i];

            blightmap.Add(lmdata);
        }

        brightLightmap = blightmap.ToArray();

    }

    // Update is called once per frame
    void Update()
    {
         if (Keyboard.current.bKey.isPressed)
         {
            OnLight1Switched();
         }

         if (Keyboard.current.cKey.isPressed)
         {
            OnLight2Switched();
         }
    }

    private void EnablePrefabCollection(GameObject[] gameObjectList)
    {
        foreach (GameObject _go in gameObjectList)
        {
            _go.SetActive(true);
        }
    }

    private void DisablePrefabCollection(GameObject[] gameObjectList)
    {
        foreach (GameObject _go in gameObjectList)
        {
            _go.SetActive(false);
        }
    }

    private void OnLight1Switched()
    {
        print("Light1Switched");
        // LightSensor[0].enabled = !LightSensor[0].enabled;
        DisablePrefabCollection(brightLightPrefab);
        EnablePrefabCollection(darkLightPrefab);
        LightmapSettings.lightmaps = darkLightmap;


    }

    private void OnLight2Switched()
    {
        print("Light2Switched");
        // LightSensor[1].enabled = !LightSensor[1].enabled;
        DisablePrefabCollection(darkLightPrefab);
        EnablePrefabCollection(brightLightPrefab);
        LightmapSettings.lightmaps = brightLightmap;
 
    }
}
