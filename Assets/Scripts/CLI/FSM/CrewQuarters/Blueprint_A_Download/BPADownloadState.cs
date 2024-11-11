using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CLI.FSM
{
    public class BPADownloadState : State
    {
        public BPADownloadState(StateController controller) : base(controller)
        {
            
        }

        public override void OnEnter()
        {

            stateController.AddText("Downloadable files:<BR><BR>--- ventilation");
            base.OnEnter();
        }

        public override void Interpret(string[] command)
        {
            if (command[0] == "help")
            {
                stateController.AddText("Available commands: <BR> cd [directory_name] --- change directory <BR> dl [file_name] --- download file");
            }

            else if (command[0] == "dl")
            {
                if (command.Length > 1)
                {
                    if (command[1] == "ventilation")
                    {
                        EventDispatcher dispatcher;
                        dispatcher = stateController.gameObject.GetComponent<EventDispatcher>();
                        dispatcher.TriggerEvent();

                        stateController.AddText("downloading ventilation schematics");
                    }

                    else
                    {
                        stateController.AddText("could not find file \"" + command[1] + "\"");
                    }
                }
                else
                {
                    base.Interpret(command);
                }
            }
            else
            {
                base.Interpret(command);
            }
        }
    }
}
