using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconSwitch : MonoBehaviour
{
    private ModeSwitch modeSwitch;
    private RawImage modeImage;
    [SerializeField] private Texture GravityGunIconOn;
    [SerializeField] private Texture GravityGunIconOff;
    [SerializeField] private GameObject multiTool;
    [SerializeField] private Texture CuttingToolIconOn;
    [SerializeField] private Texture CuttingToolIconOff;
    

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
            modeImage.texture = GravityGunIconOn;
        }
        else if (modeSwitch.selectedMode == 1)
        {
            modeImage.texture = GravityGunIconOff;
        }
        //if (modeSwitch.selectedMode == 1)
        //{
        //    modeImage.texture = CuttingToolIconOn;
        //}
        //else if (modeSwitch.selectedMode == 0)
        //{
        //    modeImage.texture = CuttingToolIconOff;
        //}
    }
}
