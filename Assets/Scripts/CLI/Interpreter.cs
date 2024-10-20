using System;
using Enviroment.MainTerminal;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Interpreter : MonoBehaviour
{
    [SerializeField] private TMP_Text commandLineText;
    [SerializeField] private TMP_InputField commandLineInput;

    private void OnEnable()
    {
        commandLineInput.ActivateInputField();
    }
    
    private void OnDisable()
    {
        commandLineInput.DeactivateInputField(true);
    }

    private void OnGUI()
    {
        if (commandLineInput.text != "" && Input.GetKeyDown(KeyCode.Return))
        {
            OnInput();    
        }
    }

    private void Update()
    {
        commandLineInput.ActivateInputField();
    }

    private void OnInput()
    {
        Debug.Log(commandLineInput.text);
        if (commandLineInput.text.ToLower() == "ship on")
        {
            if(GetComponent<ShipPowerOn>() != null)
            {GetComponent<ShipPowerOn>().turnShipOn();}
        }
        commandLineInput.text = "";
        commandLineInput.ActivateInputField();
    }
}
