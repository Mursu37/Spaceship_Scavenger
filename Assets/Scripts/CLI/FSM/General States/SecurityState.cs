using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class SecurityState : State
    {
        public SecurityState(StateController controller) : base(controller)
        {
            directories.Add("door_control", new SecurityState_D(controller));
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

    }
}

