using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class CoreStateController : StateController
    {
        private void Awake()
        {
            defaultState = new MainCoreState(this);
            stateHistory = new List<State> { defaultState };
            defaultDirName = "Reactor_Station_Systems";
            currentState = defaultState;
        }
    }
}

