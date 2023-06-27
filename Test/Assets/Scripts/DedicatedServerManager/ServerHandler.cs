using UnityEngine;
using Unity.Netcode;
using System;

public class ServerHandler : NetworkBehaviour
{

    public const int MAX_PLAYER_AMOUNT = 8;

    public static ServerHandler Instance { get; private set; }
    private NetworkList<PlayerData> playerDataNetworkList;

    public event EventHandler OnPlayerDataNetworkListChanged;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        playerDataNetworkList = new NetworkList<PlayerData>();
        playerDataNetworkList.OnListChanged += PlayerDataNetworkList_OnListChanged;

    }
    private void Start()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionApprovalCallback;
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Server_OnClientDisconnectCallback;

    }
    private void NetworkManager_Server_OnClientDisconnectCallback(ulong clientId) {
        for (int i = 0; i < playerDataNetworkList.Count; i++) {
            PlayerData playerData = playerDataNetworkList[i];
            if (playerData.clientId == clientId) {
                // Disconnected!
                playerDataNetworkList.RemoveAt(i);
            }
        }
    }

    private void NetworkManager_OnClientConnectedCallback(ulong clientId) {
        Debug.Log("Client Connected " + " " + clientId);
        playerDataNetworkList.Add(new PlayerData {
            clientId = clientId,
        });
    }

    private void NetworkManager_ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse) {

        if (NetworkManager.Singleton.ConnectedClientsIds.Count >= MAX_PLAYER_AMOUNT) {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game is full";
            return;
        }

        connectionApprovalResponse.Approved = true;
    }

    private void PlayerDataNetworkList_OnListChanged(NetworkListEvent<PlayerData> changeEvent) {
        OnPlayerDataNetworkListChanged?.Invoke(this, EventArgs.Empty);
    }

}
