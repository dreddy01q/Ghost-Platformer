using System.Collections.Generic;
using System.Linq;
using System.Net;
using NUnit.Framework;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerNetworkManager : MonoBehaviour
{
    public static bool isHost = false;

    public string HostIPAddress;
    ushort HostPort = 7777;
    public TMP_InputField InputField;

    public string PlayerLobby;
    // Manages abstract player connections
    private static List<ulong> playerIds = new List<ulong>();
    public static List<ulong> PlayerIds { get => playerIds; set => playerIds = value; }

    public void JoinPlayerHost()
    {
        var utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
        utp.SetConnectionData(GetLocalIPv4(), 7777);
        NetworkManager.Singleton.StartHost();
        loadPlayerLobby();
        isHost = true;
        //Debug.Log(utp.ConnectionData.Address);
    }

    public void JoinPlayerClient()
    {
        HostIPAddress = InputField.text;
        var utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
        utp.SetConnectionData(HostIPAddress, 7777);
        bool joined=NetworkManager.Singleton.StartClient();
        Debug.Log("Joined -> " + joined+" "+ utp.ConnectionData.Address);
        loadPlayerLobby();
    }

    private void loadPlayerLobby()
    {
        SceneManager.LoadScene(PlayerLobby);
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
        Debug.Log("Player " + clientID + " has left");
    }

    public string GetLocalIPv4()
    {
        return Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(
        f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        .ToString();
    }
}
