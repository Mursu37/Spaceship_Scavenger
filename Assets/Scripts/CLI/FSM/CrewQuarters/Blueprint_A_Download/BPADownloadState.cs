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

            stateController.AddText("System directories:<BR><BR>--- ship_blueprints_a.obpp");
            base.OnEnter();
        }

        public override void Interpret(string[] command)
        {
            if (command[0] == "help")
            {
                stateController.AddText("Available commands: <BR> cd [directory_name] --- change directory <BR> download [file_name] --- download selected file");
            }

            else if (command[0] == "download")
            {
                if (command.Length > 1)
                {
                    if (command[1] == "ship_blueprint_a.obpp")
                    {
                        stateController.AddText("downloading ship_blueprint_a.obpp");
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
