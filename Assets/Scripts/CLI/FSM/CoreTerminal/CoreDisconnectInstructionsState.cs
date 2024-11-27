using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CLI.FSM
{
    public class CoreDisconnectInstructionsState : State
    {
        public CoreDisconnectInstructionsState(StateController controller) : base(controller)
        {
            commands.Insert(0, "download");
        }

        public override void OnEnter()
        {
            base.OnEnter();
            stateController.ChangeFlavourText("<color=cyan>Core Systems Directory</color>" +
            "<BR><BR><color=red>Alert:</color> The spaceship is operating on <color=orange>low auxiliary power.</color>" +
            "<BR>Critical systems are offline." +  
            "<BR><BR>To access the <color=#00ff00>Containment Core Control Panel</color>, reconnect power to the main reactor." +
            "<BR><BR><b>Instructions:</b>" +  
            "<BR>1. Manually link the main power coupling below the reactor station." +  
            "<BR>2. Ensure primary systems are routed through the main reactor." +  
            "<BR><BR>Type <color=yellow>'download'</color> to retrieve the <color=#00ff00>power routing schematics.</color>");
            stateController.AddText("Core Systems Directory." +
                "<BR>Low auxiliary power mode on" +
                "<BR>Please connect power coupling with the main Reactor.");
        }

        public override void Interpret(string command)
        {
            if (command == "download")
            {

            }
            else
            {
                base.Interpret(command);
            }
            
        }
    }
}
