
using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace CLI.FSM
{
    public class DoorState : State
    {
        public DoorState(StateController controller) : base(controller)
        {
            commands.Insert(0, "a12");
            commands.Insert(1, "b07");
            stateController.ChangeText("Door access control: select a door to access with [Door_ID]");
            base.OnEnter();
        }

        public override void Interpret(string command)
        {
            if (command == "a12")
            {
                stateController.ChangeText("Door A12 in lockdown. Submit access credentials \r\n" +
                    "<color=#3Ca8a8>[Module injecting false credentials...] [Success] \r\n" +
                    "<color=#c8a519>Door A12 unlocked and opened via terminal access");

                EventDispatcher dispatcher;
                dispatcher = stateController.gameObject.GetComponent<EventDispatcher>();
                dispatcher.TriggerEvent();
            }

            else if (command == "b07")
            {
                stateController.ChangeText("Door B-07 in lockdown. Submit access credentials \r\n" +
                    "<color=#3Ca8a8>[Module injecting false credentials...] [Failure] \r\n" +
                    "<color=#c8a519>Access connection to Door B-07 not available. Run system diagnostics to locate the point of failure.");
            }

            else if (command == "help")
            {
                stateController.ChangeText("HELP - Door access control: select a door to access by typing the door ID.");
            }

            /*  if (command == "opendoor")
               {
                   if (command.Length < 2)
                   {
                       stateController.ChangeText("OpenDoor requires a parameter");
                       return;
                   }
                   // makes sure input is a positive integer
                   else if (command.All(Char.IsDigit) && command != "")
                   {
                       // get objects with openDoor component and see if one with the correct ID exists
                       var doors = GameObject.FindObjectsByType<OpenDoor>(FindObjectsSortMode.None);

                       foreach (var door in doors)
                       {
                           if (door.doorNumber == int.Parse(command))
                           {
                               door.Open();
                               stateController.ChangeText("Door number " + door.doorNumber + " has been opened");
                               return;
                           }
                       }
                   }
                   else
                   {
                       stateController.ChangeText("'" + command + "'" + " is not a valid input for this command");
                   }
               }*/

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
