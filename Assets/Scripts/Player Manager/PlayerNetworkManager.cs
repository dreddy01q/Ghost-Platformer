using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerNetworkManager : MonoBehaviour
{
    public string HostIPAddress;
    ushort HostPort = 7777;
    public TMP_InputField InputField;

    public string PlayerLobby;
    // Manages abstract player connections
    private static List<ulong> playerIds = new List<ulong>();
    public static List<ulong> PlayerIds { get => playerIds; set => playerIds = value; }

    public void JoinPlayerHost()
    {
        NetworkManager.Singleton.StartHost();
        loadPlayerLobby();
    }

    public void JoinPlayerClient()
    {
        //HostIPAddress = InputField.text;
        //var utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
        //utp.ConnectionData.Address = HostIPAddress;
        //utp.ConnectionData.Port = HostPort;
        NetworkManager.Singleton.StartClient();
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
}
