using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace CLI.FSM
{
    public abstract class StateController : MonoBehaviour
    {
        [SerializeField] private GameObject CLI;
        
        [SerializeField] private TMP_Text commandLineText;
        [SerializeField] private TMP_InputField commandLineInput;
        [SerializeField] private TMP_Text directoryText;
        
        public State currentState;
        public List<State> stateHistory;
        public State defaultState;
        private string path;

        protected StateController()
        {
            defaultState = new MainDriveState(this);
            stateHistory = new List<State> { defaultState };
            currentState = defaultState;
        }

        public virtual void ChangeState(State newState)
        {
            currentState?.OnExit();
            currentState = newState;
            currentState.OnEnter();
        }

        public virtual void BackOne()
        {
            if (stateHistory.Count <= 1)
            {
                ChangeText("Cannot move back from " + directoryText.text + " directory");
                return;
            }
            stateHistory.Remove(stateHistory.Last());
            ChangeState(stateHistory.Last());
            
            // loop through current directories to update the directoryText
            string[] currentDirectories = directoryText.text.ToLower().Split("\\");
            directoryText.text = "C:";
            for (int i = 1; i < currentDirectories.Length-1; i++)
            {
                directoryText.text += "\\" + currentDirectories[i];
            }
            
            ChangeText("Moved back to " + directoryText.text + " directory");
        }

        public virtual void ChangeDeeper(State newState, string dirName)
        {
            stateHistory.Add(newState);
            ChangeState(newState);
            directoryText.text += "\\" + dirName;
            ChangeText("Moved to " + directoryText.text + " directory");
        }

        public virtual void ChangeText(string text)
        {
            commandLineText.text = text;
            commandLineInput.text = "";
            commandLineInput.ActivateInputField();
        }
        
        private void OnGUI()
        {
            // Detects a new string of text being sent from commandLine
            if (commandLineInput.text != "" && Input.GetKeyDown(KeyCode.Return))
            {
                OnInput();
            }
        }

        private void OnInput()
        {
            string[] userInputs = commandLineInput.text.ToLower().Split(" ");
            // Lets the current state to define behaviours that happen based on it's individual properties
            currentState.Interpret(userInputs);
        }
        
        
        private void OnEnable()
        {
            Time.timeScale = 0;
            commandLineInput.ActivateInputField();
        }

        private void OnDisable()
        {
            Time.timeScale = 1;
            CLI.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
            {
                this.enabled = false;
            }
        }
    }
}
