using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CLI.FSM
{
    public class CoreState : State
    {
        public CoreState(StateController controller) : base(controller)
        {
            //directories.Add("instructions", new CoreDisconnectInstructionsState(controller));
            commands.Insert(0, "run execute_protocol");
            commands.Insert(1, "decrypt authorization_codes");
            commands.Insert(2, "logview core_status");
            commands.Insert(2, "override system_override");
            commands.Insert(2, "reroute power_dispatch");
            commands.Insert(2, "traceback intrustion_alert");
        }

        public override void OnEnter()
        {
            base.OnEnter();
            stateController.ChangeFlavourText("DIRECTORY: /core_systems/disconnect_protocol/\r\n" +
                "--------------------------------------------------------------------\r\n" +
                "> EXECUTE_PROTOCOL.dll - Executable for core disconnection.\r\n" +
                "> AUTHORIZATION_CODES.dat - Encrypted file of access keys.\r\n" +
                "> CORE_STATUS.log - Contains real - time core health data.\r\n" +
                "> SYSTEM_OVERRIDES.sys - Configuration for bypassing core safety.\r\n" +
                "> POWER_REDISPATCH.cfg - File for rerouting power post - disconnection.\r\n" +
                "> INTRUSION_ALERT.log - Log file tracking hacking attempts.\r\n" +
                "--------------------------------------------------------------------\r\n" +
                "* WARNING: Unauthorized access detected.\r\n");

        }

        public override void Interpret(string command)
        {
            if (command == "run execute_protocol")
            {
                if (GameObject.Find("PowerOnSwitch").GetComponent<ShipPowerOn>().isPowerOn)
                {
                    EventDispatcher dispatcher;
                    dispatcher = stateController.gameObject.GetComponent<CoreEventDispatcher>();
                    dispatcher.TriggerEvent();

                    stateController.ChangeText("<color = red> WARNING:</ color > Executing this protocol will destabilize core containment.\r\n" +
                        "Type < color = yellow > 'confirm' </ color > to proceed or < color = cyan > 'cancel' </ color > to abort.");
                }
                else
                {
                    stateController.ChangeText("This computer requires the main power coupling to be manually linked in order to disconnect the Containemnt Core");
                }
            }
            else if (command == "decrypt authorization_codes")
            {
                stateController.ChangeText("<color=yellow>Decrypting AUTHORIZATION_CODES.dat...</color>\r\n" +
                    "< color = green > Decryption successful.</ color >\r\n" +
                    "Access Key: #43X-1924-AEGIS");
            }

            else if (command == "logview CORE_STATUS")
            {
                stateController.ChangeText("<color=cyan>Core Status:</color>\r\n" +
                    "Containment Field: < color = orange > Weak </ color >\r\n" +
                    "Power Load: < color = yellow > 70 %</ color >\r\n" +
                    "External Intrusions: < color = red > Active(1 Source) </ color > ");
            }
            else if (command == "logview CORE_STATUS")
            {
                stateController.ChangeText("<color=cyan>Core Status:</color>\r\n" +
                    "Containment Field: < color = orange > Weak </ color >\r\n" +
                    "Power Load: < color = yellow > 70 %</ color >\r\n" +
                    "External Intrusions: < color = red > Active(1 Source) </ color >\r\n");
            }
            else if (command == "traceback intrustion_alert")
            {
                stateController.ChangeText("<color=cyan>Intrusion Log:</color>\r\n" +
                    "> Unregistered access from terminal ID: 07XA9\r\n" +
                    "> Hacking tool detected: VOIDHAUL_BREACHING_MODULE\r\n" +
                    "> Countermeasures: Disabled");
            }
            else if (command == "reroute power_dispatch")
            {
                stateController.ChangeText("<color=yellow>Rerouting power to auxiliary systems...</color>\r\n" +
                    "< color = green > Power successfully stabilized in non - critical systems.</ color > ");
            }
            else if (command == "help")
            {
                stateController.ChangeText("<color=red>VOIDHAUL_BREACHING_MODULE COMMANDS:</color>  " +
                    "- decrypt AUTHORIZATION_CODES" +
                    "- run EXECUTE_PROTOCOL" +
                    "- override SYSTEM_OVERRIDES  " +
                    "- logview CORE_STATUS  " +
                    "- traceback INTRUSION_ALERT  " +
                    "- reroute POWER_REDISPATCH");
            }
            else
            {
                base.Interpret(command);
            }
        }
    }
}
