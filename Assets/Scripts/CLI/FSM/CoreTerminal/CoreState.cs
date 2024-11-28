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
            directories.Add("instructions", new CoreDisconnectInstructionsState(controller));
            commands.Insert(0, "disconnect_protocol");
        }

        public override void OnEnter()
        {
            stateController.ChangeFlavourText("Core Systems Directory. " +
                "<BR><BR> Auxiliary power mode on <BR> Please connect power coupling with the main Reactor.");
            stateController.AddText("Core Systems Directory. " +
                "<BR> Auxiliary power mode on <BR> Please connect power coupling with the main Reactor.");
            base.OnEnter();
        }

        public override void Interpret(string command)
        {
            if (command == "disconnect_protocol")
            {
                if (GameObject.Find("PowerOnSwitch").GetComponent<ShipPowerOn>().isPowerOn)
                {
                    EventDispatcher dispatcher;
                    dispatcher = stateController.gameObject.GetComponent<CoreEventDispatcher>();
                    dispatcher.TriggerEvent();

                    stateController.ChangeFlavourText("Containment Core disconnected." +
                        "<BR> Containment Core released<BR><BR>>Downloaded System data to scanner" +
                        "<BR><color=red>---<BR>UNAUTHORIZED ACCESS DETECTED<BR>---" +
                        "<BR>CAUTION: Security systems activated. Alarm State raised. Containment Core meltdown imminent. Threat assessment lockdown in progress.<color=#008000>");
                }
                else
                {
                    stateController.ChangeText("This computer requires the main power coupling to be manually linked in order to disconnect the Containemnt Core");
                }
            }
            else
            {
                base.Interpret(command);
            }
        }
    }
}
