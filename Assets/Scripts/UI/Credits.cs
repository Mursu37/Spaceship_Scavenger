using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField] private FadeIn fadeIn;
    [SerializeField] private FadeOut fadeOut;

    private void Update()
    {
        if (Input.GetButtonDown("Cancel") && fadeIn.allFadedIn)
        {
            fadeOut.StartFadeOut();
            fadeIn.allFadedIn = false;
        }

        if (fadeOut.allFadedOut)
        {
            gameObject.SetActive(false);
            fadeOut.allFadedOut = false;
        }
    }
}
