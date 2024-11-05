using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

namespace CLI.FSM
{
    public abstract class State
    {
        protected Dictionary<string, State> directories = new Dictionary<string, State>();
        protected StateController stateController;
        
        protected State(StateController controller)
        {
            this.stateController = controller;
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

        public virtual void Interpret(string[] command)
        {
            if (command[0] == "cd" && command.Length > 1)
            {
                MoveDirectory(command[1]);
                return;
            }

            if (command[0] == "ls")
            {
                
            }

            if (command[0] == "help")
            {
                
            }
            
            CommandNotRecognised();
        }
        
        public virtual void ShowCommands() {}

        public virtual void OnExit() {}
        public virtual void OnEnter() {}

        protected virtual void CommandNotRecognised()
        {
            stateController.ChangeText("Command not recognised. Try typing 'help' to see a list of available commands");
        }
    }
}
