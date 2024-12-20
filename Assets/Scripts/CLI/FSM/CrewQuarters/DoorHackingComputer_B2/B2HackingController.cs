using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class B2HackingController : StateController
    {
        private void Awake()
        {
            defaultState = new B2MainState(this);
            stateHistory = new List<State> { defaultState };
            defaultDirName = "Storage_Bay_Terminal";
            currentState = defaultState;
        }
    }
}

