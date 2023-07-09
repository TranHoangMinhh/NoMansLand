using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.Networking;
using System;
using System.Text;


public class TestNetcodeUI : MonoBehaviour
{
    [SerializeField]private Button serverBtn;
    [SerializeField]private Button clientBtn;
    [SerializeField]private Button hostBtn;
   
    [SerializeField]private InputField IP;
    [SerializeField]private InputField port;
    [SerializeField]private Button ipConnectBtn;

    private string clientIP;
    private ushort connectPort;
    

    private void Awake(){

        clientIP = "127.0.0.1";
        connectPort = 7777;
        string keyId="69cc715d-2c9d-4754-ba02-3f205c5fa2d3";
        string keySecret= "mEvJCSyUey8xZtmZ4F1tcb-ClGpJcqUG";
        byte[] keyByteArray= Encoding.UTF8.GetBytes(keyId + ":" + keySecret);
        string keyBase64 = Convert.ToBase64String(keyByteArray);

        string projectID = "4c107ce0-d87f-431b-aa7b-d87d7722c71c";
        string environmentID = "67194e9e-9f2a-41ed-a78f-0044da910485";
        string url= $"https://services.api.unity.com/multiplay/servers/v1/projects/{projectID}/environments/{environmentID}/servers";
 

         serverBtn.onClick.AddListener(() => {

           NMLGameMultiplayer.Instance.StartServer();

         });
         hostBtn.onClick.AddListener(() => {

            NMLGameMultiplayer.Instance.StartHost();

         });
         clientBtn.onClick.AddListener(() => {
            //getServer(url, keyBase64);
            //Debug.Log("Client authenticated to server done");
            IP.gameObject.SetActive(true);
            port.gameObject.SetActive(true);
            ipConnectBtn.gameObject.SetActive(true);

            ipConnectBtn.onClick.AddListener(() =>
            {
                if(IP.text != "")
                {
                    clientIP = IP.text;
                }
                if(port.text != "")
                {
                    connectPort = (ushort)int.Parse(port.text);
                }
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(clientIP, connectPort);
                if(NMLGameMultiplayer.Instance.StartClient())
                {
                    IP.gameObject.SetActive(false);
                    port.gameObject.SetActive(false);
                    ipConnectBtn.gameObject.SetActive(false);
                }   
            });

        });
   }

    private void getServer(string url, string keyBase64)
    {
        WebRequests.Get(url,
        (UnityWebRequest unityWebRequest) => {
            unityWebRequest.SetRequestHeader("Authorization", "Basic " + keyBase64);
        },
        (string error) => {
            Debug.Log("Error: " + error);
        },
        (string json) => {
            Debug.Log("Success: " + json);
            ListServers listServers = JsonUtility.FromJson<ListServers>("{\"serverList\":" + json + "}");
            foreach (Server server in listServers.serverList) {
                Debug.Log(server.ip + " : " + server.port + " " + server.deleted + " " + server.status);
                if (server.status == ServerStatus.ONLINE.ToString() || server.status == ServerStatus.ALLOCATED.ToString()) {
                    // Server is Online!
                    clientIP = server.ip;
                    connectPort = (ushort)server.port;
                    break;
                }
            }
        }
        );

    }

    private enum ServerStatus {
        AVAILABLE,
        ONLINE,
        ALLOCATED
    }

    [Serializable]
    public class ListServers {
        public Server[] serverList;
    }

    [Serializable]
    public class Server {
        public int buildConfigurationID;
        public string buildConfigurationName;
        public string buildName;
        public bool deleted;
        public string fleetID;
        public string fleetName;
        public string hardwareType;
        public int id;
        public string ip;
        public int locationID;
        public string locationName;
        public int machineID;
        public int port;
        public string status;
    }


}