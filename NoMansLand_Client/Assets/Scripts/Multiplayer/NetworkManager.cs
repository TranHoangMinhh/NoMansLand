using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine;
using System;

public enum ServerToClientId : ushort
{
    playerSpawned = 1,
    playerMovement,
}

public enum ClientToServerId : ushort
{
    name = 1,
    input,
}
public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _singleton;

    public static NetworkManager Singleton
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
                Debug.Log($"{nameof(NetworkManager)} instance already exists, destroying duplicate");
                Destroy(value);
            }
        }
    }

    public Client Client {get; private set;}

    [SerializeField] private string ip;
    [SerializeField] private ushort port;
    #[SerializeField] private string host;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
        
        Client = new Client();
        Client.Connected += DidConnect;
        Client.ConnectionFailed += FailedToConnect;
        Client.Disconnected += DidDisconnect;
    }

    private void FixedUpdate()
    {
        Client.Tick();
    }

    private void OnApplicationQuit()
    {
        Client.Disconnect();
    }

    public void Connect()
    {
        Client.Connect($"{ip}:{port}");
        #Client.Connect($"{hostAddress}"); used for connecting to host server
    }

    private void DidConnect(object sender, EventArgs e)
    {
        UIManager.Singleton.SendName();
    }

    private void FailedToConnect(object sender, EventArgs e)
    {
        UIManager.Singleton.BackToMain();
    }

    private void PlayerLefrt(object sender, ClientDisconnectedEventArgs e)
    {
        if(Player.list.TryGetValue(e.Id, out Player player))
        {
            Destroy(player.gameObject);
        }
    }

    private void DidDisconnect(object sender, EventArgs e)
    {
        UIManager.Singleton.BackToMain();
        foreach (Player player in Player.list.Values)
        {
            Destroy(player.gameObject);
        }
    }

}
