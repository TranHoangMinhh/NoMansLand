using System;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Multiplay;
using Unity.Services.Authentication;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class DedicatedServer : MonoBehaviour
{
    private const string _internalServerIP = "0.0.0.0";
    private ushort _serverPort = 7777;

    private bool serverStarted = false;
    private void Start()
    {
        bool server = false;
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if(args[i] == "-dedicatedServer")
            {
                server = true;
            }
        #if DEDICATED_SERVER
            if(server)
            {
                InitializeDedicatedServer();
                Debug.Log("Initialize server done");
                if(serverStarted == false){
                    StartServer();
                    Debug.Log("Start server done");
                    ReadyServer();
                    Debug.Log("Server ready to accept players");
                }

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
        if(UnityServices.State != ServicesInitializationState.Initialized)
        {
            try
            {
                var options = new InitializationOptions();
                options.SetProfile("test_profile");
                await UnityServices.InitializeAsync(options);
#if !DEDICATED_SERVER
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
#endif

#if DEDICATED_SERVER
                Debug.Log("Unity service not initialized");

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

                if(serverConfig.AllocationId != "")
                {
                    OnAllocate(new MultiplayAllocation("", serverConfig.ServerId, serverConfig.AllocationId));
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            serverStarted = true;
#endif
        }
        else {
            serverStarted = true;
#if DEDICATED_SERVER
            Debug.Log("Unity service already initialized");
            var serverConfig = MultiplayService.Instance.ServerConfig;
            _serverPort = serverConfig.Port;

            if(serverConfig.AllocationId != "")
            {
                OnAllocate(new MultiplayAllocation("", serverConfig.ServerId, serverConfig.AllocationId));
            }

#endif
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
