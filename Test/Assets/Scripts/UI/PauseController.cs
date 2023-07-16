using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{

    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject pauseScreen;

    private StarterAssetsInputs _input;


    private void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        if (_input.pause)
        {
            Debug.Log("Pause Game");

            hud.SetActive(false);
            pauseScreen.SetActive(true);

            GameStateManager.GameState _currentGameState = GameStateManager.Instance.currentGameState;
            GameStateManager.GameState newGameState = _currentGameState == GameStateManager.GameState.Gameplay
                ? GameStateManager.GameState.Pause
                : GameStateManager.GameState.Gameplay;

            GameStateManager.Instance.SetState(newGameState);
        }
    }

}
