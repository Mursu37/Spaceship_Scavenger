using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CLI.FSM
{
    public class CoreState : State
    {
        public CoreState(StateController controller) : base(controller)
        {
        }

        public override void Interpret(string[] command)
        {
            if (command[0] == "disconnect")
            {
                if (GameObject.Find("CoreComputer").GetComponent<ShipPowerOn>().isPowerOn)
                {
                    GameObject.Find("CoreComputer").GetComponent<ShipPowerOn>().turnShipOn();
                }
                else
                {
                    stateController.ChangeText("This computer requires the main power in order to disconnect the Containemnt Core");
                }
            }
            else
            {
                base.Interpret(command);
            }
        }
    }
}
