using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mode : MonoBehaviour
{
    private ModeSwitch modeSwitch;
    private RawImage modeImage;
    [SerializeField] private GameObject multiTool;
    [SerializeField] private Texture grapplingTexture;
    [SerializeField] private Texture cuttingTexture;
   /* [SerializeField] private Texture grapplingInfo;
    [SerializeField] private Texture cuttingInfo;*/

    // Start is called before the first frame update
    void Start()
    {
        if (multiTool != null)
        {
            modeSwitch = multiTool.GetComponent<ModeSwitch>();
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
            modeImage.texture = cuttingTexture;
            //modeImage.texture = cuttingInfo;
        }
    }
}
