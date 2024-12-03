using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CLI.FSM
{
    public class CaptainsRoomSecurityState : State
    {
        private bool query = false;

        public CaptainsRoomSecurityState(StateController controller) : base(controller)
        {
            directories.Add("logview", new CaptainsRoomLogState(controller));
            commands.Insert(1, "download credentials");
        }

        public override void OnEnter()
        {
            query = false;

            base.OnEnter();
            stateController.ChangeFlavourText("<align=left>DIRECTORY: " + stateController.GetCurrentStateDirectoryText() + "\r\n" +
                "<align=flush>-----------------------------------------------------------</align>\r\n" +
                "<align=left>> ACCESS_CONTROL.dll<line-height=0>\r\n" + //left
                "<align=right>Manage connected door systems.<line-height=1em>\r\n" + //right
                                                                                     // "<align=left>> EXECUTE_LOCKDOWN.exe<line-height=0>\r\n" + //left
                                                                                     // "<align=right>Trigger lockdown on doors.<line-height=1em>\r\n" + //right
                "<align=left>> EVENT_LOG.dat<line-height=0>\r\n" + //left
                "<align=right>Activity log for door access events.<line-height=1em>\r\n" + //right
                "<align=left>> SYSTEM_DIAGNOSTIC.exe<line-height=0>\r\n" + //left
                "<align=right>Run a diagnostic of the door control system.<line-height=1em>\r\n" + //right
                "<align=flush>-----------------------------------------------------------</align>\r\n" +
                "<align=left>* Unauthorized access detected. <color=#3Ca8a8>[Emergency lockout lifted.]</color>");

            //stateController.ChangeText("Changed directory: " + stateController.GetCurrentStateDirectoryText());

        }

        public override void Interpret(string command)
        {
            query = false;
            RemoveGlobalQueryCommands();

            stateController.UpdateCommands();

            if (command == "download credentials")
            {
                stateController.ChangeText("Download authorization access credentials? Y/N");

                query = true;
                commands.Add("yes");
                commands.Add("no");
                stateController.UpdateCommands();

            }
            else if (command == "help")
            {
                stateController.ChangeText("HELP - Available Commands:\r\n" +
                    "logview - access logs\r\n" +
                    "download credentials - download access credentials");
            }
            else if (query == true && command == "yes" || command == "y")
            {
                stateController.ChangeText("Downloading authorization access credentials... Download complete.");
            }
            else if (query == true && command == "no" || command == "n")
            {
                stateController.ChangeText("Authorization access credentials download aborted.");
            }
            else
            {
                base.Interpret(command);
            }
            

        }
    }
}
