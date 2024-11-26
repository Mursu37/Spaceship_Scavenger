using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class DownloadState : State
    {
        public DownloadState(StateController controller) : base(controller)
        {
            directories.Add("blueprints", new BlueprintState(controller));
        }

        public override void OnEnter()
        {

            stateController.ChangeText("Downloadable Files:<BR><BR> blueprints");
            base.OnEnter();
        }

    }
}

