using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class RoomUI : MonoBehaviour
{
    
    public static RoomUI Instance { get; private set; }

    [SerializeField] private Transform playerTemplateUI;
    [SerializeField] private Transform playerListContainer;
    [SerializeField] private TextMeshProUGUI roomNameText;
    [SerializeField] private TextMeshProUGUI roomIDText;
    [SerializeField] private TextMeshProUGUI playerCountText;

    [SerializeField] private Button leaveRoomButton;
    [SerializeField] private Button startGameButton;


    private void Awake()
    {
        Instance = this;

        playerTemplateUI.gameObject.SetActive(false);
        
        // Adding listener when the button clicked
        leaveRoomButton.onClick.AddListener(() => {
            LobbyManager.Instance.LeaveLobby();
        });
        
        startGameButton.onClick.AddListener(StartGame);
    }

    private void Start()
    {
        LobbyManager.Instance.OnJoinedLobby += UpdateLobby_Event;
        LobbyManager.Instance.OnJoinedLobbyUpdate += UpdateLobby_Event;
        LobbyManager.Instance.OnLeftLobby += LobbyManager_OnLeftLobby;
        LobbyManager.Instance.OnKickedFromLobby += LobbyManager_OnLeftLobby;

        Hide();
    }

    private void UpdateLobby_Event(object sender, LobbyManager.LobbyEventArgs e)
    {
        UpdateRoom();
    }

    private void LobbyManager_OnLeftLobby(object sender, System.EventArgs e)
    {
        ClearRoom();
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void ClearRoom()
    {
        foreach (Transform child in playerListContainer)
        {
            if (child == playerTemplateUI) continue;
            Destroy(child.gameObject);
        }
    }

    private void UpdateRoom()
    {
        UpdateRoom(LobbyManager.Instance.GetJoinedLobby());
    }

    private void UpdateRoom(Lobby lobby)
    {
        ClearRoom();

        foreach (Player player in lobby.Players)
        {
            // Create a player entry in the list using PlayerTemplateUI
            Transform playerTemplateTransform = Instantiate(playerTemplateUI, playerListContainer);
            playerTemplateTransform.gameObject.SetActive(true);
            PlayerTemplateUI newPlayerTemplate = playerTemplateTransform.GetComponent<PlayerTemplateUI>();

            // Set visibility to kick player button. Kick player button only visible to host
            newPlayerTemplate.SetKickPLayerButtonVisible(
                LobbyManager.Instance.IsLobbyHost() &&
                player.Id != LobbyManager.Instance.GetPlayerID()  // Not allow player to kick itself
            );

            // Update player UI
            newPlayerTemplate.UpdatePlayer(player);
        }

        roomNameText.text = lobby.Name;
        roomIDText.text = $"Code: {lobby.LobbyCode}";
        playerCountText.text = $"{lobby.Players.Count}/{lobby.MaxPlayers} Players";

        Show();
    }

    private void StartGame()
    {
        //! Add functions to start the game
        Debug.Log("Start Game!!!");
        LoadingManager.Instance.LoadScene(1);
    }
}