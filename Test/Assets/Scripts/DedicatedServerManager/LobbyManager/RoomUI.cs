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

    [Header("Room Content")]
    [SerializeField] private Transform playerTemplateUI;
    [SerializeField] private Transform playerListContainer;
    [SerializeField] private TextMeshProUGUI roomNameText;

    [Header("Room Code")]
    [SerializeField] private Button roomCodeButton;
    [SerializeField] private TextMeshProUGUI roomCodeText;

    [Header("Buttons")]
    [SerializeField] private Button leaveRoomButton;
    [SerializeField] private Button startGameButton;

    private string _roomCode;


    private void Awake()
    {
        Instance = this;

        playerTemplateUI.gameObject.SetActive(false);
        
        // Adding listener when the button clicked
        leaveRoomButton.onClick.AddListener(() => {
            LobbyManager.Instance.LeaveLobby();
        });

        //roomCodeButton.onClick.AddListener(CopyCodeToClipboard);
        
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

            // Check if this is their own player, i.e. player screen which can be interactable
            bool isPlayer = player.Id == LobbyManager.Instance.GetPlayerID();

            // Set visibility to kick player button. Kick player button only visible to host
            newPlayerTemplate.SetKickPLayerButtonVisible(
                LobbyManager.Instance.IsLobbyHost() &&
                !isPlayer  // Not allow player to kick itself
            );

            newPlayerTemplate.SetChangeCharacterButtonVisible( isPlayer );
            newPlayerTemplate.SetChangeSideWeaponInteractable( isPlayer );

            // Update player UI
            newPlayerTemplate.UpdatePlayer(player);
        }

        _roomCode = lobby.LobbyCode;

        roomNameText.text = lobby.Name;
        roomCodeText.text = $"Code: {lobby.LobbyCode}";
        //playerCountText.text = $"{lobby.Players.Count}/{lobby.MaxPlayers} Players";

        Show();
    }

    private void CopyCodeToClipboard()
    {
        GUIUtility.systemCopyBuffer = _roomCode;
    }

    private void StartGame()
    {
        //! Add functions to start the game
        Debug.Log("Start Game!!!");
        LoadingManager.Instance.LoadScene(LoadingManager.Scenes.GameScene);
    }
}
