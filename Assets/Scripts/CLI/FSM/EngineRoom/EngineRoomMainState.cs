using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class EngineRoomMainState : State
    {
        public EngineRoomMainState(StateController controller) : base(controller)
        {
            directories.Add("door_control", new EngineRoomSecurityState(controller));
            directories.Add("logview", new EngineRoomLogState(controller));
        }

        public override void OnEnter()
        {
            stateController.ChangeFlavourText("DIRECTORY: /engine_room/\r\n" +
                "--------------------------------------------------------------------\r\n" +
                "Welcome to Engine Systems Terminal.\r\n" +
                "Status: Systems stable.Auxiliary power routing active.\r\n\r\n" +
                "* Reactor Operations: Restricted to authorized personnel.\r\n" +
                "* Warning: Pressure anomalies detected in secondary piping.");

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

