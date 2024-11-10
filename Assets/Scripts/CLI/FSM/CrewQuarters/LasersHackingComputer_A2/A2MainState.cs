using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class A2MainState : State
    {
        public A2MainState(StateController controller) : base(controller)
        {
            directories.Add("a2_security", new A2SecurityState(controller));
        }

        public override void OnEnter()
        {

            stateController.ChangeText("System directories:<BR><BR>--- a2_security");
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

