using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CLI.FSM
{
    public class A1DownloadState : State
    {
        private bool ventDownloaded = false;
        private bool query = false;

        public A1DownloadState(StateController controller) : base(controller)
        {
            commands.Insert(0, "download schematics");
        }

        public override void OnEnter()
        {
            RemoveGlobalQueryCommands();
            base.OnEnter();
        }

        public override void Interpret(string command)
        {
            RemoveGlobalQueryCommands();

            if (query == true && command == "yes" || command == "y")
            {
                stateController.AddText("<color=#3Ca8a8>downlaoding [Ventilation_Schematics]:</color>");
                stateController.AddText("<line-height=0>========================================================", 0, true, () =>
                {
                    stateController.AddText("<color=#3Ca8a8><line-height=2em>■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■</line-height>", 0.01f, true, () =>
                    {
                        stateController.AddText("<color=#3Ca8a8>[Ventilation Schematics succesfully downloaded.]", 0f, true, () =>
                        {
                            EventDispatcher dispatcher;
                            dispatcher = stateController.gameObject.GetComponent<EventDispatcher>();
                            dispatcher.TriggerEvent();
                            ventDownloaded = true;

                            if (commands.Contains("download schematics"))
                            {
                                commands.Remove("download schematics");
                            }
                            stateController.UpdateCommands();

                        });

                    });

                });

                query = false;
                return;
            }
            else if (query == true && command == "no" || command == "n")
            {
                stateController.ChangeText("ventilation Schematics download aborted.");
                query = false;
                return;
            }

            query = false;

            if (command == "help")
            {
                stateController.ChangeText("HELP - Available Commands: \r\n" +
                    "download schematics - download file");
            }


            else if (command == "download schematics" && !ventDownloaded)
            {
                stateController.ChangeText("Download ventilation Schematics? Yes/No");

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
