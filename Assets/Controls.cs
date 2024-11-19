using UnityEngine;

public class Controls : MonoBehaviour
{
    [SerializeField] private GameObject controls; // The Controls panel GameObject

    private bool controlsOpen = false;

    private void Update()
    {
        // Toggle controls visibility using the custom input button (F1)
        if (Input.GetButtonDown("OpenControls"))
        {
            if (!controlsOpen)
                OpenControls();
        }
        else if (Input.GetButtonUp("OpenControls") && controlsOpen)
        {
            CloseControls();
        }
    }

    private void OpenControls()
    {
        controlsOpen = true;
        controls.SetActive(true);
        Time.timeScale = 0f; // Optional: Freeze game while viewing controls
    }

    private void CloseControls()
    {
        controlsOpen = false;
        controls.SetActive(false);
        Time.timeScale = 1f; // Optional: Resume game time
    }
}
