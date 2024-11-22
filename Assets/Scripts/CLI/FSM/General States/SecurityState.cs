using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class SecurityState : State
    {
        public SecurityState(StateController controller) : base(controller)
        {
            directories.Add("doors", new DoorState(controller));
        }

        public override void OnEnter()
        {

            stateController.ChangeText("security systems:<BR><BR>- doors");
            base.OnEnter();
        }

    }
}

