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
                "<align=left>> LOGVIEW<line-height=0>\r\n" + //left
                "<align=right>View logs<line-height=1em>\r\n" + //right
                                                                                     // "<align=left>> EXECUTE_LOCKDOWN.exe<line-height=0>\r\n" + //left
                                                                                     // "<align=right>Trigger lockdown on doors.<line-height=1em>\r\n" + //right
                "<align=left>> Access_Credentials<line-height=0>\r\n" + //left
                "<align=right> Authorization Credentials To Maintenance<line-height=1em>\r\n" + //right
                "<align=flush>-----------------------------------------------------------</align>\r\n" +
                "<align=left>* Unauthorized access detected. <color=#3Ca8a8>[Emergency lockout lifted.]</color>");

            //stateController.ChangeText("Changed directory: " + stateController.GetCurrentStateDirectoryText());

        }

        public override void Interpret(string command)
        {
            
            RemoveGlobalQueryCommands();

            stateController.UpdateCommands();

            if (query == true && command == "yes" || command == "y")
            {
                stateController.ChangeText("<color=#c8a519>Downloading authorization access credentials... Download complete.");
                CheckpointManager.captainsCredentialsGained = true;
                query = false;
                return;
            }
            else if (query == true && command == "no" || command == "n")
            {
                stateController.ChangeText("Authorization access credentials download aborted.");
                query = false;
                return;
            }

            query = false;


            if (command == "help")
            {
                stateController.ChangeText("HELP - Available Commands:\r\n" +
                    "logview - access logs\r\n" +
                    "download credentials - download access credentials");

            }


            else if (command == "download credentials")
            {
                stateController.ChangeText("Download authorization access credentials? Y/N");

                query = true;
                commands.Add("yes");
                commands.Add("no");
                stateController.UpdateCommands();

            }

            else
            {
                base.Interpret(command);
            }
            

        }
    }
}
