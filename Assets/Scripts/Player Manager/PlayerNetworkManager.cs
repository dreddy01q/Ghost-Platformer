using System.Collections.Generic;
using System.Linq;
using System.Net;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class PlayerNetworkManager : MonoBehaviour
{
    public static bool isHost = false;

    // Manages abstract player connections
    private static List<ulong> playerIds = new List<ulong>();
    public static List<ulong> PlayerIds { get => playerIds; set => playerIds = value; }

    public void JoinPlayerHost()
    {
        var utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
        utp.SetConnectionData(GetLocalIPv4(), 7777);
        NetworkManager.Singleton.StartHost();
        isHost = true;
    }

    public void JoinPlayerClient(string hostIp, ushort hostPost=7777)
    {
        var utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
        utp.SetConnectionData(hostIp, hostPost);
        bool joined=NetworkManager.Singleton.StartClient();
        Debug.Log("Joined -> " + joined+" "+ utp.ConnectionData.Address);
    }

    public void DisconnectClient()
    {
        NetworkManager.Singleton.DisconnectClient(PlayerInstance.PlayerClientId);
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
        Debug.Log("Client discconeted");
        try
        {
            PlayerManager playerManager = GetComponent<PlayerManager>();
            playerManager.ClientDisconectSeverRpc();
        }
        catch
        {
            Debug.Log("discconeted failed");
        }
    }

    public string GetLocalIPv4()
    {
        return Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(
        f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        .ToString();
    }
}
