using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CLI.FSM
{
    public class A1LogState : State
    {
        public A1LogState(StateController controller) : base(controller)
        {
            commands.Insert(0, "cql-14");
        }

        public override void OnEnter()
        {
            base.OnEnter();
            stateController.ChangeFlavourText("Log entries");
            stateController.AddText("Select log entry to view.");
        }

        public override void Interpret(string command)
        {
            if (command == "cql-14")
            {
                stateController.ChangeFlavourText("[Log Entry 14: 20:12 - SHIPTIME] \r\n" +
                    "The vents might be our last option, but something’s wrong. There’s this... smell. Like cheese. It's getting stronger every hour. If you’re heading into the shafts, brace yourself — it’s not just the smell that’s unsettling.\r\n" +
                    "-- Crew Member Tess  ");
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
