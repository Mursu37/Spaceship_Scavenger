using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class SecurityShipmentMainState : State
    {
        public SecurityShipmentMainState(StateController controller) : base(controller)
        {
            directories.Add("door_control", new SecurityShipmentSecurityState(controller));
            directories.Add("logview", new SecurityShipmentLogState(controller));
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

