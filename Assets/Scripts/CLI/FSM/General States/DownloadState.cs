using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class DownloadState : State
    {
        public DownloadState(StateController controller) : base(controller)
        {
            directories.Add("blueprints", new DoorState(controller));
        }

        public override void OnEnter()
        {

            stateController.ChangeText("Directories:<BR><BR> blueprints");
            base.OnEnter();
        }

    }
}

