using RiptideNetworking;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _singleton;

    public static UIManager Singleton
    {
        get => _singleton;
        private set 
        {
            if(_singleton == null)
            {
                _singleton = value;
            }
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(UIManager)} instance already exists, destroying duplicate");
                Destroy(value);
            }
        }
    }

    [Header("Connect")]
    [SerializeField] private GameObject connectUI;
    [SerializeField] private InputField usernameField;

    [Header("Game")]
    [SerializeField] private GameObject gameUI ;
    [SerializeField] private Image hitmarker;
    private void Awake()
    {
        Singleton = this;
    }

    public void ConnectClicked()
    {
        usernameField.interactable = false;
        connectUI.SetActive(false);
        gameUI.SetActive(true);
        NetworkManager.Singleton.Connect();
    }

    public void BackToMain()
    {
        usernameField.interactable = true;
        connectUI.SetActive(true);
        gameUI.SetActive(false);
    }   

    public void ShowHitmarker()
    {
        hitmarker.enabled = true;
        StartCoroutine(HideHitmarker());
    }

    private IEnumerator HideHitmarker()
    {
        yield return new WaitForSeconds(0.1f);
        hitmarker.enabled = false;
    }

    public void SendName()
    {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.name);
        message.AddString(usernameField.text);
        NetworkManager.Singleton.Client.Send(message);
    }

}
