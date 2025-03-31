using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingTutorial : MonoBehaviour
{
    [SerializeField] private GameObject cuttingText;
    [SerializeField] private GameObject cutText;
    [SerializeField] private GameObject angleText;

    [SerializeField] private Cutting cutting;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Num2"))
        {
            if (!cuttingText.GetComponent<SmoothTextHighlight>().canHighlight)
            {
                cuttingText.GetComponent<SmoothTextHighlight>().StartHighlight();
            }
        }

        if (cutting.isAligned)
        {
            if (!cutText.GetComponent<SmoothTextHighlight>().canHighlight)
            {
                cutText.GetComponent<SmoothTextHighlight>().StartHighlight();
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (!angleText.GetComponent<SmoothTextHighlight>().canHighlight)
            {
                angleText.GetComponent<SmoothTextHighlight>().StartHighlight();
            }
        }
    }
}
