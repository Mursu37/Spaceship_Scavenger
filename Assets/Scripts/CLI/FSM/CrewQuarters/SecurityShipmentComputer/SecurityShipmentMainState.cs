using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class SecurityShipmentMainState : State
    {
        public SecurityShipmentMainState(StateController controller) : base(controller)
        {
            directories.Add("security", new SecurityShipmentSecurityState(controller));
        }

        public override void OnEnter()
        {

            stateController.ChangeText("System directories:<BR><BR>--- security");
            base.OnEnter();
        }

        public override void Interpret(string[] command)
        {
            if (command[0] == "help")
            {
                stateController.ChangeText("Available commands: <BR> cd [directory_name] --- change directory");

            }
            else if (command[0] == "ls")
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

