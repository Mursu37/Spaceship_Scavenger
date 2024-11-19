using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFadeOut : MonoBehaviour
{
    private FadeOut fadeOut;

    private void Start()
    {
        fadeOut = GetComponent<FadeOut>();
        Invoke("FadeOutStart", 0.5f);
    }

    private void FadeOutStart()
    {
        if (fadeOut != null)
        {
            fadeOut.StartFadeOut();
        }
    }

    private void Update()
    {
        if (fadeOut != null && fadeOut.allFadedOut)
        {
            gameObject.SetActive(false);
        }
    }
}
