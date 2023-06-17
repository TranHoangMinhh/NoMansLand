using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{

    [Header("Time Settings")]
    [SerializeField] private bool isCountDown;
    [SerializeField] private float duration;

    [Space(10)]
    [SerializeField] private bool hasLimit;
    [SerializeField] private float timerLimit;

    private TextMeshProUGUI _timerText;
    private float _currentTime;
    private float _minutes;
    private float _seconds;

    // Start is called before the first frame update
    void Start()
    {
        _timerText = GetComponent<TextMeshProUGUI>();
        _currentTime = duration;
    }

    // Update is called once per frame
    void Update()
    {
        _currentTime = isCountDown ? _currentTime -= Time.deltaTime : _currentTime += Time.time;

        if (hasLimit && ((isCountDown && _currentTime <= timerLimit) || (!isCountDown && _currentTime >= timerLimit)))
        {
            _currentTime = timerLimit;
            SetTimerText();
            enabled = false;
        }

        SetTimerText();
    }

    private void SetTimerText()
    {
        _minutes = Mathf.FloorToInt(_currentTime / 60);
        _seconds = Mathf.FloorToInt(_currentTime % 60);

        if (_seconds <= 10)
        {
            _timerText.color = Color.red;
            _timerText.fontStyle = FontStyles.Bold;
        }

        _timerText.text = string.Format("{0:00} : {1:00}", _minutes, _seconds);
    }
}
