using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyTemplateUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI lobbyNameText;
    [SerializeField] TextMeshProUGUI mapText;
    [SerializeField] TextMeshProUGUI playerCountText;

    private Lobby _lobby;

    private void Awake()
    {
        // Add listener for click action: user will join lobby on click using lobby ID
        GetComponent<Button>().onClick.AddListener(() =>
        {
            LobbyManager.Instance.JoinLobbyByID(_lobby);
            NMLGameMultiplayer.Instance.StartClient();
        });
    }

    public void UpdateLobby(Lobby lobby)
    {
        _lobby = lobby;

        lobbyNameText.text = lobby.Name;
        mapText.text = lobby.Data[LobbyManager.KEY_MAP].Value;
        playerCountText.text = lobby.Players.Count + "/" + lobby.MaxPlayers;
    }
}
