using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{

    [SerializeField] GameObject FlashlightToggle;

    private bool FlashlightActive = false;
    // Start is called before the first frame update
    void Start()
    {
        FlashlightToggle.gameObject.SetActive(false);
    }

    public void ToggleFlashLightOff()
    {
        FlashlightToggle.gameObject.SetActive(false);
        FlashlightActive = false;
        AudioManager.PlayModifiedClipAtPoint("FlashLightOff", transform.position, 1, 1, 1, 500);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Flashlight") && !PauseGame.isPaused)
        {
            if (FlashlightActive == false)
            {
                FlashlightToggle.gameObject.SetActive(true);
                FlashlightActive = true;
                AudioManager.PlayModifiedClipAtPoint("FlashLightOn", transform.position, 1, 1, 1, 500);
            }
            else
            {
                FlashlightToggle.gameObject.SetActive(false);
                FlashlightActive = false;
                AudioManager.PlayModifiedClipAtPoint("FlashLightOff", transform.position, 1, 1, 1, 500);
            }
        }
    }
}
