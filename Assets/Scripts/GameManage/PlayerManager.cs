using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using Unity.Netcode;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using Debug = UnityEngine.Debug;

public class PlayerManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public static PlayerManager OwnerPlayerManager;

    private GameManage gameManage;

    [Header("Player Trackers")]
    private static int playerCount = 0;
    private List<GameObject> players;
    private List<bool> playersActive;

    public static int PlayerCount { get => playerCount; set => playerCount = value; }
    public List<GameObject> Players { get => players; set => players = value; }
    public List<bool> PlayersActive { get => playersActive; set => playersActive = value; }

    private void Start()
    {
        gameManage = GetComponent<GameManage>();
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

    private void ClientConnection(ulong clientID)
    {
        PlayerCount++;
        playersActive.Add(true);
    }

    // Sets a player death
    public void PlayerDeath(int playerID)
    {
        PlayersActive[playerID] = false;

        // Checks status of other players, if no players are alive ends game
        if (!checkActivePlayers())
        {
            gameManage.EndGame(false);
        }
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
