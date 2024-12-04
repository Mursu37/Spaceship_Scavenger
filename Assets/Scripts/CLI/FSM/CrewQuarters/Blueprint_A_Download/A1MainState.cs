using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class A1MainState : State
    {
        private A1DownloadState downloadState;

        public A1MainState(StateController controller) : base(controller)
        {
            downloadState = new A1DownloadState(controller);

            directories.Add("door_control", new A1SecurityState(controller));
            directories.Add("schematics", downloadState);
            directories.Add("logview", new A1LogState(controller));
        }

        public override void OnEnter()
        {
            stateController.ChangeFlavourText("DIRECTORY: /crew_quarters/lounge/\r\n" +
                "--------------------------------------------------------------------\r\n" +
                "Welcome to CQ-12A Lounge Terminal.\r\n" +
                "Status: Active for crew utilities.\r\n\r\n" +
                "Ventilation Notice: Recent activity detected in shaft access.\r\n" +
                "Report all unusual behavior to security immediately.");

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

