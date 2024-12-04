using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CLI.FSM
{
    public class A2LaserControlState : State
    {
        public A2LaserControlState(StateController controller) : base(controller)
        {
            commands.Insert(0, "a1");
            commands.Insert(1, "a2");
            if (CheckpointManager.captainsCredentialsGained)
            {
                commands.Insert(2, "a3-[use credentials]");
            }
            else
            {
                commands.Insert(2, "a3");
            }

            stateController.ChangeText("Laser access control: select a lasergrid to access with [lasergrid_ID]");
            base.OnEnter();
        }

        public override void OnEnter()
        {
            if (CheckpointManager.captainsCredentialsGained && commands.Contains("a3"))
            {   
                commands.Remove("a3");
                commands.Insert(2, "a3-[use credentials]");
            }

            stateController.UpdateCommands();
            base.OnEnter();
        }

        public override void Interpret(string command)
        {

            if (command == "a3" && !CheckpointManager.captainsCredentialsGained)
            {
                stateController.ChangeText("Laser A3 in lockdown. Submit access credentials\r\n" +
                    "<color=#3Ca8a8>[Module injecting false credentials...] [Access Denied]\r\n" +
                    "<color=#c8a519>Security clearance insufficient.");
            }

            else if (CheckpointManager.captainsCredentialsGained && command == "a3-[use credentials]" || command == "a3")
            {
                stateController.ChangeText("Laser A3 in lockdown. Submit access credentials\r\n" +
                    "<color=#3Ca8a8>[Module injecting downloaded credentials from captain's terminal...] [Access Granted]\r\n" +
                    "<color=#c8a519>Security clearance accepted. Greetings, Captain!\r\n" +
                    "<color=#c8a519>Laser A2 unlocked and opened via terminal access");

                EventDispatcher dispatcher;
                dispatcher = stateController.gameObject.GetComponent<CredentialsEventDispatcher>();
                dispatcher.TriggerEvent();
            }

            else if (command == "a1")
            {
                stateController.ChangeText("Laser A1 in lockdown. Submit access credentials \r\n" +
                    "<color=#3Ca8a8>[Module injecting false credentials...] [Success] \r\n" +
                    "<color=#c8a519>Laser A2 unlocked and opened via terminal access");

                EventDispatcher dispatcher;
                dispatcher = stateController.gameObject.GetComponent<EventDispatcher>();
                dispatcher.TriggerEvent();
            }

            else if (command == "a2")
            {
                stateController.ChangeText("Laser A2 in lockdown. Submit access credentials \r\n" +
                    "<color=#3Ca8a8>[Module injecting false credentials...] [Success] \r\n" +
                    "<color=#c8a519>Laser A2 unlocked and opened via terminal access");

                EventDispatcher dispatcher;
                dispatcher = stateController.gameObject.GetComponent<AuxiliaryEventDispatcher>();
                dispatcher.TriggerEvent();
            }


            else if (command == "help")
            {
                stateController.ChangeText("HELP - Laser access control: select lasers to access by typing the lasergrid ID.");
            }

            else
            {
                base.Interpret(command);
            }
        }

        public override void OnExit()
        {

            base.OnExit();
        }
    }
}
