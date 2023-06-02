using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Healthbar : MonoBehaviour
{
    private Slider _healthSlider;
    private TextMeshProUGUI _healthText;

    // Start is called before the first frame update
    void Start()
    {
        _healthSlider = GetComponent<Slider>();
        _healthText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetMaxHealth(int maxHealth)
    {
        _healthSlider.maxValue = maxHealth;
        _healthSlider.value = maxHealth;
        _healthText.text = _healthSlider.value.ToString();
    }

    public void SetHealth(int health)
    {
        _healthSlider.value = health;
        _healthText.text = _healthSlider.value.ToString();
    }
}
