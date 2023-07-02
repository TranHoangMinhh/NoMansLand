using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseMode : MonoBehaviour
{

    [SerializeField] private Button singleBtn;
    [SerializeField] private Button multiBtn;

    private void Awake()
    {
        singleBtn.onClick.AddListener(OpenSingleplayerScene);
        multiBtn.onClick.AddListener(OpenMultiplayerScene);
    }

    private void OpenMultiplayerScene()
    {
        Loader.Load(Loader.Scene.LobbyScene);
    }

    private void OpenSingleplayerScene()
    {
        //! Add singleplayer scene
    }
}
