using UnityEngine;

public class Controls : MonoBehaviour
{
    [SerializeField] private GameObject controls; // The Controls panel GameObject
    private bool controlsOpen = false;

    private void Update()
    {
        // Check for F1 key press to toggle the controls panel
        if (Input.GetButtonDown("OpenControls"))
        {
            if (!controlsOpen)
                OpenControls();
            else
                CloseControls();
        }
    }

    private void OpenControls()
    {
        controlsOpen = true;
        controls.SetActive(true);
        Time.timeScale = 0f; // Freeze the game when controls are open
    }

    private void CloseControls()
    {
        controlsOpen = false;
        controls.SetActive(false);
        Time.timeScale = 1f; // Resume the game when controls are closed
    }
}
