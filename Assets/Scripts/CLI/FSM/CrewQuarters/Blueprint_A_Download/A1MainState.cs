using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class A1MainState : State
    {
        public A1MainState(StateController controller) : base(controller)
        {
            directories.Add("door_control", new A1SecurityState(controller));
            directories.Add("Schematics", new A1DownloadState(controller));
            directories.Add("logview", new A1LogState(controller));
        }

        public override void OnEnter()
        {
            stateController.ChangeFlavourText("CQ-12A Terminal Access Point Main Directory");

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

