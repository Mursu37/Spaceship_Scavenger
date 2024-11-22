using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class EngineRoomMainState : State
    {
        public EngineRoomMainState(StateController controller) : base(controller)
        {
            directories.Add("security", new EngineRoomSecurityState(controller));
        }

        public override void OnEnter()
        {

            stateController.ChangeText("System directories:<BR><BR>--- security");
            base.OnEnter();
        }

        public override void Interpret(string command)
        {
            if (command == "help")
            {
                stateController.ChangeText("Available commands: <BR> cd [directory_name] --- change directory");

            }
            else if (command == "ls")
            {
                stateController.ChangeText("Listing Directories:<BR> -- security");

            }
            else
            {
                base.Interpret(command);
            }


        }
    }
}

