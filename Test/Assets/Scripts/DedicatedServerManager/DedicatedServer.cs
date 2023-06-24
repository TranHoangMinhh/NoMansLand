using System;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Multiplay;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class DedicatedServer : MonoBehaviour
{
    private const string _internalServerIP = "0.0.0.0";
    private ushort _serverPort = 7777;
    public ushort dPort {get; private set;}
    public string dServerIPAddress {get; private set;}
    private void Start()
    {
        dPort = 7777;
        dServerIPAddress = "127.0.0.1";
        bool server = false;
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if(args[i] == "-dedicatedServer")
            {
                server = true;
            }

            /*
            if(args[i] == "-port" && (i + 1 < args.Length))
            {
                _serverPort = (ushort)int.Parse(args[i+1]);
            }
            */
        
        #if DEDICATED_SERVER
            if(server)
            {
                InitializeDedicatedServer();
                Debug.Log("Initialize server");
                StartServer();
                Debug.Log("Start server done");
                ReadyServer();
                Debug.Log("Server ready to accept players");

            }
        #endif
        }
    }

    private void StartServer()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(_internalServerIP, _serverPort, "0.0.0.0");
        NetworkManager.Singleton.StartServer();
    }

    private async void InitializeDedicatedServer()
    {
        try
        {
            await UnityServices.InitializeAsync();

            MultiplayEventCallbacks multiplayEventCallbacks = new MultiplayEventCallbacks();
            multiplayEventCallbacks.Allocate += OnAllocate;
            multiplayEventCallbacks.Deallocate += OnDeallocate;
            multiplayEventCallbacks.Error += OnError;
            multiplayEventCallbacks.SubscriptionStateChanged += OnSubscriptionStateChanged;
            
            IServerEvents serverEvents = await MultiplayService.Instance.SubscribeToServerEventsAsync(multiplayEventCallbacks);

            var serverConfig = MultiplayService.Instance.ServerConfig;
            Debug.Log($"Server ID[{serverConfig.ServerId}]");
            Debug.Log($"AllocationID[{serverConfig.AllocationId}]");
            Debug.Log($"Server IPv4 address[{serverConfig.IpAddress}]");
            Debug.Log($"Port[{serverConfig.Port}]");
            Debug.Log($"QueryPort[{serverConfig.QueryPort}");
            Debug.Log($"LogDirectory[{serverConfig.ServerLogDirectory}]");

            _serverPort = serverConfig.Port;

            dServerIPAddress = serverConfig.IpAddress;
            dPort = serverConfig.Port;
            if(serverConfig.AllocationId != "")
            {
                OnAllocate(new MultiplayAllocation("", serverConfig.ServerId, serverConfig.AllocationId));
            }


        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }


    private async void ReadyServer()
    {
        await MultiplayService.Instance.ReadyServerForPlayersAsync();
    }

    private void OnAllocate(MultiplayAllocation allocation)
    {

    }

	private void OnDeallocate(MultiplayDeallocation deallocation)
	{

	}

	private void OnError(MultiplayError error)
	{

	}

    private void OnSubscriptionStateChanged(MultiplayServerSubscriptionState state)
	{
		switch (state)
		{
			case MultiplayServerSubscriptionState.Unsubscribed: /* The Server Events subscription has been unsubscribed from. */ break;
			case MultiplayServerSubscriptionState.Synced: /* The Server Events subscription is up to date and active. */ break;
			case MultiplayServerSubscriptionState.Unsynced: /* The Server Events subscription has fallen out of sync, the subscription tries to automatically recover. */ break;
			case MultiplayServerSubscriptionState.Error: /* The Server Events subscription has fallen into an errored state and won't recover automatically. */ break;
			case MultiplayServerSubscriptionState.Subscribing: /* The Server Events subscription is trying to sync. */ break;
		}
	}

}
