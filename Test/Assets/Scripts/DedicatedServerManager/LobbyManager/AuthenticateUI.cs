using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticateUI : MonoBehaviour
{
    //! This should be done automatically when running game (if player has input player name before)

    [SerializeField] private TMP_InputField playerNameInputField;
    [SerializeField] private Button authenticateBtn;
    [SerializeField] private GameObject startMenu;

    public string playerName {get; private set;}
    private void Awake()
    {
        // Adding listener when the button clicked. It will authenticate the user and open main menu
        // Only run once on production environment
        // For development environment, it is mandatory to authenticate each time for having multiple player on same machine
        authenticateBtn.onClick.AddListener(() => {
            if(playerNameInputField.text != "")
            {
                playerName = playerNameInputField.text;
                LobbyManager.Instance.Authenticate(playerNameInputField.text);
                Hide();
                startMenu.SetActive(true);
            }
        });
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }


}
