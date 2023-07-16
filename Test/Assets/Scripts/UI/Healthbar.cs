using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Healthbar : MonoBehaviour
{
    public static Healthbar Instance {  get; private set; }

    [SerializeField] private Image[] healthbarSegmentArray;

    private int _maxHealth;
    private float _amountPerSegment;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        _amountPerSegment = _maxHealth / healthbarSegmentArray.Length;
    }

    public void UpdateHealthBar(float health)
    {
        for (int i = 0; i < healthbarSegmentArray.Length; i++)
        {
            int healthSegmentMin = Mathf.RoundToInt(i * _amountPerSegment);
            int healthSegmentMax = Mathf.RoundToInt((i + 1) * _amountPerSegment);

            if (health <= healthSegmentMin)
            {
                healthbarSegmentArray[i].fillAmount = 0f;
            }
            else
            {
                if (health >= healthSegmentMax)
                {
                    healthbarSegmentArray[i].fillAmount = 1f;
                }
                else
                {
                    float fillAmount = (health - healthSegmentMin) / _amountPerSegment;
                    healthbarSegmentArray[i].fillAmount = fillAmount;
                }
            }
        }
    }

}
