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
            commands.Insert(0, "sc-09");
            commands.Insert(1, "sc-32");
        }

        public override void OnEnter()
        {
            base.OnEnter();
            stateController.ChangeFlavourText("Log entries");
            stateController.AddText("Select log entry to view.");
        }

        public override void Interpret(string command)
        {
            if (command == "sc-09")
            {
                stateController.ChangeFlavourText("[Log Entry 9: 18:35 - SHIPTIME] \r\n" +
                    "The canteen is useless now. Food synthesizers are locked down, and the mess hall's full of empty trays. I keep hearing laughter when no one else is here.\r\n" +
                    "-- Crew Member Hal");
            }

            else if (command == "sc-32")
            {
                stateController.ChangeFlavourText("[Log Entry 32: 08:17 - SHIPTIME] \r\n" +
                    "Something isn’t right here. I sit at my station, but every time I look at the monitors, they vanish. Not the usual \"offline\" sort of thing — they’re just gone. But when I glance out of the corner of my eye, I can see them glowing, as if taunting me." +
                    "At first, I thought it was exhaustion. Now I’m not so sure. Is it a trick of the light? Or worse, is it the ship itself playing games? If this keeps up, I’ll be logging reports blindly.\r\n" +
                    "-- J. Schrödinger | Customs Inspection Officer");
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
