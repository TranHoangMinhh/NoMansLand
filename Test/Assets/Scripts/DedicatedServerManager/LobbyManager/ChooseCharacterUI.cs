using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class ChooseCharacterUI : MonoBehaviour
{
    
    public static ChooseCharacterUI Instance { get; private set; }

    [Header("Room Content")]
    [SerializeField] private Transform playerTemplateUI;
    [SerializeField] private Transform playerListContainer;
    [SerializeField] private GameObject character;
    [SerializeField] private Transform characterShowcase;
    [SerializeField] private TextMeshProUGUI characterNameText;

    [Header("Buttons")]
    [SerializeField] private List<Button> changeCharacterButtonList;
    [SerializeField] private Button LockInButton;

    [Header("Navigation")]
    [SerializeField] private GameObject chooseSideWeaponScene;

    private Dictionary<int, Transform> characterSkinsDictionary;

    //private List<Transform> _characterList;


    private void Awake()
    {
        Instance = this;

        playerTemplateUI.gameObject.SetActive(false);

        LoadButtonList();

        LockInButton.onClick.AddListener(OpenChooseSideWeaponScene);
    }

    private void Start()
    {
        LobbyManager.Instance.OnJoinedLobby += UpdateLobby_Event;
        LobbyManager.Instance.OnJoinedLobbyUpdate += UpdateLobby_Event;

        characterSkinsDictionary = new Dictionary<int, Transform>();
        character.SetActive(false);

        //Hide();
    }

    private void UpdateLobby_Event(object sender, LobbyManager.LobbyEventArgs e)
    {
        UpdateRoom(e.lobby);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    // Add button function to all choose character button
    private void LoadButtonList()
    {

        foreach (Button button in changeCharacterButtonList)
        {
            button.onClick.AddListener(() =>
            {
                LobbyManager.Instance.UpdatePlayerCharacter(button.GetComponent<ChooseObjectInfo>().GetCharacterType());
                SetCharacterObjectActive(button.GetComponent<ChooseObjectInfo>().GetCharacterType());
                characterNameText.text = button.GetComponent<ChooseObjectInfo>().GetCharacterType().ToString();
                int skinId = button.GetComponent<ChooseObjectInfo>().GetIndex();
                NMLGameMultiplayer.Instance.ChangePlayerSkin(skinId);
                RemoveSelectedFX();

                if (!character.activeSelf)
                {
                    character.SetActive(true);
                }
            });
        }
    }

    public void SetCharacterObjectActive(LobbyManager.PlayerCharacter character)
    {
        string characterString = "Character_" + character.ToString();
        foreach (Transform child in characterShowcase)
        {
            if (child.name == characterString)
            {
                DisableAllCharacterObject();
                child.gameObject.SetActive(true);
                break;
            }
        }
    }

    private void DisableAllCharacterObject()
    {
        foreach (Transform child in characterShowcase)
        {
            if (child.name != "root")
                child.gameObject.SetActive(false);
        }
    }

    private void RemoveSelectedFX()
    {
        foreach (Button button in changeCharacterButtonList)
        {
            if (button.GetComponent<ButtonBehavior>().HasButtonClicked())
            {
                button.GetComponent<ButtonBehavior>().RemoveClickedFX();
            }
        }
    }

    private void ClearRoom()
    {
        foreach (Transform child in playerListContainer)
        {
            if (child == playerTemplateUI) continue;
            Destroy(child.gameObject);
        }
    }

    //private void UpdateRoom()
    //{
    //    UpdateRoom(LobbyManager.Instance.GetJoinedLobby());
    //}

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

            // Update player UI
            newPlayerTemplate.UpdatePlayer(player);
        }

        //Show();
    }

    private void OpenChooseSideWeaponScene()
    {
        Hide();
        chooseSideWeaponScene.SetActive(true);
    }
}
