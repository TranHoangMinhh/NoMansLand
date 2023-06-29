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
    [SerializeField] private TextMeshProUGUI playerSideWeaponText;
    [SerializeField] private Button kickPlayerButton;

    private Player _player;


    private void Awake()
    {
        kickPlayerButton.onClick.AddListener(KickPlayer);
    }

    public void SetKickPLayerButtonVisible(bool visible)
    {
        kickPlayerButton.gameObject.SetActive(visible);
    }

    private void KickPlayer()
    {
        if (_player != null)
        {
            LobbyManager.Instance.KickPlayer(_player.Id);
        }
    }

    public void UpdatePlayer(Player player)
    {
        _player = player;

        // Change text on UI
        playerNameText.text = player.Data[LobbyManager.KEY_PLAYER_NAME].Value;
        playerSideWeaponText.text = player.Data[LobbyManager.KEY_SIDE_WEAPON].Value;

        // Update player's side weapon and character
        LobbyManager.PlayerCharacter playerCharacter = System.Enum.Parse<LobbyManager.PlayerCharacter>(player.Data[LobbyManager.KEY_PLAYER_CHARACTER].Value);
        LobbyManager.SideWeapon sideWeapon = System.Enum.Parse<LobbyManager.SideWeapon>(player.Data[LobbyManager.KEY_SIDE_WEAPON].Value);
    }
}
