using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
//using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CLI.FSM
{
    public abstract class StateController : MonoBehaviour
    {
        [SerializeField] private GameObject CLI;

        [SerializeField] protected TMP_Text commandLineText;
        [SerializeField] protected TMP_InputField commandLineInput;
        [SerializeField] protected TMP_Text directoryText;
        [SerializeField] protected TMP_Text flavourText;
        
        
        [SerializeField] protected Color textColor;
        [SerializeField] protected Color highlightedColor;
        [SerializeField] private float textWriteDelay = 0.001f;

        private bool textResponseCoroutineRunning = false;
        private bool flavourTextCoroutineRunning = false;

        private Coroutine textResponseCoroutine;
        private Coroutine flavourTextCoroutine;

        protected List<GameObject> commandList = new();

        [SerializeField] protected GameObject CLCommandsListObj;

        [SerializeField] protected GameObject CLCommandPrefab;
        // tracks which command is currently selected. -1 = Input field, 0 - commandList.lenght = command at that index
        protected int commandIndex = -1;

        protected State currentState;
        protected List<State> stateHistory = new();
        protected State defaultState;
        protected string dirName = "C";
        protected string defaultDirName;

        protected bool GUIBlock = false;

        private void Start()
        {
            ResetState();
            ChangeState(currentState);
        }

        public virtual void ChangeState(State newState)
        {
            currentState?.OnExit();
            currentState = newState;
            currentState.OnEnter();
            UpdateCommands();
            commandIndex = -1;
            ChangeText("");
        }

        // Removes current commands in list and adds the current states commands to it
        public virtual void UpdateCommands()
        {
            ClearCommands();
            AddCommands();
        }

        // Removes commands from commandList and delete them from canvas
        protected virtual void ClearCommands()
        {
            foreach (var command in commandList)
            {
                Destroy(command);
            }
            commandList.Clear();   
        }

        // Adds commands from current state to CLI Canvas and commandList
        protected virtual void AddCommands()
        {
            var commands = currentState.GetCommands();
            foreach (var command in commands)
            {
                var commandObj = Instantiate(CLCommandPrefab, CLCommandsListObj.transform);
                commandObj.GetComponentInChildren<TMP_Text>().text = command.ToUpper();
                commandList.Insert(0, commandObj);
            }
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
            directoryText.text = dirName;
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
            if (textResponseCoroutine != null)
            {
                StopCoroutine(textResponseCoroutine);
                AudioManager.StopAudio("TerminalTextLoop");
            }

            commandLineText.text = text;
            commandLineText.maxVisibleCharacters = 0;
            textResponseCoroutine = StartCoroutine(RoutineDelayedText(commandLineText, textWriteDelay));
            commandLineInput.text = "";
            commandLineInput.ActivateInputField();
        }

        public virtual void AddText(string text)
        {
            if (textResponseCoroutine != null)
            {
                StopCoroutine(textResponseCoroutine);
                AudioManager.StopAudio("TerminalTextLoop");
            }

            commandLineText.text = commandLineText.text + "<BR><BR>" + text;
            commandLineText.maxVisibleCharacters = 0;
            textResponseCoroutine = StartCoroutine(RoutineDelayedText(commandLineText, textWriteDelay));
            commandLineInput.text = "";

            commandLineInput.ActivateInputField();
        }
        private IEnumerator RoutineDelayedText(TMP_Text textToDisplay, float timeDelay)
        {
            if (!textResponseCoroutineRunning)
            {
                textResponseCoroutineRunning = true;
            }
            else
            {
                yield return null;
            }

            WaitForSecondsRealtime delay = new WaitForSecondsRealtime(timeDelay);

            

            for (int i = 0; i < textToDisplay.textInfo.characterCount; ++i)
            {

                textToDisplay.maxVisibleCharacters = i + 1;
                //string delayedText = textToDisplay.Substring(0, i + 1);
                // Do whatever you need to do with this string, e.g. set text on UI.
                
                yield return delay;
            }
            textResponseCoroutine = null;
            textResponseCoroutineRunning = false; // Coroutine finished;
        }

        private IEnumerator RoutineDelayedFlavourText(TMP_Text textToDisplay, float timeDelay)
        {
            if (!flavourTextCoroutineRunning)
            {
                flavourTextCoroutineRunning = true;
            }
            else
            {
                yield return null;
            }

            WaitForSecondsRealtime delay = new WaitForSecondsRealtime(timeDelay);

            if (!AudioManager.IsPlaying("TerminalTextLoop"))
            {
                AudioManager.PlayAudio("TerminalTextLoop", 1, 1, true, null, true);
            }

            for (int i = 0; i < textToDisplay.textInfo.characterCount; ++i)
            {
                textToDisplay.maxVisibleCharacters = i + 1;
                //string delayedText = textToDisplay.Substring(0, i + 1);
                // Do whatever you need to do with this string, e.g. set text on UI.
                
                yield return delay;
            }
            textResponseCoroutine = null;
            flavourTextCoroutineRunning = false; // Coroutine finished;
        }

        public virtual void ChangeFlavourText(string text)
        {
            if (flavourTextCoroutine != null)
            {
                StopCoroutine(flavourTextCoroutine);
            }

            flavourText.text = text;
            flavourText.maxVisibleCharacters = 0;
            flavourTextCoroutine = StartCoroutine(RoutineDelayedFlavourText(flavourText, textWriteDelay));

        }

        private void OnInput()
        {
            //string[] userInputs = commandLineInput.text.ToLower().Split(" ");
            // Lets the current state to define behaviours that happen based on it's individual properties
            string userInput = commandLineInput.text.ToLower();
            currentState.Interpret(userInput);

        }

        public virtual void ResetState()
        {
            stateHistory.Clear();
            stateHistory.Add(defaultState);
            currentState = defaultState;
            currentState.OnEnter();

            commandLineText.text = "";
            commandLineInput.text = "";
            if (defaultDirName != null)
            {
                dirName = defaultDirName;
            }
            directoryText.text = dirName;

            commandLineInput.ActivateInputField();
        }


        protected void OnEnable()
        {
            PauseGame.Pause(PauseGame.TransitionType.LowPassMusic);
            FindObjectOfType<PauseMenu>().enabled = false;
            VisorChange.UpdateVisor(VisorChange.Visor.Hacking);
            ResetState();
            ChangeState(currentState);
            commandLineInput.ActivateInputField();
            currentState?.OnEnter();
        }

        private void OnDisable()
        {
            textResponseCoroutineRunning = false;
            flavourTextCoroutineRunning = false;

            PauseGame.Resume(PauseGame.TransitionType.NormalMusic);
            FindObjectOfType<PauseMenu>().enabled = true;
            VisorChange.UpdateVisor(VisorChange.currentDamageState);
            ClearCommands();
            ResetState();
            if (AudioManager.IsPlaying("TerminalTextLoop"))
            {
                AudioManager.StopAudio("TerminalTextLoop");
            }

            if (!AudioManager.IsPlaying("TerminalExit"))
            {
                AudioManager.PlayAudio("TerminalExit", 1, 1, false, null, true);
            }
            CLI.SetActive(false);
        }

        void Update()
        {
            if (textResponseCoroutineRunning || flavourTextCoroutineRunning)
            {
                if (!AudioManager.IsPlaying("TerminalTextLoop"))
                {
                    AudioManager.PlayAudio("TerminalTextLoop", 1, 1, true, null, true);
                }
            }
            else
            {
                if (AudioManager.IsPlaying("TerminalTextLoop"))
                {
                    AudioManager.StopAudio("TerminalTextLoop");
                }
            }

            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
            {
                this.enabled = false;
                CLI.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow)) SelectedChange(true);
            else if (Input.GetKeyDown(KeyCode.DownArrow)) SelectedChange(false);

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (commandIndex > -1)
                {
                    currentState.Interpret(commandList[commandIndex].GetComponentInChildren<TMP_Text>().text.ToLower());
                    AudioManager.PlayAudio("TerminalSelect", 1, 1, false, null, true);
                }
                else if (commandLineInput.text != "")
                {
                    OnInput();
                    AudioManager.PlayAudio("TerminalSelect", 1, 1, false, null, true);
                }
            }

            if (commandIndex == -1)
            {
                if (!commandLineInput.isFocused) commandLineInput.ActivateInputField();
            }

            

        }

        protected virtual void SelectedChange(bool up)
        {
            if (up)
            {
                if (commandIndex + 1 > commandList.Count - 1) return;
                commandIndex++;
                AudioManager.PlayAudio("TerminalButtonHighlight", 1, 1, false, null, true);
                commandLineInput.DeactivateInputField();
                commandList[commandIndex].GetComponentInChildren<TMP_Text>().color = highlightedColor;
                if (commandIndex - 1 == -1) return;
                commandList[commandIndex - 1].GetComponentInChildren<TMP_Text>().color = textColor;
            }
            else if (commandIndex - 1 >= -1)
            {
                commandList[commandIndex].GetComponentInChildren<TMP_Text>().color = textColor;
                commandIndex--;
                AudioManager.PlayAudio("TerminalButtonHighlight", 1, 1, false, null, true);
                if (commandIndex == -1)
                {
                    commandLineInput.ActivateInputField();
                    return;
                }
                commandList[commandIndex].GetComponentInChildren<TMP_Text>().color = highlightedColor;
            }
        }
    }
}
