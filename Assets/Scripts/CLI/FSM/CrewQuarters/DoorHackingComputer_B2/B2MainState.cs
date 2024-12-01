using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class B2MainState : State
    {
        public B2MainState(StateController controller) : base(controller)
        {
            directories.Add("door_control", new B2SecurityState(controller));
            directories.Add("logview", new B2LogState(controller));
        }

        public override void OnEnter()
        {
            stateController.ChangeFlavourText("DIRECTORY: /storage_space/\r\n" +
                "--------------------------------------------------------------------\r\n" +
                "Welcome to Storage Access Terminal.\r\n" +
                "Status: Limited Access.\r\n\r\n" +
                "* Active Monitors: Security feed operational.\r\n" +
                "* Storage Units: Sealed.Contact supervisor for access codes.");

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

