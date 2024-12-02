using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CLI.FSM
{
    public class SecurityState_L : SecurityState
    {
        public SecurityState_L(StateController controller) : base(controller)
        {

        }

        public override void OnEnter()
        {
            stateController.ChangeText("DIRECTORY: /security_systems/laser_defense/" +
                "---------------------------------------------------------- -" +
                "> LASER_CONTROL.dll - Manage laser grid functionality." +
                "> SYSTEM_STATUS.log - Current operational status of laser defenses." +
                "> POWER_ROUTING.cfg - Power routing configuration for laser systems." +
                "> ERROR_LOG.dat - Records of recent system malfunctions." +
                "> DISABLE_DEFENSES.exe - Temporarily disable laser grid security." +
                "---------------------------------------------------------- -" +
                "*Warning: Manual override required to reactivate systems.");
            base.OnEnter();
        }

        public override void Interpret(string command)
        {
            if (command == "laser_control")
            {
                stateController.ChangeText("LASER_CONTROL.dll executed." +
                    "Laser grid functionality accessed." +
                    "Options:" +
                    "-Activate Grid" +
                    "- Deactivate Grid");
            }
            if (command == "system_status")
            {
                stateController.ChangeText("SYSTEM_STATUS.log accessed." +
                    "Displaying current operational status:" +
                    "- Sector A: Active" +
                    "- Sector B: Active" +
                    "- Sector C: Offline(Power routed to emergency systems)" +
                    "- Sector D: Malfunction detected" +
                    "Status report complete.");
            }
            if (command == "power_routing")
            {
                stateController.ChangeText("POWER_ROUTING.cfg accessed." +
                    "Current Configuration:" +
                    "-Primary Source: Reactor Core" +
                    "- Backup Source: Auxiliary Battery" +
                    "- Active Routing: 75 % laser systems, 25 % security doors" +
                    "Adjust routing ? Use 'modify_power' to reconfigure.");
            }
            if (command == "error_log")
            {
                stateController.ChangeText("ERROR_LOG.dat accessed." +
                    "Displaying recent malfunctions:" +
                    "[11:24:03] - Sector D: Beam alignment error detected." +
                    "[11:35:41] - Sector C: Power supply interrupted." +
                    "[12:02:14] - Grid - wide desynchronization event resolved." +
                    "Use 'clear_log' to reset error log or 'download_log' to save locally.");
            }
            if (command == "disable_defenses")
            {
                stateController.ChangeText("DISABLE_DEFENSES.exe executed." +
                    "Laser grid security disabled.All sectors offline." +
                    "Warning: Manual override required to reactivate systems.");
            }
            if (command == "help")
            {
                stateController.ChangeText("Available commands:" +
                    "LASER_CONTROL - Manage laser grid functionality." +
                    "SYSTEM_STATUS - Current operational status of laser defenses." +
                    "POWER_ROUTING - Power routing configuration for laser systems." +
                    "ERROR_LOG.dat - Records of recent system malfunctions." +
                    "DISABLE_DEFENSES - Temporarily disable laser grid security.");

            }

            base.Interpret(command);
        }

    }
}

