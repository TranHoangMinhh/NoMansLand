using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    [SerializeField] private Button openJoinLobbyButton;
    [SerializeField] private Button openCreateRoomButton;
    [SerializeField] private Button openSettingsButton;
    [SerializeField] private Button quitGameButton;

    [SerializeField] private GameObject joinLobbyScene;
    [SerializeField] private GameObject createRoomScene;
    [SerializeField] private GameObject settingsScene;


    private void Awake()
    {
        openJoinLobbyButton.onClick.AddListener(OpenJoinLobby);
        openCreateRoomButton.onClick.AddListener(OpenCreateRoom);
        openSettingsButton.onClick.AddListener(OpenSettings);
        quitGameButton.onClick.AddListener(QuitGame);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void OpenJoinLobby()
    {
        Hide();
        joinLobbyScene.SetActive(true);
    }

    private void OpenCreateRoom()
    {
        Hide();
        createRoomScene.SetActive(true);
    }

    private void OpenSettings()
    {
        Hide();
        settingsScene.SetActive(true);
    }

    public void StartGame(int sceneId)
    {
        LoadingManager.Instance.LoadScene(sceneId);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
