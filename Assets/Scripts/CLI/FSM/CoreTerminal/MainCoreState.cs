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
        private bool coreQuery = false;
        private bool downloaded = false;
        private bool breached = false;
        private StateController m_controller;

        private CoreState coreState;

        public MainCoreState(StateController controller) : base(controller)
        {
            coreState = new CoreState(controller);
            m_controller = controller;
            directories.Add("core", coreState);
            commands.Insert(0, "logs");
            commands.Insert(1, "download information");
        }


        public override void OnEnter()
        {
            isPowerOn = GameObject.FindObjectOfType<ShipPowerOn>().isPowerOn;
            query = false;

            //Remove Query Commands (This works for now but caused issues earlier when having a if-statement structure)
            RemoveGlobalQueryCommands();

            if (!isPowerOn)
            {
                stateController.ChangeFlavourText("Core Systems Directory\r\n" +
                "<color=#c8a519>Alert:</color> The spaceship is operating on low auxiliary power.  \r\n" +
                "Critical systems are offline.  \r\n" +
                "To access the Containment Core Control Panel, reconnect power to the main reactor.  \r\n\r\n" +
                "<color=#c8a519><b>Instructions:</b>  \r\n" +
                "1. Manually link the main power coupling below the reactor station.  \r\n" +
                "2. Ensure primary systems are routed through the main reactor.  \r\n\r\n" +
                "Type 'download' to retrieve the power routing blueprints.</color>\r\n");
            }
            else
            {
                stateController.ChangeFlavourText("Core Systems Directory\r\n" +
                "<color=#c8a519>Notice:</color> Main reactor power is online.\r\n" +
                "All critical systems are fully operational.\r\n" +
                "Containment Core Control Panel is now accessible.\r\n" +
                "<color=#c8a519><b>System Update:</b>\r\n" +
                "1. Power coupling successfully linked to the main reactor.\r\n" +
                "2. Primary systems are stabilized and routed.\r\n" +
                "Type 'core' to access Containment Core Control Panel.\r\n" +
                "Type 'logs' to review power restoration logs.</color>");
            }


            stateController.ChangeText("Type Your Command");

            base.OnEnter();
        }

        public override void OnExit()
        {
            /*
            if (commands.Contains("yes"))
            {
                commands.Remove("yes");
            }
            if (commands.Contains("no"))
            {
                commands.Remove("no");
            }
            if (commands.Contains("confirm"))
            {
                commands.Remove("confirm");
            }
            if (commands.Contains("cancel"))
            {
                commands.Remove("cancel");
            }
*/
            //  stateController.UpdateCommands();

            coreQuery = false;
            query = false;

            base.OnExit();
        }

        public override void Interpret(string command)
        {
            //Remove Query commands
            RemoveGlobalQueryCommands();

            if (query == true && command == "no" || query == true && command == "n")
            {
                stateController.ChangeText("Power routing blueprints download aborted.");
                query = false;
                return;
            }

            else if (query == true && command == "yes" || query == true && command == "y")
            {
                stateController.ChangeText("<color=#c8a519>Downloading power routing blueprints...</color>");
                stateController.AddText("<line-height=0>========================================================", 0, true, () =>
                {
                    stateController.AddText("<color=#c8a519><line-height=2em>■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■</line-height>", 0.05f, true, () =>
                    {
                        stateController.AddText("Download Complete: Main Power Coupling Blueprints saved to scanner device.", 0f, true, () =>
                        {
                            EventDispatcher dispatcher;
                            dispatcher = stateController.gameObject.GetComponent<DownloadEventDispatcher>();
                            dispatcher.TriggerEvent();

                            downloaded = true;

                        });

                    });

                });

                query = false;
                return;
            }


            query = false;



            if (coreQuery == true && command == "cancel" && isPowerOn)
            {
                stateController.ChangeText("Power routing blueprints download aborted.");
                coreQuery = false;
                return;
            }

            else if (coreQuery == true && command == "confirm" && isPowerOn)
            {
                stateController.ChangeText("<color=#3Ca8a8>[Module: Breaching Core Control Directory...]</color>");
                stateController.AddText("<line-height=0>========================================================", 0, true, () =>
                {
                    stateController.AddText("<color=#3Ca8a8><line-height=2em>■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■</line-height>", 0.05f, true, () =>
                    {
                        breached = true;
                        stateController.ChangeDeeper(coreState, "core");
                    });

                });

                coreQuery = false;
                return;
            }

            if (command == "core" && isPowerOn)
            {
                if (!breached)
                {
                    stateController.ChangeText("Restricted directory, standing by for access codes input.." +
                        "<color=#3Ca8a8>[Module: Breach core control directory?</color><color=#53e09c> Confirm / Cancel?]");
                    coreQuery = true;
                    commands.Add("confirm");
                    commands.Add("cancel");
                    stateController.UpdateCommands();
                }
                else
                {
                    stateController.ChangeDeeper(coreState, "core");
                }
            }

            else if (command == "core" && !isPowerOn)
            {
                stateController.ChangeText("<color=#3Ca8a8>[Module: Cannot access containment core utilities while the system is running on auxiliary power mode.\r\n" +
                    "Reroute the power to main power in order to breach core system directory.]");
                coreQuery = false;


            }

            else if (command == "help")
            {
                stateController.ChangeText("- help - show available commands:\r\n" +
                    "- download - Download system diagnostic blueprints.  \r\n" +
                    "- logs - View previous event logs.");

            }

            else if (command == "download" || command == "download information")
            {
                if (!downloaded)
                {
                    stateController.ChangeText("Download Power Routing Blueprints? Yes/No");

                    query = true;
                    commands.Add("yes");
                    commands.Add("no");
                    stateController.UpdateCommands();
                }
                else
                {
                    stateController.ChangeText("Power routing blueprints already downloaded.\r\n" +
                        "<color=#3Ca8a8>[Module: Activate your visor scanner off terminal with pressing 'X' Key.]");
                }
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
                if (!isPowerOn)
                {
                    stateController.ChangeText("Accessing Logs:\r\n" +
                    "[ERROR: Containment Breach Detected]\r\n" +
                    "[WARNING: Power Fluctuations in Reactor Core]  \r\n" +
                    "[INFO: Auxiliary Systems Engaged - Low Power Mode Active] \r\n" +
                    "[INFO: Containment Breach Sealed - Reactor Stable]");
                }
                else
                {
                    stateController.ChangeText("Accessing Logs:\r\n" +
                        "[INFO: Containment Breach Sealed - Reactor Stable]\r\n" +
                        "[INFO: Power Fluctuations Resolved - Core Online]\r\n" +
                        "[INFO: Main Power Restored - Normal Mode Active]\r\n");
                }
            }



            else
            {
                base.Interpret(command);
            }


        }

    }
}
