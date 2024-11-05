using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class MainCoreState : State
    {
        public MainCoreState(StateController controller) : base(controller)
        {
            directories.Add("core", new CoreState(controller));
        }
    }
}

