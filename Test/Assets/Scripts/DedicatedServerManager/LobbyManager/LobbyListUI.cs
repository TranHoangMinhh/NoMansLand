using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyListUI : MonoBehaviour
{
    public static LobbyListUI Instance { get; private set; }

    [Header("Scene Title")]
    [SerializeField] private TextMeshProUGUI sceneTitle;

    [Space(10)]
    [Header("Lobby List")]
    [SerializeField] private Transform lobbyTemplate;
    [SerializeField] private Transform lobbyListContainer;
    [SerializeField] private Button refreshButton;
    [SerializeField] private Button backButton;

    [Space(10)]
    [Header("Join/Create Room")]
    [SerializeField] private GameObject createRoomUI;
    [SerializeField] private Button createRoomButton;

    [Space(5)]
    [SerializeField] private TMP_InputField inputRoomCodeText;
    [SerializeField] private Button joinRoomButton;

    private void Awake()
    {
        Instance = this;

        lobbyTemplate.gameObject.SetActive(false);
        refreshButton.onClick.AddListener(RefreshButtonClick);
        createRoomButton.onClick.AddListener(CreateLobbyButtonClick);  //! Reserved for new UI

        joinRoomButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.JoinLobbyByCode(inputRoomCodeText.text);
        });

        backButton.onClick.AddListener(LoadMainMenuScene);
    }

    private void Start()
    {
        // Subscribe to event on Lobby Manager
        LobbyManager.Instance.OnLobbyListChanged += LobbyManager_OnLobbyListChanged;
        LobbyManager.Instance.OnJoinedLobby += LobbyManager_OnJoinedLobby;
        LobbyManager.Instance.OnLeftLobby += LobbyManager_OnLeftLobby;
        LobbyManager.Instance.OnKickedFromLobby += LobbyManager_OnKickedFromLobby;

        sceneTitle.text = "lobby";
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
        LobbyManager.Instance.RefreshLobbyList();
    }

    private void LobbyManager_OnLobbyListChanged(object sender, LobbyManager.OnLobbyListChangedEventArgs e)
    {
        UpdateLobbyList(e.lobbyList);
    }

    private void LobbyManager_OnJoinedLobby(object sender, LobbyManager.LobbyEventArgs e)
    {
        Hide();
        //roomUI.SetActive(true);
        LoadingManager.Instance.LoadScene(LoadingManager.Scenes.ChooseScene);
    }

    private void LobbyManager_OnLeftLobby(object sender, EventArgs e)
    {
        Show();
        //roomUI.SetActive(false);
    }

    private void LobbyManager_OnKickedFromLobby(object sender, LobbyManager.LobbyEventArgs e)
    {
        Show();
        //roomUI.SetActive(false);
    }

    //! Reserved for new UI
    //private void CreateLobbyButtonClick()
    //{
    //    throw new NotImplementedException();
    //}

    private void LoadMainMenuScene()
    {
        Loader.Load(Loader.Scene.MainMenuScene);
    }

    private void CreateLobbyButtonClick()
    {
        Hide();
        createRoomUI.gameObject.SetActive(true);
    }

    private void RefreshButtonClick()
    {
        LobbyManager.Instance.RefreshLobbyList();
    }

    private void UpdateLobbyList(List<Lobby> lobbyList)
    {
        // Search through all lobby and destroy game object for new lobby add later on
        foreach (Transform child in lobbyListContainer)
        {
            if (child == lobbyTemplate) continue;  // Do not remove the lobby template

            Destroy(child.gameObject);
        }

        // Go through the list and create lobby entry from the lobby template
        foreach (Lobby lobby in lobbyList)
        {
            Transform lobbyTemplateTransform = Instantiate(lobbyTemplate, lobbyListContainer);
            lobbyTemplateTransform.gameObject.SetActive(true);
            LobbyTemplateUI lobbyTemplateUI = lobbyTemplateTransform.GetComponent<LobbyTemplateUI>();
            lobbyTemplateUI.UpdateLobby(lobby);

#if UNITY_EDITOR
            Debug.Log($"Room name: {lobby.Name}; Max Player: {lobby.MaxPlayers}; In room: {lobby.Players.Count}");
#endif
        }
    }

}
