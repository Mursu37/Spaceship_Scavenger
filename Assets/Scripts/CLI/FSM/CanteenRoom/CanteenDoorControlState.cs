using Enviroment.MainTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CLI.FSM
{
    public class CanteenDoorControlState : State
    {
        public CanteenDoorControlState(StateController controller) : base(controller)
        {
            commands.Insert(0, "c2");
            stateController.ChangeText("Door access control: select a door to access with [Door_ID]");
            base.OnEnter();
        }

        public override void Interpret(string command)
        {
            if (command == "c2")
            {
                stateController.ChangeText("Door c2 in lockdown. Submit access credentials \r\n" +
                    "<color=#3Ca8a8>[Module injecting false credentials...] [Success] \r\n" +
                    "<color=#c8a519>Door c2 unlocked and opened via terminal access");

                EventDispatcher dispatcher;
                dispatcher = stateController.gameObject.GetComponent<EventDispatcher>();
                dispatcher.TriggerEvent();
            }

            else if (command == "help")
            {
                stateController.ChangeText("HELP - Door access control: select a door to access by typing the door ID.");
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
