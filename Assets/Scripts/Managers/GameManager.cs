using System;
using UnityEngine;

public enum GameState
{
    MainMenu,
    Gameplay,
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

    [SerializeField] private GameState gameState;
    [SerializeField] private Phase phase;

    public static event Action<GameState> OnGameStateChanged;
    public static event Action<Phase> OnPhaseChanged;

    [HideInInspector] public GameObject player;

    public static bool isPaused;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        player = GameObject.FindWithTag("Player");
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
}