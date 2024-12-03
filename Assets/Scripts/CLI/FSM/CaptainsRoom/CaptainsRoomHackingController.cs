using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class CaptainsRoomHackingController : StateController
    {
        private void Awake()
        {
            defaultState = new CaptainsRoomMainState(this);
            stateHistory = new List<State> { defaultState };
            defaultDirName = "Captains_Quarters_Terminal";
            currentState = defaultState;
        }
    }
}

