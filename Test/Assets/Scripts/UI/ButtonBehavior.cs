using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private GameObject buttonHoverFX;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private bool isQuitButton;
    [SerializeField] private bool isMainMenuButton = true;
    
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

        _defaultFontSize = buttonText.fontSize;
        _defaultPosition = buttonText.transform.position;
        _defaultColor = buttonText.color;

        if (isMainMenuButton)
        {
            _increasedPositionTo = 0.2513f * _defaultPosition.x;

            _hoverColor = new Color(0.2392157f, 0.2392157f, 0.2392157f, 1);  // Color code: 3D3D3D (Dark gray)
            _hoverPosition = new Vector3(_defaultPosition.x + _increasedPositionTo, _defaultPosition.y, _defaultPosition.z);
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

        if (isMainMenuButton)
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

        if (isMainMenuButton)
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
}
