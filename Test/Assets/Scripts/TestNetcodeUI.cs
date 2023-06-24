using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Multiplay;


public class TestNetcodeUI : MonoBehaviour
{
    [SerializeField]private Button serverBtn;
    [SerializeField]private Button clientBtn;
    [SerializeField]private Button hostBtn;

   [SerializeField]private DedicatedServer dServer;
    private string ipv4Address;
    private ushort serverPort;

    private void Awake(){
#if DEDICATED_SERVER
         Debug.Log("Testing dedicated server");
#endif


        serverBtn.onClick.AddListener(() => {
           NetworkManager.Singleton.StartServer();
        });
        hostBtn.onClick.AddListener(() => {
           NetworkManager.Singleton.StartHost();
        });
        clientBtn.onClick.AddListener(() => {
            ipv4Address = dServer.dServerIPAddress;
            serverPort = dServer.dPort;
           NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ipv4Address, serverPort);
           Debug.Log(ipv4Address + ":" + serverPort);
           NetworkManager.Singleton.StartClient();
        });

   }
}