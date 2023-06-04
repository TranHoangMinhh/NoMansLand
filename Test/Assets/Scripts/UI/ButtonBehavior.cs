using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI m_TextMeshProUGUI;
    private float defaultFontSize;

    private void Awake()
    {
        m_TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
        defaultFontSize = m_TextMeshProUGUI.fontSize;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_TextMeshProUGUI.fontSize = 52f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_TextMeshProUGUI.fontSize = defaultFontSize;
    }
}
