using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class BlueprintState : State
    {
        public BlueprintState(StateController controller) : base(controller)
        {
            commands.Insert(0, "download_file");
        }

        public override void OnEnter()
        {

            stateController.ChangeText("Directories:<BR><BR> blueprints");
            base.OnEnter();
        }
        public override void Interpret(string command)
        {
            if (command == "download_file")
            {
                EventDispatcher dispatcher = stateController.gameObject.GetComponent<DownloadEventDispatcher>();
                if (dispatcher != null)
                {
                    dispatcher.TriggerEvent();
                }
            }

            base.Interpret(command);
        }
    }
}

