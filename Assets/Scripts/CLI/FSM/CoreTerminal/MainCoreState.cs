using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enviroment.MainTerminal;

namespace CLI.FSM
{
    public class MainCoreState : State
    {
        private bool isPowerOn = false;
        private bool query = false;

        public MainCoreState(StateController controller) : base(controller)
        {
            directories.Add("core", new CoreState(controller));
            //directories.Add("downloads", new DownloadState(controller));
            commands.Insert(0, "status");
            commands.Insert(0, "download");
            commands.Insert(0, "diagnose");
            // commands.Insert(0, "initialize");
            commands.Insert(0, "logs");
        }


        public override void OnEnter()
        {
            isPowerOn = GameObject.FindObjectOfType<ShipPowerOn>().isPowerOn;
            base.OnEnter();
            stateController.ChangeFlavourText("Core Systems Directory\r\n" +
                "<color=#c8a519>Alert:</color> The spaceship is operating on low auxiliary power.  \r\n" +
                "Critical systems are offline.  \r\n" +
                "To access the Containment Core Control Panel, reconnect power to the main reactor.  \r\n\r\n" +
                "<color=#c8a519><b>Instructions:</b>  \r\n" +
                "1. Manually link the main power coupling below the reactor station.  \r\n" +
                "2. Ensure primary systems are routed through the main reactor.  \r\n" +
                "Type 'download' to retrieve the power routing schematics.</color>");

            stateController.ChangeText("Type Your Command");

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



            if (query == true && command == "no" || query == true && command == "n")
            {
                stateController.ChangeText("Power routing schematics download aborted.");
                query = false;
                return;
            }

            else if (query == true && command == "yes" || query == true && command == "y")
            {
                stateController.ChangeText("<color=#c8a519>Downloading power routing schematics...</color>");
                stateController.AddText("<line-height=0>========================================================", 0, true, () =>
                {
                    stateController.AddText("<color=#c8a519><line-height=2em>■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■", 0.05f, true, () =>
                    {
                        stateController.AddText("Download Complete: Main Power Coupling Schematics saved to scanner device.", 0f, true, () =>
                        {
                            EventDispatcher dispatcher;
                            dispatcher = stateController.gameObject.GetComponent<DownloadEventDispatcher>();
                            dispatcher.TriggerEvent();

                        });

                    });

                });

                query = false;
                return;
            }


            query = false;


            if (command == "core" && !isPowerOn)
            {
                stateController.ChangeText("<color=#3Ca8a8>[Module cannot access containment core utilities while the system is running on auxiliary power mode.\r\n" +
                    "Reroute the power to main power in order to breach core system directory.]");
            }

            else if (command == "help")
            {
                stateController.ChangeText("- help - show available commands:\r\n" +
                    "- status - View system status.  \r\n" +
                    "- download - Download system diagnostic schematics.  \r\n" +
                    "- diagnose - Run a system diagnostic.  \r\n" +
                    //  "- `initialize` - Reconnect power systems.  \r\n" +
                    "- logs - View previous event logs.");
                   // "- `reroute` - Reroute power to critical systems.  \r\n" +
                   // "- `help` - Display this menu."
                   
            }

            else if (command == "status")
            {
                stateController.ChangeText("System Status:  \r\n" +
                    "Auxiliary Power: <color=#c8a519>Active</color>  \r\n" +
                    "Main Reactor: <color=#c8a519>Offline</color>  \r\n" +
                    "Containment Core: <color=#c8a519>Restricted Access</color>  \r\n" +
                    "Power Routing: <color=#c8a519>Unlinked</color>");
            }

            else if (command == "download")
            {
                stateController.ChangeText("Download Power Routing Schematics? Yes/No");

                query = true;
                commands.Add("yes");
                commands.Add("no");
                stateController.UpdateCommands();
            }

            else if (command == "diagnose")
            {
                stateController.ChangeText("Diagnostic Results:  \r\n" +
                    "Main Power Coupling: <color=#c8a519>Disconnected</color>  \r\n" +
                    "Auxiliary Power Level: <color=#c8a519>32%</color>  \r\n" +
                    "Reactor Room Pressure: Stable  \r\n" +
                    "Manual Override: Required");
            }
            /*
            else if (command == "initialize")
            {
                stateController.ChangeText("<color=yellow>Initializing main reactor power...</color>  \r\n" +
                    "<color=green>Success:</color> Main Power Restored. ");
            }
            */
            else if (command == "logs")
            {
                stateController.ChangeText("Accessing Logs:\r\n" +
                    "[ERROR: Containment Breach Detected]\r\n" +
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

