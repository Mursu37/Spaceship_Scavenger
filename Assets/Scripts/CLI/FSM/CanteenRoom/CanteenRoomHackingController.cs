using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class CanteenRoomHackingController : StateController
    {
        private void Awake()
        {
            defaultState = new SecurityState(this);
            stateHistory = new List<State> { defaultState };
            defaultDirName = "Canteen_Terminal_Access";
            currentState = defaultState;
        }
    }
}

