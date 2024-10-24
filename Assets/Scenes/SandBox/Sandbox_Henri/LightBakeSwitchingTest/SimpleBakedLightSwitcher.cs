using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleBakedLightSwitcher : MonoBehaviour
{

    public Texture2D[] darkLightmapDir, darkLightmapColor;
    public Texture2D[] brightLightmapDir, brightLightmapColor;
    private LightmapData[] darkLightmap, brightLightmap;

    // Start is called before the first frame update
    void Start()
    {
        List<LightmapData> dLightmap = new List<LightmapData>();
       
       for(int i = 0; i < darkLightmapDir.Length; i++)
       {
            LightmapData lmdata = new LightmapData();
            //lmdata.lightmapDir = darkLightmapDir[i];
            lmdata.lightmapColor = darkLightmapColor[i];

            dLightmap.Add(lmdata);
       }

       darkLightmap = dLightmap.ToArray();

        List<LightmapData> blightmap = new List<LightmapData>();

         for(int i = 0; i < brightLightmapDir.Length; i++)
       {
            LightmapData lmdata = new LightmapData();
            //lmdata.lightmapDir = brightLightmapDir[i];
            lmdata.lightmapColor = brightLightmapColor[i];

            dLightmap.Add(lmdata);
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

    private void OnLight1Switched()
    {
        print("Light1Switched");
       // LightSensor[0].enabled = !LightSensor[0].enabled;
       LightmapSettings.lightmaps = darkLightmap;
    }

    private void OnLight2Switched()
    {
        print("Light2Switched");
       // LightSensor[1].enabled = !LightSensor[1].enabled;
        LightmapSettings.lightmaps = brightLightmap;
    }
}
