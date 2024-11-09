using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class BPAController : StateController
    {
        private void Awake()
        {
            defaultState = new BPAMainState(this);
            stateHistory = new List<State> { defaultState };
            currentState = defaultState;
        }
    }
}

