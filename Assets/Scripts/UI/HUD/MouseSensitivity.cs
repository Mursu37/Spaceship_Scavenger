using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivity : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private float savedSensitivity;

    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject pauseMenu;

    void Start()
    {
        savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 0.5f);

        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();

            if (playerMovement != null)
            {
                slider.value = savedSensitivity;
                valueText.text = (slider.value * 100).ToString("0.0");
            }
        }
    }

    public void SensitivitySlider(float value)
    {
        if (playerMovement != null)
        {
            playerMovement.turnAcceleration = value;
            valueText.text = (value * 100).ToString("0.0");
            PlayerPrefs.SetFloat("MouseSensitivity", value);
        }
    }
}
