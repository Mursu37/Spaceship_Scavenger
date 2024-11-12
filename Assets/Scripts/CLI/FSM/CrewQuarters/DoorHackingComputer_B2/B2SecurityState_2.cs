using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CLI.FSM
{
    public class B2SecurityState_2 : State
    {
        public B2SecurityState_2(StateController controller) : base(controller)
        {
            
        }

        public override void OnEnter()
        {

            stateController.AddText("Do you want to deactivate the the security systems? Y/N");
            base.OnEnter();
        }

        public override void Interpret(string[] command)
        {
            if (command[0] == "help")
            {
                stateController.AddText("Confirm your choise by typing \"Y\" or cancel by typing \"N\"");
            }

            else if (command[0] == "N" || command[0] == "n")
            {
                stateController.BackOne();
                stateController.AddText("Function cancelled by the user. Returning to Security systems directory.");
            }

            else if (command[0] == "Y" || command[0] == "y")
            {
                if (command.Length == 1)
                {
                    EventDispatcher dispatcher;
                    dispatcher = stateController.gameObject.GetComponent<EventDispatcher>();
                    dispatcher.TriggerEvent();

                    stateController.AddText("Security Systems in B2 have been deactivated. Security doors unlocked.");
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
