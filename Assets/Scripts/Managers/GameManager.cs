using System;
using UnityEngine;

public enum GameState
{
    MainMenu,
    Gameplay,
    Hacking,
    PauseMenu,
    Cutscene,
    GameOver
}

public enum Phase
{
    Exploration,
    Meltdown
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState gameState;
    public Phase phase;

    public static event Action<GameState> OnGameStateChanged;
    public static event Action<Phase> OnPhaseChanged;

    public static bool isPaused;

    [HideInInspector] public bool instantGameOverRequested = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        UpdateGameState(GameState.Gameplay);
    }

    public void UpdateGameState(GameState newState)
    {
        gameState = newState;

        switch (newState)
        {
            case GameState.MainMenu:
                if (isPaused) Resume();
                break;
            case GameState.Gameplay:
                if (isPaused) Resume();
                break;
            case GameState.Hacking:
                Pause();
                break;
            case GameState.PauseMenu:
                Pause();
                break;
            case GameState.Cutscene:
                Pause();
                break;
            case GameState.GameOver:
                isPaused = true;
                break;
            default:
                throw new System.ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }

    public void UpdatePhase(Phase newPhase)
    {
        phase = newPhase;

        switch (phase)
        {
            case Phase.Exploration:
                break;
            case Phase.Meltdown:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newPhase), newPhase, null);
        }

        OnPhaseChanged?.Invoke(newPhase);
    }

    public static void Pause()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        isPaused = true;
    }

    public static void Resume()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
    }

    public void KillPlayer(bool instant = false)
    {
        instantGameOverRequested = instant;
        UpdateGameState(GameState.GameOver);
        instantGameOverRequested = false;
    }
}