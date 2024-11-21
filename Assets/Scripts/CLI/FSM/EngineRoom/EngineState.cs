using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class EngineState : State
    {
        public EngineState(StateController controller) : base(controller)
        {
        }

        public override void Interpret(string command)
        {
            if (GameObject.Find("EngineSwitch").transform.GetChild(0).GetComponent<EngineRoomSwitch>().isEngineCompuerOn)
            {
                if (command == "poweron")
                {
                    GameObject.Find("EngineComputer").GetComponent<ShipPowerOn>().turnShipOn();
                }
                else
                {
                    base.Interpret(command);
                }
            }
            else
            {
                stateController.ChangeText("This computer requires the engine room power to function.");
            }
        }
    }
}
