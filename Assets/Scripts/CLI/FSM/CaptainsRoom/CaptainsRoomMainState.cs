using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class CaptainsRoomMainState : State
    {
        private CaptainsRoomSecurityState securityState;
        private bool breached = false;
        private StateController _stateController;

        public CaptainsRoomMainState(StateController controller) : base(controller)
        {
            _stateController = controller;
            securityState = new CaptainsRoomSecurityState(_stateController);
            if (!breached)
            {
                directories.Add("[breach]", securityState);
            }
            else
            {
                directories.Add("personal_logs", securityState);
            }
        }

        public override void OnEnter()
        {
            stateController.UpdateCommands();

            stateController.ChangeFlavourText("DIRECTORY: /captains_quarters/\r\n" +
                "--------------------------------------------------------------------\r\n" +
                "Captain’s Command Terminal.\r\n" +
                "Status: Secure Access Required.");

            base.OnEnter();
        }

        public override void Interpret(string command)
        {
            if (command == "help")
            {
                stateController.ChangeText("HELP - Available Commands: \r\n" +
                    "Secure Access required\r\n" +
                    "Please provide access credentials."
                    );
            }

            else if (!breached && command == "[breach]" || !breached && command == "breach")
            {
                stateController.ChangeText("<color=#3Ca8a8>run [VOIDHAUL_MODULE_BREACH.exe]:</color>");
                stateController.AddText("<line-height=0>========================================================", 0, true, () =>
                {
                    stateController.AddText("<color=#3Ca8a8><line-height=2em>■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■</line-height>", 0.05f, true, () =>
                    {
                        directories.Remove("[breach]");
                        directories.Add("personal_logs", securityState);
                        stateController.UpdateCommands();
                        stateController.ChangeDeeper(securityState, "personal_logs");
                        breached = true;
                    });
                });
            }
            else
            {
                base.Interpret(command);
            }
        }
    }
}

