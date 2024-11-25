using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.HighDefinition.CameraSettings;
using TMPro;


public class Toolinfo : MonoBehaviour
{
    private ModeSwitch modeSwitch;
    private RawImage modeImage;
    private Cutting cutting;
    private GravityGun gravityGun;
    [SerializeField] private GameObject multiTool;
    [SerializeField] private Texture grapplingInfo;
    [SerializeField] private Texture verticalcuttingInfo;
    [SerializeField] private Texture horizontalcuttingInfo;
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private float maxStrength = 100f;

    // Start is called before the first frame update
    void Start()
    {
        if (multiTool != null)
        {
            modeSwitch = multiTool.GetComponent<ModeSwitch>();
            cutting = multiTool.GetComponent<Cutting>();
            gravityGun = multiTool.GetComponent<GravityGun>();
        }

        modeImage = GetComponent<RawImage>();
    }

    // Update is called once per frame
  
    void Update()
    {
        if (modeSwitch != null && gravityGun != null)
        { 
            if (modeSwitch.selectedMode == 0)
            {
                modeImage.texture = grapplingInfo;

                float strengthPercentage = Mathf.Clamp((gravityGun.strength / maxStrength) * 100f, 0, 100f);

                infoText.text =     $"OBJ WEIGHT {gravityGun.objectMass:F1}KG\n" +
                                    $"GRAPL STRENGTH {gravityGun.strength:F0}%\n" +
                                    $"OBJ DISTANCE {gravityGun.distanceToPlayer:F1}M";
            }
        else if (modeSwitch.selectedMode == 1)
        {
            if (!cutting.isVerticalCut)
            {
                modeImage.texture = horizontalcuttingInfo;
                infoText.text = "CUTTING ALIGNED\nCUTTABLE OBJ\nSCANNING OBJS";
            }
            else
            {
                modeImage.texture = verticalcuttingInfo;
                infoText.text = "CUTTING ALIGNED\nCUTTABLE OBJ\nSCANNING OBJS";
                }
            }
        }
    }
}

