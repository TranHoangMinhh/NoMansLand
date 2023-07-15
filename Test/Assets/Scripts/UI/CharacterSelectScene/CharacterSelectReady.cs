using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterSelectReady : NetworkBehaviour
{

    public static CharacterSelectReady Instance { get; private set; }

    public event EventHandler OnReadyChanged;


    private Dictionary<ulong, bool> playerReadyDictionary;


    private void Awake() {
        Instance = this;

        playerReadyDictionary = new Dictionary<ulong, bool>();
    }

    private async void Start() {
#if !DEDICATED_SERVER
        Debug.Log("DEDICATED_SERVER CHARACTER SELECT");

        Debug.Log("ReadyServerForPlayersAsync");
        await MultiplayService.Instance.ReadyServerForPlayersAsync();

        Camera.main.enabled = false;
#endif
    }


    public void SetPlayerReady() {
        SetPlayerReadyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default) {
        SetPlayerReadyClientRpc(serverRpcParams.Receive.SenderClientId);

        Debug.Log("SetPlayerReadyServerRpc " + serverRpcParams.Receive.SenderClientId);
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;
        int numberOfPlayers = LobbyManager.Instance.GetPlayerNumberFromLobby();
        bool allClientsLockIn = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds) {
            if (!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId]) {
                // This player is NOT ready
                allClientsLockIn = false;
                break;
            }
        }

        if (allClientsLockIn && playerReadyDictionary.Count == numberOfPlayers) {
            //LobbyManager.Instance.DeleteLobby();
            Loader.LoadNetwork(Loader.Scene.GameScene);
        }
    }

    [ClientRpc]
    private void SetPlayerReadyClientRpc(ulong clientId) {
        playerReadyDictionary[clientId] = true;

        OnReadyChanged?.Invoke(this, EventArgs.Empty);
    }


    public bool IsPlayerReady(ulong clientId) {
        return playerReadyDictionary.ContainsKey(clientId) && playerReadyDictionary[clientId];
    }

}
