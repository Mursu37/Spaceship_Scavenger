using System;
using UnityEngine;

namespace CLI.FSM.ExampleScene
{
    public class SimpleStateController : StateController
    {
        public SimpleStateController()
        {
            defaultState = new SimpleState(this);
            stateHistory.Add(defaultState);
            currentState = defaultState;
            dirName = "OP_system";
        }
    }
}

