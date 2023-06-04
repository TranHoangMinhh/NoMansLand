using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    private int lastFrameIndex;
    private float[] frameDeltaTimeArray;

    private TextMeshProUGUI uiText;

    private void Awake()
    {
        uiText = GetComponent<TextMeshProUGUI>();
        frameDeltaTimeArray = new float[50];
    }

    // Update is called once per frame
    void Update()
    {
        frameDeltaTimeArray[lastFrameIndex] = Time.deltaTime;
        lastFrameIndex = (lastFrameIndex + 1) % frameDeltaTimeArray.Length;

        uiText.text = "FPS: " + Mathf.RoundToInt(CalculateFPS()).ToString();
    }

    private float CalculateFPS()
    {
        float total = 0f;
        foreach (float deltaTime in frameDeltaTimeArray)
        {
            total += deltaTime;
        }

        return frameDeltaTimeArray.Length / total;
    }
}
