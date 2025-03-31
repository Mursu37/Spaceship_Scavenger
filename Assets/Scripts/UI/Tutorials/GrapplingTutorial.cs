using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingTutorial : MonoBehaviour
{
    [SerializeField] private GameObject gravityText;
    [SerializeField] private GameObject grabbleText;
    [SerializeField] private GameObject distanceText;

    [SerializeField] private GravityGun gravityGun;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Num1"))
        {
            if (!gravityText.GetComponent<SmoothTextHighlight>().canHighlight)
            {
                gravityText.GetComponent<SmoothTextHighlight>().StartHighlight();
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (!grabbleText.GetComponent<SmoothTextHighlight>().canHighlight)
            {
                grabbleText.GetComponent<SmoothTextHighlight>().StartHighlight();
            }
        }

        if ((Input.GetAxis("Mouse ScrollWheel") < 0 || Input.GetAxis("Mouse ScrollWheel") > 0) && gravityGun.isAttracting)
        {
            if (!distanceText.GetComponent<SmoothTextHighlight>().canHighlight)
            {
                distanceText.GetComponent<SmoothTextHighlight>().StartHighlight();
            }
        }
    }
}
