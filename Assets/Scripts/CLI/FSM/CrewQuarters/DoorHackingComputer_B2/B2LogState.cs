using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CLI.FSM
{
    public class B2LogState : State
    {
        public B2LogState(StateController controller) : base(controller)
        {
            commands.Insert(0, "sb-10");
        }

        public override void OnEnter()
        {
            base.OnEnter();
            stateController.ChangeFlavourText("Log entries");
            stateController.AddText("Select log entry to view.");
        }

        public override void Interpret(string command)
        {
            if (command == "sb-10")
            {
                stateController.ChangeFlavourText("[Log Entry 10: 16:05 - SHIPTIME]\r\n" +
                    "Storage monitors are looping footage.Can't tell if it's real or not.Someone—or something—is moving in the aisles.Avoid opening crates without scanning first.\r\n" +
                    "-- Logistics Officer Greer  ");
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
