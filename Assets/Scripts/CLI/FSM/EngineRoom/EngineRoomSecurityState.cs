using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CLI.FSM
{
    public class EngineRoomSecurityState : State
    {
        public EngineRoomSecurityState(StateController controller) : base(controller)
        {
            
        }

        public override void OnEnter()
        {

            stateController.AddText("Do you want to access the security systems? Y/N");
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
                stateController.AddText("Access to security systems cancelled by the user. Returning to system directory.");
            }

            else if (command == "y")
            {
                if (command.Length == 1)
                {
                    stateController.ChangeDeeper(new EngineRoomSecurityState_2(stateController), "Doors");


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
