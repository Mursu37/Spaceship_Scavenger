using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class VentilationShaftsController : StateController
    {
        private void Awake()
        {
            defaultState = new DownloadState(this);
            stateHistory = new List<State> { defaultState };
            defaultDirName = "Ventilation_Systems";
            currentState = defaultState;
        }
    }
}

