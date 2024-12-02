using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.HighDefinition.CameraSettings;
using TMPro;
using Unity.VisualScripting;


public class Toolinfo : MonoBehaviour
{
    private ModeSwitch modeSwitch;
    private RawImage modeImage;
    private HighlightCuttable cuttableHighlight;
    private Cutting cutting;
    private GravityGun gravityGun;
    [SerializeField] private Image scrollIcon;
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
            cuttableHighlight = multiTool.transform.parent.GetComponentInParent<HighlightCuttable>();
            gravityGun = multiTool.GetComponent<GravityGun>();
        }

        modeImage = GetComponent<RawImage>();
    }

    // Update is called once per frame
  
    void FixedUpdate()
    {

        if (modeSwitch != null && gravityGun != null)
        {
            if (modeSwitch.selectedMode == 0)
            {
                modeImage.texture = grapplingInfo;

                float strengthPercentage = Mathf.Clamp((gravityGun.strength / maxStrength) * 100f, 0, 100f);

                infoText.text = "<align=left>OBJ WEIGHT<line-height=0>\r\n" +
                                $"<align=left><indent=53%>{gravityGun.objectMass:F1}KG</indent><line-height=1em>\r\n" +
                                "<align=left>GRAPL STR<line-height=0>\r\n" +
                                $"<align=left><indent=53%>{gravityGun.strength:F0}%</indent><line-height=1em>\r\n" +
                                "<align=left>OBJ DISTANCE<line-height=0>\r\n" +
                                $"<align=left><indent=53%>{gravityGun.distanceToPlayer:F1}M</indent><line-height=1em>";

                if (gravityGun.objectMass == 0)
                {
                    scrollIcon.gameObject.SetActive(false);
                }
                else
                {
                    scrollIcon.gameObject.SetActive(true);
                }

            }
            else if (modeSwitch.selectedMode == 1)
            {
                string aligned = "<color=#8b8b8b>YES</color>/NO";
                string detected = "<color=#8b8b8b>SCANNING..";
                string scanning = "";


                if (cuttableHighlight.currentlyCuttable != null)
                {
                    aligned = "YES/<color=#8b8b8b>NO</color>";
                }

                if (cuttableHighlight.AreObjectsInRange())
                {
                    detected = "DETECTED";
                    scanning = "";
                }

                infoText.text = "CUTTING ALIGNED - " + aligned + "\r\n" +
                                "CUTTABLE OBJ - " + detected + "\r\n" +
                                scanning;

                if (!cutting.isVerticalCut)
                {
                    modeImage.texture = horizontalcuttingInfo;
                }
                else
                {
                    modeImage.texture = verticalcuttingInfo;
                }
            }
        }

    }

}

