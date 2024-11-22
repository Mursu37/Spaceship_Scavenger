using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class B2MainState : State
    {
        public B2MainState(StateController controller) : base(controller)
        {
            directories.Add("b2_security", new B2SecurityState(controller));
        }

        public override void OnEnter()
        {

            stateController.ChangeText("System directories:<BR><BR>--- b2_security");
            base.OnEnter();
        }

        public override void Interpret(string command)
        {
            if (command == "help")
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

