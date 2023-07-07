using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    
    public static StartButton Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI buttonText;

    private float _defaultFontSize;
    private Color _defaultColor;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _defaultFontSize = buttonText.fontSize;
        _defaultColor = buttonText.color;
    }

    public void OnHoverButtonChange()
    {
        buttonText.fontSize = _defaultFontSize + 4f;
        buttonText.color = new Color(0.8980392f, 0.8823529f, 0.1568628f);
    }

    public void ResetButtonChange()
    {
        buttonText.fontSize = _defaultFontSize;
        buttonText.color = _defaultColor;
    }

}
