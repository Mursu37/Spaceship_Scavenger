using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.HighDefinition.CameraSettings;

public class Toolinfo : MonoBehaviour
{
    private ModeSwitch modeSwitch;
    private RawImage modeImage;
    private Cutting cutting;
    [SerializeField] private GameObject multiTool;
    [SerializeField] private Texture grapplingInfo;
    [SerializeField] private Texture verticalcuttingInfo;
    [SerializeField] private Texture horizontalcuttingInfo;


    // Start is called before the first frame update
    void Start()
    {
        if (multiTool != null)
        {
            modeSwitch = multiTool.GetComponent<ModeSwitch>();
            cutting = multiTool.GetComponent<Cutting>();
        }

        modeImage = GetComponent<RawImage>();
    }

    // Update is called once per frame
  
    void Update()
    {
        if (modeSwitch.selectedMode == 0)
        {
            modeImage.texture = grapplingInfo;
            //modeImage.texture = grapplingInfo;
        }
        else if (modeSwitch.selectedMode == 1)
        {
            if (!cutting.isVerticalCut)
            {
                modeImage.texture = horizontalcuttingInfo;
            }
            else
            {
                modeImage.texture = verticalcuttingInfo;
            }
            //modeImage.texture = cuttingInfo;
        }
    }
}
