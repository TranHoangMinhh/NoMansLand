using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI m_TextMeshProUGUI;
    private float defaultFontSize;

    private float increasedFontSize;

    private void Start()
    {
        m_TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
        defaultFontSize = m_TextMeshProUGUI.fontSize;
        increasedFontSize = m_TextMeshProUGUI.fontSize * 0.25f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_TextMeshProUGUI.fontSize = m_TextMeshProUGUI.fontSize + increasedFontSize;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_TextMeshProUGUI.fontSize = defaultFontSize;
    }
}
