using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    // Manages player objects

    public NetworkObject PlayerPrefab;
    public GameObject[] PlayerSpawns;
    private GameManage gameManager;
    

    [Header("Player Trackers")]
    private int playerCount = 0;
    private GameObject[] players;
    private bool[] playersActive;
    private int playerArrayId = 0;

    public int PlayerCount { get => playerCount; set => playerCount = value; }
    public GameObject[] Players { get => players; set => players = value; }
    public bool[] PlayersActive { get => playersActive; set => playersActive = value; }


    private void Start()
    {
        gameManager = GetComponent<GameManage>();
        if (IsServer)
        {
            playerCount = PlayerNetworkManager.PlayerIds.Count;
            Players = new GameObject[playerCount];
            PlayersActive = new bool[PlayerCount];
            foreach (ulong playerId in PlayerNetworkManager.PlayerIds)
            {
                Debug.Log("Spawning player " + playerId);
                IntialSpawnPlayer(playerId);
            }
        }
        else
        {
            Debug.Log("Im a client");
        }
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void RespawnPlayerServerRpc(ulong playerId, int playerArrayId)
    {
        RespawnPlayer(playerId,playerArrayId);
    }

    private void IntialSpawnPlayer(ulong playerId)
    {
        SpawnPlayer(playerId, playerArrayId);
        playerArrayId++;
    }

    public void RespawnPlayer(ulong playerId, int playerArrayId)
    {
        Destroy(Players[playerArrayId].gameObject);
        SpawnPlayer(playerId, playerArrayId);
    }

    private void SpawnPlayer(ulong playerId, int playerArrayId)
    {
        Debug.Log("I am a host: " + IsHost + ". I am spawning player " + playerId + " with " + playerArrayId);

        NetworkObject playerNetworkObject = NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(PlayerPrefab, playerId, true, true, false, Vector3.zero);


        playerNetworkObject.name += playerId;
        playerNetworkObject.GetComponent<PlayerController>().SetPlayerId(playerId, playerArrayId);
        //if(playerNetworkObject.is)
        playerNetworkObject.GetComponent<PlayerController>().SetPlayerIdClientRpc(playerId, playerArrayId);

        Players[playerArrayId] = playerNetworkObject.gameObject;
        PlayersActive[playerArrayId] = true;
    }


    [Rpc(SendTo.Server,InvokePermission =RpcInvokePermission.Everyone)]
    public void PlayerDeathServerRpc(int playerID)
    {
        PlayerDeath(playerID);
    }

    // Sets a player death
    public bool PlayerDeath(int playerID)
    {
        PlayersActive[playerID] = false;

        // Checks status of other players, if no players are alive ends game
        if (!checkActivePlayers())
        {
            //gameManager.EndGame(false);
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
