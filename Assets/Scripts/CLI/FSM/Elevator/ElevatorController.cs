using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class ElevatorController : StateController
    {
        private void Awake()
        {
            defaultState = new ElevatorMainState(this);
            stateHistory = new List<State> { defaultState };
            defaultDirName = "Engine_Room_Operations";
            currentState = defaultState;
        }
    }
}

