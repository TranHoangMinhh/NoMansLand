using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomUI : MonoBehaviour
{
    
    public static CreateRoomUI Instance { get; private set; }

    //! Add map options
    [SerializeField] private TMP_InputField roomNameInputField;
    [SerializeField] private Slider maxPlayerSilder;
    [SerializeField] private Slider durationSlider;
    [SerializeField] private Toggle isPrivateToggle;
    [SerializeField] Button createRoomButton;

    private string _roomName;
    private int _maxPlayer;
    private string _duration;
    private bool _isPrivate;

    //! Add customize map
    private LobbyManager.Maps _map;

    private void Awake()
    {
        Instance = this;

        // Adding listener when the button clicked
        createRoomButton.onClick.AddListener(() =>
        {
            UpdateInputValue();  // Update player's choice and input on the UI
            LobbyManager.Instance.CreateLobby(_roomName, _maxPlayer, _isPrivate, _duration);
            Hide();
        });
    }

    private void UpdateInputValue()
    {
        _roomName = roomNameInputField.text;
        _maxPlayer = (int)maxPlayerSilder.value;
        _duration = durationSlider.value.ToString();
        _isPrivate = isPrivateToggle.isOn;
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

}
