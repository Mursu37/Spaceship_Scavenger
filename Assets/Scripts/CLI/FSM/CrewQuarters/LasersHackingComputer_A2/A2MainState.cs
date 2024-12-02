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
            stateController.ChangeFlavourText("DIRECTORY: /maintenance/\r\n" +
                "--------------------------------------------------------------------\r\n" +
                "Welcome to Maintenance Terminal #045.\r\n" +
                "Status: Active for system diagnostics and tools access.\r\n" +
                "Ensure all safety protocols are followed when handling equipment.\r\n\r\n" +
                "* Note: Security lasers are currently active. Exercise caution.");

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

