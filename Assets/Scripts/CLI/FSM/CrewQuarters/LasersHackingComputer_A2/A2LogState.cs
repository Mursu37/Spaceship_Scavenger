using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CLI.FSM
{
    public class A2LogState : State
    {
        public A2LogState(StateController controller) : base(controller)
        {
            commands.Insert(0, "ma-12");
        }

        public override void OnEnter()
        {
            base.OnEnter();
            stateController.ChangeFlavourText("Log entries");
            stateController.AddText("Select log entry to view.");
        }

        public override void Interpret(string command)
        {
            if (command == "ma-12")
            {
                stateController.ChangeFlavourText("[Log Entry 12: 08:10 - SHIPTIME] \r\n" +
                    "Security lasers activated after the last breach attempt. Someone keeps tampering with the access points. Whoever it is, they're skilled. Or desperate. Either way, don’t cross alone.\r\n" +
                    "-- Senior Technician Mira");
            }
            else if (command == "help")
            {
                stateController.AddText("select a log entry to open by typing 'view [Log_ID]'.");
            }
            else
            {
                base.Interpret(command);
            }
        }
    }
}
