using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticateUI : MonoBehaviour
{
    //! This should be done automatically when running game (if player has input player name before)

    [SerializeField] private GameObject playerNameInput;
    [SerializeField] private TextMeshProUGUI indicatorText;
    [SerializeField] private GameObject loadingIcon;

    private string _playerName;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (playerNameInput.GetComponentInChildren<TMP_InputField>().text != "") {
                _playerName = playerNameInput.GetComponentInChildren<TMP_InputField>().text;
                LobbyManager.Instance.Authenticate(_playerName);

                //Hide();
                playerNameInput.gameObject.SetActive(false);
                loadingIcon.gameObject.SetActive(true);
                indicatorText.text = "Authenticating Player";

                // PlayerProfile.Instance.SetPlayerName(_playerName);
                LoadingManager.Instance.LoadScene(LoadingManager.Scenes.MainMenuScene);
            }
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }


}
