using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class A1HackingController : StateController
    {
        private void Awake()
        {
            defaultState = new A1MainState(this);
            stateHistory = new List<State> { defaultState };
            defaultDirName = "CQ-12A";
            currentState = defaultState;
        }
    }
}

