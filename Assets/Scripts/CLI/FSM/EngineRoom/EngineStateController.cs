using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class EngineStateController : StateController
    {
        private void Awake()
        {
            defaultState = new MainEngineState(this);
            stateHistory = new List<State> { defaultState };
            currentState = defaultState;
        }
    }
}

