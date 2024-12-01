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
            defaultState = new CanteenMainState(this);
            stateHistory = new List<State> { defaultState };
            defaultDirName = "Ship_Customs_Terminal";
            currentState = defaultState;
        }
    }
}

