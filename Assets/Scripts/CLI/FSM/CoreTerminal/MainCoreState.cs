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
            directories.Add("downloads", new DownloadState(controller));
        }

        public override void OnEnter()
        {
            stateController.ChangeFlavourText("Containment Core Reactor Station -- Systems Interface <BR><BR> Containment Core -- Status: Stabile<BR>Systems Running on Auxiliary Power.");
            base.OnEnter();
        }

        public override void Interpret(string command)
        {
            if (command == "help")
            {
                stateController.ChangeText("Navigate to directoriesby typing ['directory_name'] for further commands. Use command 'Instructions' to navigate to instructions page.");

            }

            else
            {
                base.Interpret(command);
            }


        }
    }
}

