using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Mode : MonoBehaviour
{
    private ModeSwitch modeSwitch;
    private TextMeshProUGUI modeText;
    [SerializeField] private GameObject multiTool;

    // Start is called before the first frame update
    void Start()
    {
        if (multiTool != null)
        {
            modeSwitch = multiTool.GetComponent<ModeSwitch>();
        }

        modeText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (modeSwitch.selectedMode == 0)
        {
            modeText.text = "Mode: Grappling";
        }
        else if (modeSwitch.selectedMode == 1)
        {
            modeText.text = "Mode: Cutting";
        }
    }
}
