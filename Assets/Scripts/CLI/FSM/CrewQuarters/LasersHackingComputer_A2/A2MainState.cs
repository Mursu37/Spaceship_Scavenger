using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class A2MainState : State
    {
        public A2MainState(StateController controller) : base(controller)
        {
            directories.Add("laser_control", new A2SecurityState(controller));
            directories.Add("logview", new A2LogState(controller));
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void Interpret(string command)
        {
            if (command == "help")
            {
                stateController.ChangeText("HELP - Available Commands: \r\n" +
                    "Door_control - change directory to door_control. \r\n" +
                    "logview - view personnel log entries."
                    );
            }

            base.Interpret(command);
        }
    }
}

