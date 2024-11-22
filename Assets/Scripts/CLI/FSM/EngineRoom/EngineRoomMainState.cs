using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class EngineRoomMainState : State
    {
        public EngineRoomMainState(StateController controller) : base(controller)
        {
            directories.Add("security", new SecurityState(controller));
            directories.Add("download", new DownloadState(controller));
        }

        public override void OnEnter()
        {

            stateController.ChangeText("system directories:<BR><BR>- security<BR>- download");
            base.OnEnter();
        }

        public override void Interpret(string command)
        {   
            base.Interpret(command);
        }
    }
}

