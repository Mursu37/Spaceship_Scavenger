using System;
using System.Linq;
using CLI.FSM;
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
        //commandLineInput.ActivateInputField();
    }

    private void OnInput()
    {
        string[] userInputs = commandLineInput.text.ToLower().Split(" ");
        Debug.Log(commandLineInput.text);
        Debug.Log(userInputs.Length);
        /*
        if (commandLineInput.text.ToLower() == "ship on")
        {
            if(GetComponent<ShipPowerOn>() != null) {GetComponent<ShipPowerOn>().turnShipOn();}

            commandLineText.text = "Ship power has been turned on!";
        }

        else if (commandLineInput.text.ToLower() == "help")
        {
            commandLineText.text = "Commands: \n" +
                                   "HELP            Shows list of commands \n" +
                                   "SHIP ON         Turns ship power on";
        }
        else if (userInputs.Length >= 3)
        {
            if (userInputs[0] == "open" && userInputs[1] == "door")
            {
                if (userInputs[2].All(Char.IsDigit))
                {
                    Debug.Log("valid input opening door");
                }
            }
        }
        else
        {
            commandLineText.text = "Command not recognised. Try typing help to see available commands";
        }
        */
        
        commandLineInput.text = "";
        commandLineInput.ActivateInputField();
    }
}
