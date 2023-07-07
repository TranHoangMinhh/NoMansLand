using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticateUI : MonoBehaviour
{
    //! This should be done automatically when running game (if player has input player name before)

    [SerializeField] private TMP_InputField playerNameInputField;
    // [SerializeField] private Button authenticateBtn;
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject playerProfile;

    // Change background
    [Space(5)]
    [SerializeField] private GameObject currentBackground;
    [SerializeField] private GameObject mainMenuBackground;

    public string playerName { get; private set; }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            playerName = playerNameInputField.text;
            LobbyManager.Instance.Authenticate(playerNameInputField.text);
            
            Hide();
            
            startMenu.SetActive(true);
            playerProfile.SetActive(true);
            PlayerProfile.Instance.SetPlayerName(playerName);

            currentBackground.SetActive(false);
            mainMenuBackground.SetActive(true);
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }


}
