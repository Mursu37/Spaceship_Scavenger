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

        public override void OnEnter()
        {
            stateController.AddText("System directories:<BR>-- core<BR><BR>downloadable files:<BR>power_bp");
            base.OnEnter();
        }

        public override void Interpret(string command)
        {
            if (command == "help")
            {
                stateController.ChangeText("Available commands: <BR> cd [directory_name] --- change directory<BR> dl [file_name] --- download file");

            }

            else if (command == "dl")
            { 
                if (command.Length > 1)
                {
                    if (command == "power_bp")
                    {
                        EventDispatcher dispatcher;
                        dispatcher = stateController.gameObject.GetComponent<EventDispatcher>();
                        dispatcher.TriggerEvent();

                        stateController.AddText("downloaded file: power_bp<BR><BR>Schematics added to user scanner.");
                    }
                    else
                    {
                        stateController.AddText("file '" + command +"' could not be found.");
                    }
                }

            }

            else
            {
                base.Interpret(command);
            }


        }
    }
}

