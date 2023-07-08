using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTemplateUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private Button playerSideWeapon;
    [SerializeField] private Button playerCharacter;
    [SerializeField] private Button kickPlayerButton;
    
    // Icon list
    [Space(5)]
    [SerializeField] private List<Sprite> sideWeapons;
    [SerializeField] private List<Sprite> characters;

    //private Dictionary<LobbyManager.PlayerCharacter, Sprite> _playerCharacterDictionary;
    //private Dictionary<LobbyManager.SideWeapon, Sprite> _sideWeaponDictionary;

    private Player _player;

    private Image _characterImage;
    private Image _sideWeaponImage;

    private int _characterIndex;
    private int _sideWeaponIndex;

    public enum  UpdateMode
    {
        Room,
        SideWeapon,
        Character,
        Name
    }


    private void Awake()
    {
        kickPlayerButton.onClick.AddListener(KickPlayer);
        playerCharacter.onClick.AddListener(ChangePlayerCharacter);
        playerSideWeapon.onClick.AddListener(ChangePlayerSideWeapon);
    }

    private void Start()
    {
        _characterImage = playerCharacter.GetComponentInChildren<Image>();
        _sideWeaponImage = playerSideWeapon.GetComponentInChildren<Image>();

        _characterIndex = 0;
        _sideWeaponIndex = 0;
    }

    public void SetKickPLayerButtonVisible(bool visible)
    {
        kickPlayerButton.gameObject.SetActive(visible);
    }

    public void SetChangeCharacterButtonVisible(bool visible)
    {
        playerCharacter.gameObject.SetActive(visible);
    }

    private void InsertListToDictionary()
    {
        foreach (var item in sideWeapons)
        {

        }
    }

    private void KickPlayer()
    {
        if (_player != null)
        {
            LobbyManager.Instance.KickPlayer(_player.Id);
        }
    }

    public void UpdatePlayer(Player player, UpdateMode mode = UpdateMode.Room)
    {
        _player = player;

        switch (mode)
        {
            case UpdateMode.SideWeapon:
                if (_sideWeaponIndex >= sideWeapons.Count)
                    _sideWeaponIndex = 0;
                else
                    _sideWeaponIndex++;

                playerSideWeapon.GetComponentInChildren<Image>().sprite = sideWeapons[_sideWeaponIndex];

                LobbyManager.Instance.UpdatePlayerSideWeapon(
                    System.Enum.Parse<LobbyManager.SideWeapon>(player.Data[LobbyManager.KEY_SIDE_WEAPON].Value)
                );
                break;

            case UpdateMode.Character:
                if (_characterIndex >= characters.Count)
                    _characterIndex = 0;
                else
                    _characterIndex++;

                playerCharacter.GetComponentInChildren<Image>().sprite = characters[_characterIndex];

                LobbyManager.Instance.UpdatePlayerCharacter(
                    System.Enum.Parse<LobbyManager.PlayerCharacter>(player.Data[LobbyManager.KEY_PLAYER_CHARACTER].Value)
                );
                break;

            case UpdateMode.Name:
                // Change text on UI
                playerNameText.text = player.Data[LobbyManager.KEY_PLAYER_NAME].Value;
                LobbyManager.Instance.UpdatePlayerName( playerNameText.text );
                break;

            default:
                playerNameText.text = player.Data[LobbyManager.KEY_PLAYER_NAME].Value;
                break;
        }
    }

    private void ChangePlayerSideWeapon()
    {
        //UpdatePlayer(_player, UpdateMode.SideWeapon);
        Debug.Log(_player.Data[LobbyManager.KEY_SIDE_WEAPON].Value);
    }

    private void ChangePlayerCharacter()
    {
        UpdatePlayer(_player, UpdateMode.Character);
        Debug.Log(_player.Data[LobbyManager.KEY_PLAYER_CHARACTER].Value);
    }

    public void SetChangeSideWeaponInteractable(bool interactable)
    {
        playerSideWeapon.interactable = interactable;
    }
}
