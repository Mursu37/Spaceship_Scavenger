using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CLI.FSM
{
    public class CanteenLogState : State
    {
        public CanteenLogState(StateController controller) : base(controller)
        {
            commands.Insert(0, "er-09");
        }

        public override void OnEnter()
        {
            base.OnEnter();
            stateController.ChangeFlavourText("Log entries");
            stateController.AddText("Select log entry to view.");
        }

        public override void Interpret(string command)
        {
            if (command == "er-09")
            {
                stateController.ChangeFlavourText("[Log Entry 9: 18:35 - SHIPTIME] \r\n" +
                    "The canteen is useless now. Food synthesizers are locked down, and the mess hall's full of empty trays. I keep hearing laughter when no one else is here.\r\n" +
                    "-- Crew Member Hal");
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
