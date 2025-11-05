using System;
using UnityEngine;

public enum GameState
{
    Exploration,
    Meltdown,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState gameState;

    public static event Action<GameState> OnGameStateChanged;

    public GameObject player;

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
        UpdateGameState(GameState.Exploration);
    }

    public void UpdateGameState(GameState state)
    {
        gameState = state;

        switch (state)
        {
            case GameState.Exploration:
                break;
            case GameState.Meltdown:
                break;
            case GameState.GameOver:
                break;
            default:
                throw new System.ArgumentOutOfRangeException(nameof(state), state, null);
        }

        OnGameStateChanged?.Invoke(state);
    }
}