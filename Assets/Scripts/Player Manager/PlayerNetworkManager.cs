using System.Collections.Generic;
using NUnit.Framework;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerNetworkManager : MonoBehaviour
{
    // Manages abstract player connections
    private static List<ulong> playerIds = new List<ulong>();

    public static List<ulong> PlayerIds { get => playerIds; set => playerIds = value; }

    private void Start()
    {

    }

    public void JoinPlayerHost()
    {
        Debug.Log("Host");
        NetworkManager.Singleton.StartHost();
    }

    public void JoinPlayerClient()
    {
        Debug.Log("Client");
        NetworkManager.Singleton.StartClient();
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

        Debug.Log("Player " + clientID + " has joined");
    }

    private void ClientDisonnect(ulong clientID) {
        Debug.Log("Player " + clientID + " has left");
    }
}
