using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CLI.FSM
{
    public class SecurityShipmentSecurityState : State
    {
        private bool query = false;

        public SecurityShipmentSecurityState(StateController controller) : base(controller)
        {
            directories.Add("access", new SecurityShipmentDoorControlState(controller));
            commands.Insert(0, "log");
            commands.Insert(1, "run diagnostics");
        }

        public override void OnEnter()
        {
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

            base.OnEnter();

        }

        public override void Interpret(string command)
        {
            if (commands.Contains("yes"))
            {
                commands.Remove("yes");
            }
            if (commands.Contains("no"))
            {
                commands.Remove("no");
            }

            stateController.UpdateCommands();

            if (command == "log")
            {
                stateController.ChangeText("[DOOR USAGE LOG: MAINTENANCE SHAFT]\r\n" +
                    "- 14:47 SHIPTIME: Admin Karsen secured the shaft.\r\n" +
                    "- 14:50 SHIPTIME: Lockdown sequence finalized.\r\n" +
                    "- 14:58 SHIPTIME: Unauthorized teleport signal detected.\r\n");
            }
            else if (command == "run diagnostics")
            {
                stateController.ChangeText("SYSTEM_DIAGNOSTIC.exe executed.\r\n" +
                    "Analyzing door control system... 100 % complete. " + "Results:\r\n" +
                    "- Door A2: Operational..\r\n");

            }
            else if (command == "help")
            {
                stateController.ChangeText("HELP - Available Commands:\r\n" +
                    "ACCESS - Manage door systems.\r\n" +
                    // "LOCKDOWN - Trigger lockdown on doors.\r\n" +
                    // "OVERRIDE - Emergency override options.\r\n" +
                    "LOG - View door access activity log.\r\n" +
                    "RUN DIAGNOSTICS - Run a system diagnostic.\r\n");
                //  "* Unauthorized access detected.Emergency lockout lifted.\r\n");
            }
            else if (query == true && command == "yes")
            {
                stateController.ChangeText("Downloading system diagnostics report schematics.");
            }
            else if (query == true && command == "no")
            {
                stateController.ChangeText("Systems diagnostics report download aborted.");
            }
            else
            {
                base.Interpret(command);
            }


        }
    }
}
