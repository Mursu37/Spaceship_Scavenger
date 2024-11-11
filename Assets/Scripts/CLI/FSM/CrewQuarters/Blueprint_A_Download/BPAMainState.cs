using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class BPAMainState : State
    {
        public BPAMainState(StateController controller) : base(controller)
        {
            directories.Add("ship_blueprints_a", new BPADownloadState(controller));
        }

        public override void OnEnter()
        {

            stateController.ChangeText("System directories:<BR><BR>--- ship_blueprints_a");
            base.OnEnter();
        }

        public override void Interpret(string[] command)
        {
            if (command[0] == "help")
            {
                stateController.ChangeText("Available commands: <BR> cd [directory_name] --- change directory");

            }
            else
            {
                base.Interpret(command);
            }


        }
    }
}

