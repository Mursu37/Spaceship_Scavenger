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
            defaultState = new SecurityState(this);
            stateHistory = new List<State> { defaultState };
            defaultDirName = "Security_Shipments_Operations";
            currentState = defaultState;
        }
    }
}

