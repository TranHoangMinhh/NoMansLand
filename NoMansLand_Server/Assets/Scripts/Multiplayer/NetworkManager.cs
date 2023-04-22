using RiptideNetworking;
using RiptideNetworking.Utils;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Multiplay;
using UnityEngine;

public enum ServerToClientId : ushort
{
    sync = 1,
    activeScene,
    playerSpawned,
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

    public Server Server {get; private set;}

    public ushort CurrentTick {get; private set;} = 0;

    [SerializeField] private ushort port;
    [SerializeField] private ushort maxClientCount;
    private void Awake()
    {
        Singleton = this;
        //InitializeUnityAuthentication();
        
    }

    private async void InitializeUnityAuthentication()
    {
        if(UnityServices.State != ServicesInitializationState.Initialized)
        {
            InitializationOptions initializationOptions = new InitializationOptions();
            await UnityServices.InitializeAsync(initializationOptions);
#if !DEDICATED_SERVER
            await  
#endif
        }
    }

    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
        Server = new Server();
        Server.Start(port, maxClientCount);
        Debug.Log($"Listening on port {port}");
        Server.ClientDisconnected += PlayerLeft;
    }

    private void FixedUpdate()
    {
        Server.Tick();
        if(CurrentTick % 200 == 0){
            SendSync();
        }
        CurrentTick++;
    }
    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    private void PlayerLeft(object sender, ClientDisconnectedEventArgs e)
    {
        if(Player.list.TryGetValue(e.Id, out Player player))
        {
            Destroy(player.gameObject);
        }
    }

    private void SendSync()
    {
        Message message = Message.Create(MessageSendMode.unreliable, (ushort)ServerToClientId.sync);
        message.Add(CurrentTick);
        Server.SendToAll(message); 
    }

}