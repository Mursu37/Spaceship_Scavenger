using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

namespace CLI.FSM
{
    public abstract class State
    {
        protected Dictionary<string, State> directories = new Dictionary<string, State>();
        protected List<string> commands = new List<string>();
        
        protected StateController stateController;
        
        protected State(StateController controller)
        {
            this.stateController = controller;
            commands.Add("back");
            commands.Add("help");
        }

        public virtual void MoveDirectory(string dir)
        {
            if (dir == "..")
            {
                stateController.BackOne();
            }
            else if (directories.ContainsKey(dir))
            {
                Debug.Log(dir);
                stateController.ChangeDeeper(directories[dir], dir);
            }
            else
            {
                Debug.Log(dir);
                stateController.ChangeText("Directory '" + dir + "' not found");
            }
        }

        public virtual void Interpret(string command)
        {
            if (command == "back")
            {
                MoveDirectory("..");
                return;
            }
            foreach (var dir in directories)
            {
                if (command == dir.Key)
                {
                    stateController.ChangeDeeper(dir.Value, dir.Key);
                    return;
                }
            }
            CommandNotRecognised();
        }
        
        public virtual void ShowCommands() {}

        public virtual List<string> GetCommands()
        {
            var allCommands = new List<string>();
            
            // add all directories and commands to a new list and return it;
            foreach (var dirCommand in directories.Keys)
            {
                allCommands.Add(dirCommand);   
            }
            foreach (var command in commands)
            {
                allCommands.Add(command);   
            }

            return allCommands;
        }

        public virtual void OnExit()
        {
        }

        public virtual void OnEnter()
        {
            stateController.ChangeText("Changed Directory: " + stateController.GetCurrentStateDirectoryText());
        }

        public virtual string GetCurrentStateName()
        {
            return stateController.GetCurrentStateDirectoryText();
        }

        protected virtual void CommandNotRecognised()
        {
            stateController.ChangeText("Command not recognised. Try typing 'help' to see a list of available commands");
        }
    }
}
