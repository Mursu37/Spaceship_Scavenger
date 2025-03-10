using Enviroment.MainTerminal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
//using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        [SerializeField] protected Color commandColor;
        [SerializeField] protected Color coreCommandColor;
        [SerializeField] private float textWriteDelay = 0.001f;

        private bool textResponseCoroutineRunning = false;
        private bool flavourTextCoroutineRunning = false;

        private Coroutine textResponseCoroutine;
        private Coroutine flavourTextCoroutine;

        protected List<GameObject> commandList = new();

        [SerializeField] protected GameObject CLCommandsListObj;

        [SerializeField] protected GameObject CLCommandPrefab;
        [SerializeField] protected GameObject CoreCLCommandPrefab;
        // tracks which command is currently selected. -1 = Input field, 0 - commandList.lenght = command at that index
        public int commandIndex = -1;

        protected State currentState;
        protected List<State> stateHistory = new();
        protected State defaultState;
        protected string dirName = "C";
        protected string defaultDirName;

        protected bool GUIBlock = false;

        [SerializeField] private bool powerTextEnabled = false;
        [SerializeField] private GameObject powerSlot;

        private void Start()
        {
            ResetState();
            ChangeState(currentState);
            currentState?.OnEnter();
        }

        public virtual void ChangeState(State newState)
        {
            currentState?.OnExit();
            currentState = newState;
            UpdateCommands();
            commandIndex = -1;
            ChangeText("");
            currentState.OnEnter();
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
                GameObject commandObj;
                if (command == "core" || command == "run core_extract_protocol")
                {
                    commandObj = Instantiate(CoreCLCommandPrefab, CLCommandsListObj.transform);
                    if (!GameObject.FindObjectOfType<ShipPowerOn>().isPowerOn)
                    {
                        commandObj.GetComponentInChildren<TMP_Text>().alpha = 0.10f;
                    }
                    else
                    {
                        commandObj.GetComponentInChildren<TMP_Text>().alpha = 1f;
                    }
                }
                else
                {
                    commandObj = Instantiate(CLCommandPrefab, CLCommandsListObj.transform);
                }
                commandObj.GetComponentInChildren<TMP_Text>().text = command.ToUpper();
                commandList.Insert(0, commandObj);
                var commandButton = commandObj.GetComponent<Button>();
                commandButton.onClick.AddListener(() => currentState.Interpret(command));
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

        public virtual void ChangeText(string text, float delay = 0, bool waitToFinish = false, Action onFinish = null)
        {
            if (textResponseCoroutine != null)
            {
                StopCoroutine(textResponseCoroutine);
                AudioManager.StopAudio("TerminalTextLoop");
            }

            commandLineText.text = text;
            commandLineText.maxVisibleCharacters = 0;
            commandLineText.ForceMeshUpdate(true);
            if (text.Length > 0)
            {
                if (delay > 0)
                {
                    textResponseCoroutine = StartCoroutine(RoutineDelayedText(commandLineText, delay, waitToFinish, onFinish));
                }
                else
                {
                    textResponseCoroutine = StartCoroutine(RoutineDelayedText(commandLineText, textWriteDelay, waitToFinish, onFinish));
                }
            }
            else
            {
                onFinish?.Invoke(); // invoke immediately if there is no text.
            }
            commandLineInput.text = "";
            commandLineInput.interactable = true;
            commandLineInput.ActivateInputField();
        }

        public virtual void AddText(string text, float delay = 0, bool waitToFinish = false, Action onFinish = null)
        {
            if (textResponseCoroutine != null)
            {
                StopCoroutine(textResponseCoroutine);
                AudioManager.StopAudio("TerminalTextLoop");
            }

            int previousTextLength;
            previousTextLength = commandLineText.textInfo.characterCount;

            commandLineText.text = commandLineText.text + "\r\n" + text;
            commandLineText.maxVisibleCharacters = commandLineText.textInfo.characterCount;
            commandLineText.ForceMeshUpdate(true);

            

            if (text.Length > 0)
            {
                if (delay > 0)
                {
                    textResponseCoroutine = StartCoroutine(RoutineDelayedText(commandLineText, delay, waitToFinish, onFinish, previousTextLength));
                }
                else
                {
                    textResponseCoroutine = StartCoroutine(RoutineDelayedText(commandLineText, textWriteDelay, waitToFinish, onFinish, previousTextLength));
                }
            }
            else
            {
                onFinish?.Invoke(); // invoke immediately if there is no text.
            }
            commandLineInput.text = "";
            commandLineInput.interactable = true;
            commandLineInput.ActivateInputField();

        }
        private IEnumerator RoutineDelayedText(TMP_Text textToDisplay, float timeDelay, bool waitToFinish, Action onFinish, int startFromCount = 0)
        {
            if (!textResponseCoroutineRunning && textToDisplay.text.Length > 0)
            {
                textResponseCoroutineRunning = true;
            }
            else
            {
                yield return null;
            }

            WaitForSecondsRealtime delay = new WaitForSecondsRealtime(timeDelay);

            if (textToDisplay.textInfo == null)
            {
                textResponseCoroutine = null;
                textResponseCoroutineRunning = false; // Coroutine finished;
                yield break;
            }
            else
            {
                textToDisplay.ForceMeshUpdate(true);
                int characterCount = textToDisplay.textInfo.characterCount;

                if (characterCount > 0 && startFromCount < characterCount)
                {
                    for (int i = startFromCount; i < characterCount; ++i)
                    {

                        textToDisplay.maxVisibleCharacters = i + 1;
                        //string delayedText = textToDisplay.Substring(0, i + 1);
                        // Do whatever you need to do with this string, e.g. set text on UI.

                        yield return delay;
                    }
                }
                    textResponseCoroutine = null;
                    textResponseCoroutineRunning = false; // Coroutine finished;

                    if (waitToFinish)
                    {
                        onFinish?.Invoke();
                    }
            }
        }

            private IEnumerator RoutineDelayedFlavourText(TMP_Text textToDisplay, float timeDelay, bool waitToFinish, Action onFinish)
        {
            if (!flavourTextCoroutineRunning && textToDisplay.text.Length > 0)
            {
                flavourTextCoroutineRunning = true;
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
            flavourTextCoroutineRunning = false; // Coroutine finished;

            if (waitToFinish)
            {
                onFinish?.Invoke();
            }
        }

        public virtual void ChangeFlavourText(string text, float delay = 0, bool waitToFinish = false, Action onFinish = null)
        {
            if (flavourTextCoroutine != null)
            {
                StopCoroutine(flavourTextCoroutine);
                AudioManager.StopAudio("TerminalTextLoop");
            }

            flavourText.text = text;
            flavourText.maxVisibleCharacters = 0;
            flavourText.ForceMeshUpdate(true);
            if (text.Length > 0)
            {
                if (delay > 0)
                {
                    flavourTextCoroutine = StartCoroutine(RoutineDelayedFlavourText(flavourText, delay, waitToFinish, onFinish));
                }
                else
                {
                    flavourTextCoroutine = StartCoroutine(RoutineDelayedFlavourText(flavourText, textWriteDelay, waitToFinish, onFinish));
                }
            }
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

            commandLineText.text = "";
            commandLineInput.text = "";
            if (defaultDirName != null)
            {
                dirName = defaultDirName;
            }
            directoryText.text = dirName;

            commandLineInput.interactable = true;
            commandLineInput.ActivateInputField();
        }


        protected void OnEnable()
        {
            PauseGame.Pause(PauseGame.TransitionType.LowPassMusic);
            FindObjectOfType<PauseMenu>().enabled = false;
            VisorChange.UpdateVisor(VisorChange.Visor.Hacking);
            ResetState();
            ChangeState(currentState);
            commandLineInput.interactable = true;
            commandLineInput.ActivateInputField();
            currentState?.OnEnter();
            if (powerTextEnabled)
            {
                powerSlot.SetActive(true);
            }
        }

        private void OnDisable()
        {
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

            if (powerTextEnabled)
            {
                powerSlot.SetActive(false);
            }
        }

        void Update()
        {
            commandLineInput.onFocusSelectAll = true;
            if (textResponseCoroutineRunning | flavourTextCoroutineRunning)
            {
                if (!AudioManager.IsPlaying("TerminalTextLoop"))
                {
                    AudioManager.PlayAudio("TerminalTextLoop", 1, 1, true, null, true);
                }
            }
            else if (!textResponseCoroutineRunning && !flavourTextCoroutineRunning)
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
            else if (Input.anyKeyDown && commandIndex != -1 && !Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.Backspace))
            {
                SelectedChangeInputField();
            }

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
                if (!commandLineInput.isFocused)
                {
                    commandLineInput.interactable = true;
                    commandLineInput.ActivateInputField();
                }

            }
        }

        public virtual string GetCurrentStateDirectoryText()
        {
            return directoryText.text.ToString();
        }

        public virtual void SelectedChangeInputField()
        {
            if (commandIndex != -1) commandList[commandIndex].GetComponentInChildren<TMP_Text>().color = commandColor;
            AudioManager.PlayAudio("TerminalButtonHighlight", 1, 1, false, null, true);
            commandIndex = -1;
            commandLineInput.ActivateInputField();
            commandLineInput.interactable = true;
            commandLineInput.onFocusSelectAll = false;
            commandLineInput.text = Input.inputString;
            commandLineInput.MoveToEndOfLine(false,false);
        }

        public virtual void SelectedChange(bool up)
        {
            if (up)
            {
                if (commandIndex + 1 > commandList.Count - 1) return;
                commandIndex++;
                AudioManager.PlayAudio("TerminalButtonHighlight", 1, 1, false, null, true);
                commandLineInput.DeactivateInputField();
                commandLineInput.interactable = false;
                commandList[commandIndex].GetComponentInChildren<TMP_Text>().color = highlightedColor;
                if (commandIndex - 1 == -1) return;
                if (commandList[commandIndex - 1].GetComponentInChildren<TMP_Text>().text != "CORE"
                    && commandList[commandIndex - 1].GetComponentInChildren<TMP_Text>().text != "RUN CORE_EXTRACT_PROTOCOL")
                {
                    commandList[commandIndex - 1].GetComponentInChildren<TMP_Text>().color = commandColor;
                }
                else
                {
                    commandList[commandIndex - 1].GetComponentInChildren<TMP_Text>().color = coreCommandColor;
                    if (!GameObject.FindObjectOfType<ShipPowerOn>().isPowerOn)
                    {
                        commandList[commandIndex - 1].GetComponentInChildren<TMP_Text>().alpha = 0.10f;
                    }
                    else
                    {
                        commandList[commandIndex - 1].GetComponentInChildren<TMP_Text>().alpha = 1f;
                    }
                }
            }
            else if (commandIndex - 1 >= -1)
            {
                if (commandList[commandIndex].GetComponentInChildren<TMP_Text>().text != "CORE"
                    && commandList[commandIndex].GetComponentInChildren<TMP_Text>().text != "RUN CORE_EXTRACT_PROTOCOL")
                {
                    commandList[commandIndex].GetComponentInChildren<TMP_Text>().color = commandColor;
                }
                else
                {
                    commandList[commandIndex].GetComponentInChildren<TMP_Text>().color = coreCommandColor;
                    if (!GameObject.FindObjectOfType<ShipPowerOn>().isPowerOn)
                    {
                        commandList[commandIndex].GetComponentInChildren<TMP_Text>().alpha = 0.10f;
                    }
                    else
                    {
                        commandList[commandIndex].GetComponentInChildren<TMP_Text>().alpha = 1f;
                    }
                }
                commandIndex--;
                AudioManager.PlayAudio("TerminalButtonHighlight", 1, 1, false, null, true);
                if (commandIndex == -1)
                {
                    commandLineInput.interactable = true;
                    commandLineInput.ActivateInputField();
                    return;
                }
                commandList[commandIndex].GetComponentInChildren<TMP_Text>().color = highlightedColor;
            }
        }
    }
}
