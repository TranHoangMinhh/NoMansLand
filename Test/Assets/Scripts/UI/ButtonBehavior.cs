using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI m_TextMeshProUGUI;
    private float defaultFontSize;
    private Color defaultColor;

    private void Awake()
    {
        m_TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
        defaultFontSize = m_TextMeshProUGUI.fontSize;
        defaultColor = m_TextMeshProUGUI.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_TextMeshProUGUI.fontSize = 50f;
        m_TextMeshProUGUI.color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_TextMeshProUGUI.fontSize = defaultFontSize;
        m_TextMeshProUGUI.color = defaultColor;
    }
}
