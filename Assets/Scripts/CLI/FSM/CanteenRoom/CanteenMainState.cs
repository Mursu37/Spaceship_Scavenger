using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class CanteenMainState : State
    {
        public CanteenMainState(StateController controller) : base(controller)
        {
            directories.Add("door_control", new CanteenSecurityState(controller));
            directories.Add("logview", new CanteenLogState(controller));
        }

        public override void OnEnter()
        {
            stateController.ChangeFlavourText("DIRECTORY: /ship_customs/\r\n" +
                "--------------------------------------------------------------------\r\n" +
                "Welcome to Customs Entry Terminal.\r\n" +
                "Status: Monitoring all personnel movement.\r\n\r\n" +
                "* Scanning for unauthorized items.\r\n" +
                "*Report anomalies to Security Command immediately.  ");

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

