using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace CLI.FSM
{
    public abstract class StateController : MonoBehaviour
    {
        [SerializeField] private GameObject CLI;

        [SerializeField] protected TMP_Text commandLineText;
        [SerializeField] protected TMP_InputField commandLineInput;
        [SerializeField] protected TMP_Text directoryText;

        protected State currentState;
        protected List<State> stateHistory;
        protected State defaultState;
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

            ChangeText("Moved back to " + directoryText.text + " directory");
            
            ChangeState(stateHistory.Last());
            
            // loop through current directories to update the directoryText
            string[] currentDirectories = directoryText.text.ToLower().Split("\\");
            directoryText.text = "C:";
            for (int i = 1; i < currentDirectories.Length-1; i++)
            {
                directoryText.text += "\\" + currentDirectories[i];
            }
            
        }

        public virtual void ChangeDeeper(State newState, string dirName)
        {
            stateHistory.Add(newState);
            directoryText.text += "\\" + dirName;
            ChangeText("Moved to " + directoryText.text + " directory");
            ChangeState(newState);
        }

        public virtual void ChangeText(string text)
        {
            commandLineText.text = text;
            commandLineInput.text = "";
            commandLineInput.ActivateInputField();
        }

        public virtual void AddText(string text)
        {
            commandLineText.text = commandLineText.text + "<BR><BR>" + text;
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
        
        
        protected void OnEnable()
        {
            Time.timeScale = 0.01f;
            commandLineInput.ActivateInputField();
            currentState?.OnEnter();
        }

        private void OnDisable()
        {
            Time.timeScale = 1;
            CLI.SetActive(false);

            stateHistory.Clear();
            stateHistory.Add(defaultState);
            currentState = defaultState;
            currentState.OnEnter();

            commandLineText.text = "";
            commandLineInput.text = "";
            directoryText.text = "C:";

            commandLineInput.ActivateInputField();
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
