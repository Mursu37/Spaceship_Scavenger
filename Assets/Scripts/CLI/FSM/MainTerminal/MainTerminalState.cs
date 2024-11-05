using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class MainTerminalState : State
    {
        public MainTerminalState(StateController controller) : base(controller)
        {
        }

        public override void Interpret(string[] command)
        {
            if (command[0] == "poweron")
            {
                Debug.Log("Power on.");
            }
            else
            {
                base.Interpret(command);
            }
        }
    }
}
