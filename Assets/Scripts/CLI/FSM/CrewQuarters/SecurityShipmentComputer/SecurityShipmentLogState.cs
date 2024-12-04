using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CLI.FSM
{
    public class SecurityShipmentLogState : State
    {
        public SecurityShipmentLogState(StateController controller) : base(controller)
        {
            commands.Insert(0, "mc-05");
        }

        public override void OnEnter()
        {
            base.OnEnter();
            stateController.ChangeFlavourText("Log entries");
            stateController.AddText("Select log entry to view.");
        }

        public override void Interpret(string command)
        {
            if (command == "mc-05")
            {
                stateController.ChangeFlavourText("[Log Entry 5: 14:50 - SHIPTIME] \r\n" +
                    "The security lock is in place. Nobody’s teleporting the core out without my clearance. Not after what happened in Bay 3. If anyone tries, the alarms will fry their ears. \r\n" +
                    "-- Systems Admin Karsen  ");
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
