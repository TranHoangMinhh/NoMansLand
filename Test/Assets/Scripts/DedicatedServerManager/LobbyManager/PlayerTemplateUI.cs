using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unity.Netcode;

public class PlayerTemplateUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private Button kickPlayerButton;
    [SerializeField] private Image characterImage;
    [SerializeField] private TextMeshProUGUI characterName;

    private enum OnScene
    {
        LobbyScene,
        ChooseScene
    }
    [SerializeField] private OnScene onScene;

    private Player _player;


    private void Awake()
    {
        if (onScene.ToString() == "LobbyScene")
        {
            kickPlayerButton.onClick.AddListener(KickPlayer);
        }
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

        playerNameText.text = player.Data[LobbyManager.KEY_PLAYER_NAME].Value;

        // For choosing side weapon scene
        //LobbyManager.SideWeapon sideWeapon = System.Enum.Parse<LobbyManager.SideWeapon>(player.Data[LobbyManager.KEY_SIDE_WEAPON].Value);

        //LobbyManager.Instance.UpdatePlayerSideWeapon(sideWeapon);

        if (onScene.ToString() == "ChooseScene")
        {
            LobbyManager.PlayerCharacter playerCharacter = System.Enum.Parse<LobbyManager.PlayerCharacter>(player.Data[LobbyManager.KEY_PLAYER_CHARACTER].Value);

            characterImage.sprite = CharacterSprites.Instance.GetCharacterSprite(playerCharacter);

            if (playerCharacter != LobbyManager.PlayerCharacter.None)
            {
                characterName.text = playerCharacter.ToString();
            }
            else
            {
                characterName.text = "(Picking...)";
            }
        }
    }
}
