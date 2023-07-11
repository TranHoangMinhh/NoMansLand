using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unity.Netcode;

public class PlayerTemplateUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private Image playerCharacterImage;
    [SerializeField] private TextMeshProUGUI playerCharacterText;
    [SerializeField] private Button kickPlayerButton;

    //private Dictionary<LobbyManager.PlayerCharacter, Sprite> _playerCharacterDictionary;
    //private Dictionary<LobbyManager.SideWeapon, Sprite> _sideWeaponDictionary;

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

        playerNameText.text = player.Data[LobbyManager.KEY_PLAYER_NAME].Value;

        // For choosing side weapon scene
        //LobbyManager.SideWeapon sideWeapon = System.Enum.Parse<LobbyManager.SideWeapon>(player.Data[LobbyManager.KEY_SIDE_WEAPON].Value);

        //LobbyManager.Instance.UpdatePlayerSideWeapon(sideWeapon);

        LobbyManager.PlayerCharacter playerCharacter = System.Enum.Parse<LobbyManager.PlayerCharacter>(player.Data[LobbyManager.KEY_PLAYER_CHARACTER].Value);

        playerCharacterImage.sprite = CharacterSprites.Instance.GetCharacterSprite(playerCharacter);

        if (playerCharacter != LobbyManager.PlayerCharacter.None)
        {
            playerCharacterText.text = playerCharacter.ToString();
        }
        else
        {
            playerCharacterText.text = "(Picking...)";
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Exit");
    }
}
