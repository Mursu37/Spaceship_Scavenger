using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class MainCoreState : State
    {
        public MainCoreState(StateController controller) : base(controller)
        {
            directories.Add("core", new CoreState(controller));
        }

        public override void OnEnter()
        {
            stateController.AddText("System directories:<BR><BR>--- core");
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

