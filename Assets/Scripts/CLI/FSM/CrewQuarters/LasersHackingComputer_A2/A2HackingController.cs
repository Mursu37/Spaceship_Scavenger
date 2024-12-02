using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class A2HackingController : StateController
    {
        private void Awake()
        {
            defaultState = new A2MainState(this);
            stateHistory = new List<State> { defaultState };
            currentState = defaultState;
        }
    }
}

