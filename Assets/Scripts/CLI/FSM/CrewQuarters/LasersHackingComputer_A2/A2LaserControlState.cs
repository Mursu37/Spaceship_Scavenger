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
            stateController.ChangeText("Laser access control: select a lasergrid to access with [lasergrid_ID]");
            base.OnEnter();
        }

        public override void Interpret(string command)
        {
            if (command == "a1")
            {
                stateController.ChangeText("Laser A1 in lockdown. Submit access credentials \r\n" +
                    "<color=#3Ca8a8>[Module injecting false credentials...] [Success] \r\n" +
                    "<color=#c8a519>Laser A1 unlocked and opened via terminal access");

                EventDispatcher dispatcher;
                dispatcher = stateController.gameObject.GetComponent<EventDispatcher>();
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
