using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Manages player objects

    public NetworkObject PlayerPrefab;
    private GameManage gameManager;
    

    [Header("Player Trackers")]
    private int playerCount = 0;
    private List<GameObject> players;
    private List<bool> playersActive;

    public int PlayerCount { get => playerCount; set => playerCount = value; }
    public List<GameObject> Players { get => players; set => players = value; }
    public List<bool> PlayersActive { get => playersActive; set => playersActive = value; }


    private void Start()
    {
        gameManager = GetComponent<GameManage>();

        playerCount=PlayerNetworkManager.PlayerIds.Count;

        Players = new List<GameObject>();
        PlayersActive = new List<bool>();   

        foreach(ulong playerId in PlayerNetworkManager.PlayerIds)
        {
            Debug.Log("Spawning player " + playerId);
            SpawnPlayer(playerId);
        }
    }

    private void SpawnPlayer(ulong playerId)
    {
        NetworkObject playerNetworkObject=NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(PlayerPrefab, playerId, true, true, false, Vector3.zero);
        Players.Add(playerNetworkObject.gameObject);
        PlayersActive.Add(true);
        Debug.Log("Player spawned player " + playerId);
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
