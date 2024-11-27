using UnityEngine;
using UnityEngine.UI;

public class FullscreenToggle : MonoBehaviour
{
    [SerializeField] private Toggle resolutionToggle;

    private void Start()
    {
        if (Screen.fullScreen == true)
        {
            resolutionToggle.isOn = true;
        }
        else
        {
            resolutionToggle.isOn = false;
        }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
