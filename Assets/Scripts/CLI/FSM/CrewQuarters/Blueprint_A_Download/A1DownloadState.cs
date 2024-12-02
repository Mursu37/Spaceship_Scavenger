using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CLI.FSM
{
    public class A1DownloadState : State
    {
        public A1DownloadState(StateController controller) : base(controller)
        {
            
        }

        public override void OnEnter()
        {

            stateController.AddText("Downloadable files:<BR><BR>--- ventilation");
            base.OnEnter();
        }

        public override void Interpret(string command)
        {
            if (command == "help")
            {
                stateController.AddText("Available commands: <BR> cd [directory_name] --- change directory <BR> dl [file_name] --- download file");
            }

            else if (command == "dl")
            {
                if (command.Length > 1)
                {
                    if (command == "ventilation")
                    {
                        EventDispatcher dispatcher;
                        dispatcher = stateController.gameObject.GetComponent<EventDispatcher>();
                        dispatcher.TriggerEvent();

                        stateController.AddText("downloading ventilation schematics");
                    }

                    else
                    {
                        stateController.AddText("could not find file \"" + command + "\"");
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
