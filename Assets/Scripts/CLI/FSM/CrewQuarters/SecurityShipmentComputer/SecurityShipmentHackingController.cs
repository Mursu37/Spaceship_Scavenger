using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class SecurityShipmentHackingController : StateController
    {
        private void Awake()
        {
            defaultState = new SecurityShipmentMainState(this);
            stateHistory = new List<State> { defaultState };
            defaultDirName = "Maintenance_checkpoint_terminal";
            currentState = defaultState;
        }
    }
}

