using System.Collections.Generic;
using System.Linq;
using System.Net;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.SceneManagement;

public class PlayerNetworkManager : NetworkBehaviour
{
    public static bool isHost = false;

    // Manages abstract player connections
    private static List<ulong> playerIds = new List<ulong>();
    public static List<ulong> PlayerIds { get => playerIds; set => playerIds = value; }

    public void JoinPlayerHost(bool LANGame)
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().UseWebSockets = true;

        if (LANGame)
        {
            var utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
            utp.SetConnectionData(GetLocalIPv4(), 7777);
        }
        NetworkManager.Singleton.StartHost();
        isHost = true;
    }

    public void JoinPlayerClient(bool LANGame, string hostIp="", ushort hostPost=7777)
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().UseWebSockets = true;

        if (LANGame)
        {
            if (string.IsNullOrEmpty(hostIp))
            {
                return;
            }
            var utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
            utp.SetConnectionData(hostIp, hostPost);
        }
        bool joined=NetworkManager.Singleton.StartClient();
    }

    public void DisconnectClient()
    {
        DisconnectClientRpc(PlayerInstance.PlayerClientId);
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    private void DisconnectClientRpc(ulong playerClientId)
    {
        NetworkManager.Singleton.DisconnectClient(playerClientId);
    }

    private void OnEnable()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += ClientConnection;
        NetworkManager.Singleton.OnClientDisconnectCallback += ClientDisonnect;
    }

    private void OnDisable()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= ClientConnection;
        NetworkManager.Singleton.OnClientDisconnectCallback -= ClientDisonnect;
    }

    private void ClientConnection(ulong clientID)
    {
        PlayerInstance.PlayerClientId = clientID;
        PlayerIds.Add(clientID);
    }

    private void ClientDisonnect(ulong clientID) {
        PlayerManager playerManager = GetComponent<PlayerManager>();
        if (isHost)
        {
            PlayerIds.Clear();
            NetworkManager.Singleton.Shutdown();
        }
        SceneLoader sceneLoader = GetComponent<SceneLoader>();
        SceneManager.LoadScene(sceneLoader.MainMenu);
    }

    public string GetLocalIPv4()
    {
        return Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(
        f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        .ToString();
    }
}
