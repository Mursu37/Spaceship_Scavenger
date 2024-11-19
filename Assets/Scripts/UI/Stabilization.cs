using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Stabilization : MonoBehaviour
{
    private TextMeshProUGUI textField;
    private PlayerMovement playerMovement;

    private void Start()
    {
        textField = GetComponent<TextMeshProUGUI>();
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    private void Update()
    {
        if (playerMovement.isStabilized)
        {
            textField.text = "V STABILIZATION <color=#00A0FF>ON</color><color=#3A5D6C>/OFF</color>";
        }
        else
        {
            textField.text = "V STABILIZATION <color=#3A5D6C>ON/</color><color=#00A0FF>OFF</color>";
        }
    }
}
