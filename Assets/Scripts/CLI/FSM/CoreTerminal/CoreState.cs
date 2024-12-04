using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CLI.FSM
{
    public class CoreState : State
    {
        public bool protocolHasRun = false;
        private bool protocolIsRunning = false;
        private bool decryptedCodesDone = false;
        private bool overrideSystemDone = false;
        private bool injectedCredentialsDone = false;
        private bool query = false;

        

        public CoreState(StateController controller) : base(controller)
        {
            commands.Insert(0, "module instructions");
            commands.Insert(1, "run core_extract_protocol");
            if (!injectedCredentialsDone)
            {
                commands.Insert(2, "inject access_point");
            }
            if (!decryptedCodesDone)
            {
                commands.Insert(3, "decrypt keys");
            }
            if (!overrideSystemDone)
            {
                commands.Insert(4, "run system_overrides");
            }
            // commands.Insert(5, "traceback intrustion_alert");
        }

        public override void OnEnter()
        {

            //Remove Query Commands (This works for now but caused issues earlier when having a if-statement structure)
            RemoveGlobalQueryCommands();

            query = false;
            stateController.ChangeFlavourText("DIRECTORY: /core_systems/disconnect_protocol/\r\n" +
                "<align=flush>--------------------------------------------------------------------</align>\r\n" +
                "<align=left>> CORE_EXTRACT_PROTOCOL.dll<line-height=0>\r\n" +
                "<align=right>- Executable for core disconnection.<line-height=1em>\r\n" +
                "<align=left>> INTRUSION_ALERT.log<line-height=0>\r\n" +
                "<align=right>- Log file tracking hacking attempts.<line-height=1em>\r\n" +
                "   <align=left><color=#3Ca8a8>[ACCESS_POINT.exe<line-height=0>\r\n" +
                "<align=right>- Establish injection point for system control.]<line-height=1em>\r\n" +
                "   <align=left><color=#3Ca8a8>[DECRYPT_CODES.exe<line-height=0>\r\n" +
                "<align=right>- Decrypt authorization keys for access.]<line-height=1em>\r\n" +
                "   <align=left><color=#3Ca8a8>[SYSTEM_OVERRIDES.sys<line-height=0>\r\n" +
                "<align=right>- Configure bypass of core safety protocols.]<line-height=1em>\r\n" +
                "<align=flush><color=#0a6310>--------------------------------------------------------------------</align>\r\n" +
                "<align=left><color=#0a6310>* WARNING: Unauthorized access detected.</align>");

            base.OnEnter();
        }

        private void InjectFlavor()
        {
            if (!injectedCredentialsDone)
            {
                if (!protocolIsRunning)
                {
                    stateController.ChangeText("");
                }

                stateController.ChangeText("<color=#3Ca8a8>run [ACCESS_POINT.exe]</color>");
                stateController.AddText("<line-height=0>========================================================", 0, true, () =>
                {
                    LightMapSceneManager.instance.PreLoadScene(LightMapSceneManager.instance.pool.sceneNames[2]);
                    stateController.AddText("<color=#3Ca8a8><line-height=2em>■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■</line-height>", 0.05f, true, () =>
                    {
                        stateController.AddText("Injection point established.\r\n" +
                           "Temporary system control link activated.\r\n" +
                           "Proceed with decryption of access codes.", 0f, true, () =>
                        {
                            injectedCredentialsDone = true;

                            if (commands.Contains("inject access_point"))
                            {
                                commands.Remove("inject access_point");
                            }
                            stateController.UpdateCommands();

                            if (protocolIsRunning)
                            {
                                DecryptFlavor();
                            }
                        });

                    });

                });
            }
            else
            {
                stateController.AddText("Injection point already established.\r\n" +
                           "Proceed with decryption of access codes.");

                if (protocolIsRunning)
                {
                    DecryptFlavor();
                }
            }
        }

        private void DecryptFlavor()
        {
            if (!decryptedCodesDone)
            {
                if (!protocolIsRunning)
                {
                    stateController.ChangeText("");
                }

                stateController.AddText("<color=#3Ca8a8>run [DECRYPT_CODES.exe]:</color>");
                stateController.AddText("<line-height=0>========================================================", 0, true, () =>
                {
                    stateController.AddText("<color=#3Ca8a8><line-height=2em>■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■</line-height>", 0.05f, true, () =>
                    {
                        stateController.AddText("<color=#3Ca8a8>[Authorization keys successfully decrypted.\r\n" +
                           "<color=#3Ca8a8>[Access key: \"#43X-1924-AEGIS\" added to memory buffer.]\r\n" +
                           "<color=#3Ca8a8>[Continue to safety override configuration.]", 0f, true, () =>
                           {
                               decryptedCodesDone = true;

                               if (commands.Contains("decrypt keys"))
                               {
                                   commands.Remove("decrypt keys");
                               }
                               stateController.UpdateCommands();

                               if (protocolIsRunning)
                               {
                                   OverrideFlavor();
                               }
                           });

                    });

                });
            }
            else
            {
                stateController.AddText("<color=#3Ca8a8>[Authorization keys successfully decrypted.]\r\n" +
                "<color=#3Ca8a8>[Access key: \"#43X-1924-AEGIS\" added to memory buffer.]\r\n" +
                "<color=#3Ca8a8>[Continue to safety override configuration.]");

                if (protocolIsRunning)
                {
                    OverrideFlavor();
                }
            }
        }

        private void OverrideFlavor()
        {
            if (!overrideSystemDone)
            {
                if (!protocolIsRunning)
                {
                    stateController.ChangeText("");
                }

                stateController.AddText("<color=#3Ca8a8>run [SYSTEM_OVERRIDES.sys]:</color>");
                stateController.AddText("<line-height=0>========================================================", 0, true, () =>
                {
                    stateController.AddText("<color=#3Ca8a8><line-height=2em>■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■</line-height>", 0.05f, true, () =>
                    {
                        stateController.AddText("<color=#3Ca8a8>[Core safety protocols bypassed.]\r\n" +
                           "<color=#3Ca8a8>[Extraction measures enabled. Proceed with caution.]\r\n" +
                           "<color=#3Ca8a8>[Execute CORE_EXTRACT_PROTOCOL to begin disconnection.]", 0f, true, () =>
                           {
                               overrideSystemDone = true;

                               if (commands.Contains("run system_overrides"))
                               {
                                   commands.Remove("run system_overrides");
                               }
                               stateController.UpdateCommands();

                               if (protocolIsRunning)
                               {
                                   Disconnect();
                               }
                           });

                    });

                });
            }
            else
            {
                stateController.ChangeText("<color=#3Ca8a8>[SYSTEM_OVERRIDES.sys]:\r\n" +
                "<color=#3Ca8a8>[Override over Core safety protocols has already been established.]\r\n" +
                "<color=#3Ca8a8>[Execute CORE_EXTRACT_PROTOCOL to begin disconnection.]");

                if (protocolIsRunning)
                {
                    Disconnect();
                }
            }


        }

        private void Disconnect()
        {
            if (!protocolHasRun)
                {
                    if (!protocolIsRunning)
                    {
                        stateController.ChangeText("");
                    }

                    stateController.AddText("<color=#3Ca8a8>run CORE_EXTRACT_PROTOCOL.dll:</color>");
                    stateController.AddText("<line-height=0>========================================================", 0, true, () =>
                    {
                        stateController.AddText("<color=#3Ca8a8><line-height=2em>■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■</line-height>", 0.05f, true, () =>
                        {
                            stateController.AddText("<align=left>Core disconnection initiated.<line-height=0>\r\n" +
                               "<align=right><color=#3Ca8a8>[Using Injected Access point]<line-height=1em>\r\n" +
                               "<align=left><color=#0a6310>*Unauthorized access detected<line-height=0>\r\n" +
                               "<align=right><color=#3Ca8a8>[Submitting decryted access keys]<line-height=1em>\r\n" +
                               "<align=left><color=#0a6310>Activating security protocols<line-height=0>\r\n" +
                               "<align=right><color=#3Ca8a8>[Safety protocols bypassed]<line-height=1em>\r\n" +
                               "<align=left><color=#3Ca8a8>[Manual extraction of core succesfully enabled.]\r\n" +
                               "<align=left><color=#0a6310>*Safety countermeasures blocked. Rebooting security systems..." +
                               "", 0f, true, () =>
                               {
                                   EventDispatcher dispatcher;
                                   dispatcher = stateController.gameObject.GetComponent<CoreEventDispatcher>();
                                   dispatcher.TriggerEvent();
                                   AudioManager.PlayAudio("InteractBeep2", 0.4f, 1, false, null, true);
                                   protocolHasRun = true;
                                   protocolIsRunning = false;
                               });

                        });

                    });
                }
        }

        private void DisconnectFlavor()
        {
            protocolIsRunning = true;

            if (!protocolHasRun)
            {
                InjectFlavor();
            }
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
         //   stateController.UpdateCommands();

            query = false;

            base.OnExit();
        }

        public override void Interpret(string command)
        {

            //Remove Query commands
            RemoveGlobalQueryCommands();

            if (query == true && command == "confirm")
            {
                DisconnectFlavor();
                query = false;
                return;
            }
            else if (query == true && command == "cancel")
            {
                stateController.ChangeText("<color=#3Ca8a8>[Containment Core Extract Protocol execution aborted.]");

                query = false;
                return;
            }

            query = false;


            if (command == "run core_extract_protocol")
            {
                if (!protocolHasRun)
                {
                    if (GameObject.Find("PowerOnSwitch").GetComponent<ShipPowerOn>().isPowerOn)
                    {

                        stateController.ChangeText("<color=#3Ca8a8>[WARNING: Executing this protocol will destabilize the core containment.\r\n" +
                        "<color=#3Ca8a8>Type 'confirm' to proceed or 'cancel' to abort.]");

                        query = true;
                        commands.Add("confirm");
                        commands.Add("cancel");
                        stateController.UpdateCommands();
                    }
                    else
                    {
                        stateController.ChangeText("This computer requires the main power coupling to be manually linked in order to disconnect the Containemnt Core");
                    }
                }
                else
                {
                    stateController.ChangeText("<color=#3Ca8a8>[The protocol has been executed.]\r\n" +
                        "[Manual disconnection of core is enabled.]\r\n" +
                        "[Proceed with manual core disengagement. Disengagement switch ready for activation.]\r\n" +
                        "<color=#53e09c>[Warning: Core extraction activates the ship alarms and triggers core meltdown sequence.]");
                }

            }


            else if (command == "module instructions")
            {
                stateController.ChangeText("<color=#3Ca8a8>[Type 'RUN CORE_EXTRACT_PROTOCOL' to initiate\r\n" +
                    "automated core disconnection override.\r\n" +
                    "<color=#53e09c><b>Warning:</b></color> Core extraction will trigger shipwide alarms.\r\n" +
                    "<color=#53e09c>Emergency protocols and automated defenses may engage," +
                    " Proceed with caution.<color=#3Ca8a8>]\r\n");
            }

            else if (command == "inject access_point")
            {
                InjectFlavor();
            }

            else if (command == "decrypt keys")
            {
                DecryptFlavor();
            }

            else if (command == "run system_overrides")
            {
                OverrideFlavor();
            }

          /*  else if (command == "traceback intrustion_alert")
            {
                stateController.ChangeText("Intrusion Log:\r\n" +
                    "> Unregistered access from terminal ID: 07XA9\r\n" +
                    "> Hacking tool detected: VOIDHAUL_BREACHING_MODULE\r\n" +
                    "> Countermeasures: Disabled");
            }*/
            else if (command == "help")
            {
                stateController.ChangeText("<color=#3Ca8a8>[VOIDHAUL_BREACHING_MODULE COMMANDS]</color>\r\n" +
                    "<color=#3Ca8a8>[- module instructions - instructions of the hacking module]\r\n" +
                    "<color=#3Ca8a8>[- run core_extract_protocol - run core extract protocol]\r\n" +
                    "<color=#3Ca8a8>[- inject access_point] - establishes an access point for the module\r\n" +
                    "<color=#3Ca8a8>[- decrypt keys] - decrypts access keys\r\n" +
                    "<color=#3Ca8a8>[- run system_overrides - overrides safety protocols]");
            }
            else
            {
                base.Interpret(command);
            }
        }
    }
}
