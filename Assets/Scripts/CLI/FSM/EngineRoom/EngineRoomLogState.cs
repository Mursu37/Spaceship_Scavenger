using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CLI.FSM
{
    public class EngineRoomLogState : State
    {
        public EngineRoomLogState(StateController controller) : base(controller)
        {
            commands.Insert(0, "er-15");
            commands.Insert(1, "er-16");
        }

        public override void OnEnter()
        {
            base.OnEnter();
            stateController.ChangeFlavourText("Log entries");
            stateController.AddText("Select log entry to view.");
        }

        public override void Interpret(string command)
        {
            if (command == "er-15")
            {
                stateController.ChangeFlavourText("[LOG ENTRY 15: 10:45 - SHIPTIME]\r\n" +
                    "------------------------------------------------------------\r\n" +
                    "With the ship on auxiliary power, all critical doors are sealed\r\n" +
                    "with reinforced alloy plating. These can withstand plasma fire.\r\n" +
                    "Manual override is offline until the reactor is restored.\r\n \r\n" +
                    "Explosives may breach the plating, so we should make sure to secure and lock the storages containing volatile material\r\n" +
                    "I wouldn't want them to get any crazy ideas.\r\n \r\n" +
                    "--  Officer Cedric Halvorsen | Engine Room Security");
            }
            if (command == "er-16")
            {
                stateController.ChangeFlavourText("[Log Entry 17: 09:20 - SHIPTIME] \r\n" +
                    "The doors are reinforced, but the sabotage won't stop. I found coolant lines severed and the backup thruster sabotaged. If anyone's left alive, check the reactor seals — if they go, we're done. \r\n" +
                    "-- Officer Cedric Halvorsen | Engine Room Security");
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
