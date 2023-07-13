using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private GameObject buttonHoverFX;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private bool isQuitButton;
    [SerializeField] private GameObject selectedFX;

    private enum ButtonType
    {
        None,
        MainMenuButton,
        ChooseLoadoutButton
    }
    [SerializeField] private ButtonType buttonType;
    
    private float _increasedPositionTo;

    private float _defaultFontSize;
    private Vector3 _defaultPosition;
    private Color _defaultColor;

    private Color _hoverColor;
    private Vector3 _hoverPosition;

    private StartButton _startButton;
    private bool _isStartButton;


    private void Start()
    {
        _isStartButton = TryGetComponent<StartButton>(out _startButton);

        if (buttonText != null)
        {
            _defaultFontSize = buttonText.fontSize;
            _defaultPosition = buttonText.transform.position;
            _defaultColor = buttonText.color;
        }

        if (buttonType.ToString() == "MainMenuButton")
        {
            _increasedPositionTo = 0.2513f * _defaultPosition.x;

            _hoverColor = new Color(0.2392157f, 0.2392157f, 0.2392157f, 1);  // Color code: 3D3D3D (Dark gray)
            _hoverPosition = new Vector3(_defaultPosition.x + _increasedPositionTo, _defaultPosition.y, _defaultPosition.z);
        }

        if (buttonType.ToString() == "ChooseLoadoutButton")
        {
            GetComponent<Button>().onClick.AddListener(ChooseLoadoutClicked);
        }

        // Reset effect on start
        ResetButtonFX();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ButtonHoverFX();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ResetButtonFX();
    }

    private void ButtonHoverFX()
    {
        buttonHoverFX.SetActive(true);
        //Debug.Log(buttonType.ToString());

        if (buttonType.ToString() == "MainMenuButton")
        {
            buttonText.fontSize = 42f;

            if (!isQuitButton)
                buttonText.color = _hoverColor;

            buttonText.transform.position = _hoverPosition;
        }

        if (_isStartButton)
        {
            StartButton.Instance.OnHoverButtonChange();
        }
    }

    private void ResetButtonFX()
    {
        buttonHoverFX.SetActive(false);

        if (buttonType.ToString() == "MainMenuButton")
        {
            buttonText.fontSize = _defaultFontSize;
            buttonText.color = _defaultColor;
            buttonText.transform.position = _defaultPosition;
        }

        if (_isStartButton)
        {
            StartButton.Instance.ResetButtonChange();
        }
    }

    private void ChooseLoadoutClicked()
    {
        selectedFX.SetActive(true);
    }

    public void RemoveClickedFX()
    {
        selectedFX.SetActive(false);
    }

    public bool HasButtonClicked()
    {
        return selectedFX.activeSelf;
    }
}
