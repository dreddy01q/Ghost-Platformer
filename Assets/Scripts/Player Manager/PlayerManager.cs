using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject PlayerPrefab;

    private GameManage gameManager;

    [Header("Player Trackers")]
    private int playerCount = 0;
    private List<GameObject> players;
    private List<bool> playersActive;

    public int PlayerCount { get => playerCount; set => playerCount = value; }
    public List<GameObject> Players { get => players; set => players = value; }
    public List<bool> PlayersActive { get => playersActive; set => playersActive = value; }




    public static List<ulong> PlayerIds { get => playerIds; set => playerIds = value; }

    private static List<ulong> playerIds;







    private void Start()
    {
        gameManager = GetComponent<GameManage>();

        Players = new List<GameObject>();
        PlayersActive = new List<bool>();   
    }

    public void JoinPlayerHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void JoinPlayerClient()
    {
        NetworkManager.Singleton.StartClient();
        
    }

    private void OnEnable()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += ClientConnection;
    }

    private void OnDisable()
    {
        //NetworkManager.Singleton.OnClientConnectedCallback -= ClientConnection;
    }

    private void ClientConnection(ulong clientID)
    {
        PlayerCount++;
        NetworkObject playerNetworkObject = NetworkManager.Singleton.ConnectedClients[clientID].PlayerObject;

        playerNetworkObject.gameObject.name += PlayerCount;

        Players.Add(playerNetworkObject.gameObject);
        PlayersActive.Add(true);
    }

    // Sets a player death
    public bool PlayerDeath(int playerID)
    {
        PlayersActive[playerID] = false;

        // Checks status of other players, if no players are alive ends game
        if (!checkActivePlayers())
        {
            gameManager.EndGame(false);
            return true;
        }

        return false;
    }

    // Checks the status of all players, returns true if there is a player alive.
    private bool checkActivePlayers()
    {
        foreach (bool player in PlayersActive)
        {
            if (player)
            {
                return true;
            }
        }

        return false;
    }
}
