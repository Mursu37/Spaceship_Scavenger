using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CLI.FSM
{
    public class SecurityShipmentSecurityState_2 : State
    {
        public SecurityShipmentSecurityState_2(StateController controller) : base(controller)
        {
            
        }

        public override void OnEnter()
        {

            stateController.AddText("Do you want to deactivate the security systems? Y/N");
            base.OnEnter();
        }

        public override void Interpret(string command)
        {
            if (command == "help")
            {
                stateController.AddText("Confirm your choice by typing \"Y\" or cancel by typing \"N\"");
            }

            else if (command == "n")
            {
                stateController.BackOne();
                stateController.AddText("Function cancelled by the user. Returning to security systems directory.");
            }

            else if (command == "y")
            {
                if (command.Length == 1)
                {
                    EventDispatcher dispatcher;
                    dispatcher = stateController.gameObject.GetComponent<EventDispatcher>();
                    dispatcher.TriggerEvent();

                    stateController.AddText("Security systems in the security shipment have been deactivated.");
                }
                else
                {
                    base.Interpret(command);
                }
            }
            else
            {
                base.Interpret(command);
            }
        }
    }
}
