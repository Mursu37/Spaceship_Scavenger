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
            commands.Insert(0, "that");
        }

        public override void OnEnter()
        {
            stateController.ChangeFlavourText("DIRECTORY: /maintenance_shaft/\r\n" +
                "--------------------------------------------------------------------\r\n" +
                "Welcome to Shaft Access Terminal.\r\n" +
                "Status: Blocked.Heavy security protocols in place.\r\n\r\n" +
                "* Core Movement Restriction: Authorization required.\r\n" +
                "* Maintenance teleporter offline for unauthorized access.");
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

