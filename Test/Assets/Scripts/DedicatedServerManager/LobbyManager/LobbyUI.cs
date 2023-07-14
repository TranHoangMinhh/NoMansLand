using System;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{

    [Header("Player List")]
    [SerializeField] private Transform playerListContainer;
    [SerializeField] private Transform listView;
    [SerializeField] private Transform playerTemplate;

    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button leaveRoomButton;

    [Header("Room Information")]
    [SerializeField] private Button roomCodeButton;
    [SerializeField] private TextMeshProUGUI roomCodeText;
    [SerializeField] private TextMeshProUGUI roomName;
    [SerializeField] private TextMeshProUGUI roomMap;
    [SerializeField] private TextMeshProUGUI roomPlayerCount;

    [Header("Navigation")]
    [SerializeField] private GameObject previousScene;

    private string _roomCode;
    private bool _hasListViewInstantiate = false;
    private bool _hasSceneChange = false;


    private void Awake()
    {
        playerTemplate.gameObject.SetActive(false);

        startButton.onClick.AddListener(() => {
            Hide();
            //_hasSceneChange = true;
            //Loader.LoadNetwork(Loader.Scene.ChooseScene);
            LoadingManager.Instance.LoadSceneNetwork(LoadingManager.Scenes.ChooseScene);
        });

        leaveRoomButton.onClick.AddListener(() => {
            LobbyManager.Instance.LeaveLobby();
            NetworkManager.Singleton.Shutdown();
            previousScene.SetActive(true);
        });

        roomCodeButton.onClick.AddListener(CopyCodeToClipboard);
    }

    private void Start()
    {
        LobbyManager.Instance.OnJoinedLobby += UpdateLobby_Event;
        LobbyManager.Instance.OnJoinedLobbyUpdate += UpdateLobby_Event;
        LobbyManager.Instance.OnLeftLobby += LobbyManager_OnLeftLobby;
        LobbyManager.Instance.OnKickedFromLobby += LobbyManager_OnLeftLobby;

        SceneManager.activeSceneChanged += OnChangedActiveScene;

        Hide();
    }

    private void OnChangedActiveScene(Scene current, Scene next)
    {
        _hasSceneChange = true;
    }

    private void LobbyManager_OnLeftLobby(object sender, EventArgs e)
    {
        if (!_hasSceneChange)
        {
            ClearList();
            Hide();
        }
    }

    private void UpdateLobby_Event(object sender, LobbyManager.LobbyEventArgs e)
    {
        if (!_hasSceneChange)
        {
            UpdatePlayerList(e.lobby);
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ClearList()
    {
        foreach (Transform child in playerListContainer)
        {
            foreach (Transform subChild in child)
            {
                if (subChild == playerTemplate) continue;
                Destroy(subChild.gameObject);
            }

            //Destroy(child.gameObject);
        }
    }

    //private void UpdatePlayerList()
    //{
    //    UpdatePlayerList(LobbyManager.Instance.GetJoinedLobby());
    //}

    private void UpdatePlayerList(Lobby lobby)
    {
        ClearList();

        foreach (Player player in lobby.Players)
        {
            Transform _listView = listView;
            if (listView.childCount >= 4 && _hasListViewInstantiate == false)
            {
                _listView = Instantiate(listView, playerListContainer);
                _hasListViewInstantiate = true;
            }
            // Create a player entry in the list using PlayerTemplateUI
            Transform playerTemplateTransform = Instantiate(playerTemplate, _listView);
            playerTemplateTransform.gameObject.SetActive(true);
            PlayerTemplateUI newPlayerTemplate = playerTemplateTransform.GetComponent<PlayerTemplateUI>();

            // Check if this is their own player, i.e. player screen which can be interactable
            bool isPlayer = player.Id == LobbyManager.Instance.GetPlayerID();

            // Set visibility to kick player button. Kick player button only visible to host
            newPlayerTemplate.SetKickPLayerButtonVisible(
                LobbyManager.Instance.IsLobbyHost() &&
                !isPlayer  // Not allow player to kick itself
            );

            // Update player UI
            newPlayerTemplate.UpdatePlayer(player);
        }

        roomCodeText.text = lobby.LobbyCode;
        _roomCode = lobby.LobbyCode;

        roomName.text = LobbyManager.Instance.GetJoinedLobby().Name;
        roomMap.text = LobbyManager.Instance.GetJoinedLobby().Data[LobbyManager.KEY_MAP].Value;
        roomPlayerCount.text = $"{lobby.Players.Count}/{lobby.MaxPlayers} Players";

        Show();
    }

    private void CopyCodeToClipboard()
    {
        GUIUtility.systemCopyBuffer = _roomCode;
    }

}
