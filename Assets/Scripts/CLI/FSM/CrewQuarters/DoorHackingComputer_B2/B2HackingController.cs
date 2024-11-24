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
            defaultState = new SecurityState(this);
            stateHistory = new List<State> { defaultState };
            currentState = defaultState;
        }
    }
}

