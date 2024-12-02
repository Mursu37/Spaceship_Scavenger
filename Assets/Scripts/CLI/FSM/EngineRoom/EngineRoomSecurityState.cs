using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CLI.FSM
{
    public class EngineRoomSecurityState : State
    {
        private bool query = false;

        public EngineRoomSecurityState(StateController controller) : base(controller)
        {
            directories.Add("access", new EngineRoomDoorControlState(controller));
            commands.Insert(0, "log");
            commands.Insert(1, "run diagnostics");
        }

        public override void OnEnter()
        {
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
            if (commands.Contains("yes"))
            {
                commands.Remove("yes");
            }
            if (commands.Contains("no"))
            {
                commands.Remove("no");
            }

            stateController.UpdateCommands();

            /*   if (command == "access")
               {
                   stateController.ChangeText("> access DOOR_03A\r\n" +
                       "<color=#0a6310>DOOR_03A unlocked. Proceed with caution.</color>");
               }*/
            /* else if (command == "lockdown")
             {
                 stateController.ChangeText("> lockdown\r\n" +
                     "<color=#c8a519>All access points secured. Authorization required to lift lockdown.</color>");
             } */
            /*  else if (command == "override")
              {
                  stateController.ChangeText("> lockdown\r\n" +
                      "<color=#c8a519>OVERRIDE_PROTOCOL initiated.\r\n" +
                      "Access permissions bypassed.Manual control granted.\r\n" +
                      "You may now execute restricted commands.</color>");
              }*/
            if (command == "log")
            {
                stateController.ChangeText("[09:32:21] - Unauthorized breach attempt detected at Door B07.\r\n" +
                    "[09:35:03] - Manual lock-down triggered by security officer. All internal doors sealed.\r\n" +
                    "[11:00:08] - Breach attempt detected at Door B07. Personnel deployed to reinforce locks manually.\r\n" +
                    "[11:23:11] - Security systems reset. Physical locks holding.");
            }
            else if (command == "run diagnostics")
            {
                stateController.ChangeText("SYSTEM_DIAGNOSTIC.exe executed.\r\n" +
                    "Analyzing door control system... 100 % complete. " + "Results:\r\n" +
                    "- Door A12: Operational..\r\n" +
                    "- Door B07: Disconnected from terminal.\r\n" +
                    "<align=left><color=#c8a519>Warning: Door B07 is not responding to control commands. Manual Intervention required.<line-height=0>\r\n" +
                    "<align=right><color=#c8a519> Download report schematics? Y/N<line-height=1em>");

                query = true;
                commands.Add("yes");
                commands.Add("no");
                stateController.UpdateCommands();

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
