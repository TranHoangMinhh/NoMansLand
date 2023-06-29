using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticateUI : MonoBehaviour
{
    //! This should be done automatically when running game (if player has input player name before)

    [SerializeField] private TMP_InputField playerNameInputField;
    [SerializeField] private Button authenticateButton;
    [SerializeField] private GameObject mainMenu;

    private void Awake()
    {
        // Adding listener when the button clicked. It will authenticate the user and open main menu
        // Only run once on production environment
        // For development environment, it is mandatory to authenticate each time for having multiple player on same machine
        authenticateButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.Authenticate(playerNameInputField.text);
            Hide();
            openMainMenu();
        });
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void openMainMenu()
    {
        mainMenu.SetActive(true);
    }

}
