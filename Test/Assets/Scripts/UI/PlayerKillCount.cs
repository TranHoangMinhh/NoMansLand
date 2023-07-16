using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerKillCount : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI killCountText;


    private void Start()
    {
        killCountText.text = "0";
    }

}
