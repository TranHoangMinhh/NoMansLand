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

            if(args[i] == "-port" && (i + 1 < args.Length))
            {
                _serverPort = (ushort)int.Parse(args[i+1]);
            }

            if(server)
            {
                StartServer();
                StartServerServices();
                ReadyServer();
            }
        }
    }

    private void StartServer()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(_internalServerIP, (ushort)_serverPort);
        NetworkManager.Singleton.StartServer();
    }

    private async void StartServerServices()
    {
        try
        {
            await UnityServices.InitializeAsync();
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
}
