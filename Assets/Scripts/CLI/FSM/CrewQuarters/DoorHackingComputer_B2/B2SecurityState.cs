using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CLI.FSM
{
    public class B2SecurityState : State
    {
        public B2SecurityState(StateController controller) : base(controller)
        {
            
        }

        public override void OnEnter()
        {

            stateController.AddText("Do you want to access the security systems? Y/N");
            base.OnEnter();
        }

        public override void Interpret(string[] command)
        {
            if (command[0] == "help")
            {
                stateController.AddText("Available commands: <BR> cd [directory_name] --- change directory <BR> download [file_name] --- download selected file");
            }

            else if (command[0] == "N" || command[0] == "n")
            {
                stateController.BackOne();
                stateController.AddText("Access to security systems cancelled by the user. Returning to system directory.");
            }

            else if (command[0] == "Y" || command[0] == "y")
            {
                if (command.Length == 1)
                {
                    stateController.ChangeDeeper(new B2SecurityState_2(stateController), "Doors");


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
