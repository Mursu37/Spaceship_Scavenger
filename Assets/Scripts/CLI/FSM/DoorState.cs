
using System;
using System.Linq;
using UnityEngine;

namespace CLI.FSM
{
    public class DoorState : State
    {
        public DoorState(StateController controller) : base(controller)
        {
        }

        public override void Interpret(string command)
        {
            if (command == "opendoor")
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
            }
            else
            {
                base.Interpret(command);
            }
        }
    }
}
