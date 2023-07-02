using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderControler : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI valueText;

    private void Start()
    {
        valueText.text = slider.value.ToString();
    }

    public void OnSliderChange(float value)
    {
        valueText.text = value.ToString();
    }
}
