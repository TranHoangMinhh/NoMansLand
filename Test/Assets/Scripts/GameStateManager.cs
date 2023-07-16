using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    
    public static GameStateManager Instance {  get; private set; }

    public enum GameState
    {
        Gameplay,
        Pause
    }
    public GameState currentGameState { get; private set; }

    public event EventHandler<GameStateEventArgs> OnGameStateChange;
    public class GameStateEventArgs : EventArgs
    {
        public GameState state;
    }


    private void Awake()
    {
        Instance = this;
    }

    public void SetState(GameState newGameState)
    {
        if (newGameState == currentGameState) return;

        currentGameState = newGameState;
        OnGameStateChange?.Invoke(this, new GameStateEventArgs { state = newGameState });
    }

}
