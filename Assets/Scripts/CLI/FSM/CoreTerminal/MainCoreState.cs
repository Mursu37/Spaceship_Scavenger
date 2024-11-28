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
            //directories.Add("downloads", new DownloadState(controller));
            commands.Insert(0, "status");
            commands.Insert(0, "download");
            commands.Insert(0, "diagnose");
            commands.Insert(0, "initialize");
            commands.Insert(0, "logs");
        }

        public override void OnEnter()
        {
            base.OnEnter();
            stateController.ChangeFlavourText("<color=green>Core Systems Directory</color>\r\n" +
                "<BR><BR>" +
                "<color=red>Alert:</color> The spaceship is operating on <color=orange>low auxiliary power.</color>  \r\n" +
                "<BR>Critical systems are offline.  \r\n" +
                "<BR><BR>" +
                "To access the <color=#00ff00>Containment Core Control Panel</color>, reconnect power to the main reactor.  \r\n" +
                "<BR><BR><b>Instructions:</b>  \r\n" +
                "<BR>1. Manually link the main power coupling below the reactor station.  \r\n" +
                "<BR>2. Ensure primary systems are routed through the main reactor.  \r\n" +
                "<BR><BR>" +
                "Type <color=yellow>'download'</color> to retrieve the <color=#00ff00>power routing schematics.</color>");

        }

        public override void Interpret(string command)
        {
            if (command == "help")
            {
                stateController.ChangeText("<color=cyan>Terminal Help Menu:</color>  \r\n" +
                    "- `status` - View system status.  \r\n" +
                    "- `download` - Download blueprints or schematics.  \r\n" +
                    "- `diagnose` - Run a system diagnostic.  \r\n" +
                    "- `initialize` - Reconnect power systems.  \r\n" +
                    "- `logs` - View previous event logs.  \r\n" +
                    "- `reroute` - Reroute power to critical systems.  \r\n" +
                    "- `help` - Display this menu.  ");
            }

            else if (command == "status")
            {
                stateController.ChangeText("<color=green>System Status:</color>  \r\n" +
                    "Auxiliary Power: <color=orange>Active</color>  \r\n" +
                    "Main Reactor: <color=red>Offline</color>  \r\n" +
                    "Containment Core: <color=yellow>Restricted Access</color>  \r\n" +
                    "Power Routing: <color=red>Unlinked</color>");
            }

            else if (command == "download")
            {
                stateController.ChangeText("<color=yellow>Downloading...</color>  \r\n" +
                    "<color=green>Download Complete:</color> Main Power Coupling Schematics saved to device.");
            }

            else if (command == "diagnose")
            {
                stateController.ChangeText("<color=green>Diagnostic Results:</color>  \r\n" +
                    "Main Power Coupling: <color=red>Disconnected</color>  \r\n" +
                    "Auxiliary Power Level: <color=orange>32%</color>  \r\n" +
                    "Reactor Room Pressure: Stable  \r\n" +
                    "Manual Override: Required");
            }

            else if (command == "initialize")
            {
                stateController.ChangeText("<color=yellow>Initializing main reactor power...</color>  \r\n" +
                    "<color=green>Success:</color> Main Power Restored. ");
            }

            else if (command == "logs")
            {
                stateController.ChangeText("<color=green>Accessing Logs:</color>  \r\n" +
                    "[ERROR: Containment Breach Detected]  \r\n" +
                    "[WARNING: Power Fluctuations in Reactor Core]  \r\n" +
                    "[INFO: Auxiliary Systems Engaged - Low Power Mode Active]");
            }

            else
            {
                base.Interpret(command);
            }


        }
    }
}

