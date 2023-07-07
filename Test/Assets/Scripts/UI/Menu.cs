using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    [SerializeField] private Button startBtn;
    [SerializeField] private Button openSettingsButton;
    [SerializeField] private Button quitGameButton;

    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject chooseModeMenu;


    private void Awake()
    {
        startBtn.onClick.AddListener(() => 
        {
            Hide();
            chooseModeMenu.SetActive(true);
        });
        openSettingsButton.onClick.AddListener(() => {
            Hide();
            settingsMenu.SetActive(true);

        });
        quitGameButton.onClick.AddListener(() => {
            QuitGame();
        });
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
