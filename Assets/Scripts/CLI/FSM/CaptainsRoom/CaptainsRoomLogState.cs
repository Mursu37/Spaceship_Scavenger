using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CLI.FSM
{
    public class CaptainsRoomLogState : State
    {
        public CaptainsRoomLogState(StateController controller) : base(controller)
        {
            commands.Insert(0, "ct-07");
            commands.Insert(1, "ct-12");
        }

        public override void OnEnter()
        {
            base.OnEnter();
            stateController.ChangeFlavourText("Log entries");
            stateController.AddText("Select log entry to view.");
        }

        public override void Interpret(string command)
        {
            if (command == "ct-07")
            {
                stateController.ChangeFlavourText("[LOG ENTRY 7: 22:15 - SHIPTIME]\r\n" +
                    "------------------------------------------------------------\r\n" +
                    "Voidhaul's directives are clear: \"The core always comes first.\"\r\n" +
                    "But at what cost? These new events showed us how fragile our control really is.\r\n" +
                    "My crew trusts me to lead, but I’ve seen what happens when\r\n" +
                    "containment falters — entire ships reduced to drifting tombs.\r\n" +
                    "Corporate doesn’t care about the lives aboard this vessel.\r\n" +
                    "Every decision I make to protect the crew feels like a rebellion against them.\r\n" +
                    "The reactor’s instability grows worse. If we don’t stabilize the core soon,\r\n" +
                    "it won’t matter what Voidhaul wants. We’ll all be lost to the void.\r\n" +
                    "-- Captain Alaric Soren");
            }
            if (command == "ct-12")
            {
                stateController.ChangeFlavourText("[LOG ENTRY 12: 03:42 - SHIPTIME] \r\n" +
                    "Something is wrong with the containment core. There wasn’t supposed to\r\n" +
                    "be any lasting effects of the strange hum, but the core... it feels different now.\r\n" +
                    "I don’t know how else to describe it. Corporate keeps demanding progress reports,\r\n" +
                    "but they ignore the warnings. “Extract the core,” they say, as if it’s that simple.\r\n" +
                    "If we force it out now, this ship won’t survive. Neither will we.\r\n" +
                    "I’ve issued an emergency lockdown on the core room. My orders won’t sit well\r\n" +
                    "with Voidhaul, but I’d rather face their wrath than let the core take us all.\r\n");
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
