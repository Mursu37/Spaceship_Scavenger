using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mode : MonoBehaviour
{
    private ModeSwitch modeSwitch;
    private RawImage modeImage;
    private Cutting cutting;
    [SerializeField] private GameObject multiTool;
    [SerializeField] private Texture grapplingTexture;
    [SerializeField] private Texture cuttingTextureHorizontal;
    [SerializeField] private Texture cuttingTextureVertical;
    /* [SerializeField] private Texture grapplingInfo;
     [SerializeField] private Texture cuttingInfo;*/

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
            modeImage.texture = grapplingTexture;
            //modeImage.texture = grapplingInfo;
        }
        else if (modeSwitch.selectedMode == 1)
        {
            if (!cutting.isVerticalCut)
            {
                modeImage.texture = cuttingTextureHorizontal;
            }
            else
            {
                modeImage.texture = cuttingTextureVertical;
            }
            //modeImage.texture = cuttingInfo;
        }
    }
}
