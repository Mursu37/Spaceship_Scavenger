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
            stateController.ChangeFlavourText("Core Systems Directory Instructions <br>"+
                "<br> systems running on <color=red> auxiliary power <color=#008000>"+
                "<br>to access further containment core systems. Link main power coupling manually below the reactor station"+
                "<br>"+
                "<br>type 'download' to download main power coupling link blueprints");
            stateController.AddText("Core Systems Directory." +
                "<BR> Auxiliary power mode on" +
                "<BR> Please connect power coupling with the main Reactor.");
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
