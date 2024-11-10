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

        public override void OnEnter()
        {
            stateController.AddText("Core Systems Directory. <BR> Auxiliary power mode on <BR> Please connect power coupling with the main Reactor.");
            base.OnEnter();
        }

        public override void Interpret(string[] command)
        {
            if (command[0] == "disconnect")
            {
                if (GameObject.Find("PowerOnSwitch").gameObject.transform.GetChild(0).GetComponent<ShipPowerOn>().isPowerOn)
                {
                    GameObject.Find("CoreComputer").GetComponent<MeltdownPhase>().enabled = true;
                    
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
