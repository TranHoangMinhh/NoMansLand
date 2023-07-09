using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NMLGameManager : NetworkBehaviour
{
    public static NMLGameManager Instance { get; private set; }

    [SerializeField] private Transform playerPrefab;
    private NetworkVariable<State> state = new NetworkVariable<State>(State.WaitingToStart);
    private Dictionary<ulong, bool> playerReadyDictionary;
    private Dictionary<ulong, bool> playerPausedDictionary;

    private enum State {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    private void Awake() {
        Instance = this;

        playerReadyDictionary = new Dictionary<ulong, bool>();
        playerPausedDictionary = new Dictionary<ulong, bool>();
    }
    public override void OnNetworkSpawn() {
        state.OnValueChanged += State_OnValueChanged;

        if (IsServer) {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
        }
    }

    private void State_OnValueChanged(State previousValue, State newValue) {
    }

    private void SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut) {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds) {
            Transform playerTransform = Instantiate(playerPrefab);
            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
        }
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId) {
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default) {
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientsReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds) {
            if (!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId]) {
                // This player is NOT ready
                allClientsReady = false;
                break;
            }
        }

        if (allClientsReady) {
            state.Value = State.CountdownToStart;
        }
    }



}
