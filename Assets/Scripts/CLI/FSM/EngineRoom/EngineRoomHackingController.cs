using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class EngineRoomHackingController : StateController
    {
        private void Awake()
        {
            defaultState = new EngineRoomMainState(this);
            stateHistory = new List<State> { defaultState };
            defaultDirName = "Engine_Room_Operations";
            currentState = defaultState;
        }
    }
}

