using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerProfile : MonoBehaviour
{

    public static PlayerProfile Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI playerNameTMP;
    // [SerializeField] private Image playerAvatar;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetPlayerName(LobbyManager.Instance.GetPlayerName());
    }

    public void SetPlayerName(string playerName)
    {
        playerNameTMP.text = playerName;
    }

}
