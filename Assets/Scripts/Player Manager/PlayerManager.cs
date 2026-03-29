using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : NetworkBehaviour
{
    // Manages player objects
    private GameManage gameManager;
    private PlayerManager playerManager;
    private PlayerSpawner playerSpawner;

    private SceneLoader sceneLoader;


    [Header("Player Trackers")]
    private int playerCount = 0;
    private GameObject[] players;
    private NetworkObject[] networkPlayers;
    private bool[] playersActive;
    private int playerArrayIdCount = 0;

    public int PlayerCount { get => playerCount; set => playerCount = value; }
    public GameObject[] Players { get => players; set => players = value; }
    public NetworkObject[] NetworkPlayers { get => networkPlayers; set => networkPlayers = value; }
    public bool[] PlayersActive { get => playersActive; set => playersActive = value; }


    private void Start()
    {
        gameManager = GetComponent<GameManage>();
        playerSpawner=GetComponent<PlayerSpawner>();

        playerManager = GetComponent<PlayerManager>();
        sceneLoader =GetComponent<SceneLoader>();    

        if (IsServer)
        {
            setPlayerArrayValues();
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

    private void setPlayerArrayValues()
    {
        playerCount = PlayerNetworkManager.PlayerIds.Count;
        Players = new GameObject[playerCount];
        NetworkPlayers = new NetworkObject[playerCount];
        PlayersActive = new bool[PlayerCount];
    }

    public void ReturnPlayersToMainMenu()
    {
        foreach(NetworkObject player in networkPlayers)
        {
            try
            {
                player.GetComponent<PlayerController>().GameManage.PlayerManager.ReturnToMenuRpc();
            }
            catch
            {
                Debug.Log("Cannot returbn " );
            }
        }
    }

    [Rpc(SendTo.ClientsAndHost, InvokePermission = RpcInvokePermission.Everyone)]
    public void ReturnToMenuRpc()
    {
        Debug.Log(OwnerClientId + " will return to main menu");
        SceneManager.LoadScene(sceneLoader.MainGameScene);
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void RespawnPlayerServerRpc(ulong playerId, int playerArrayId)
    {
        RespawnPlayer(playerId,playerArrayId);
    }

    private void IntialSpawnPlayer(ulong playerId)
    {
        NetworkObject playerNetworkObject = playerSpawner.SpawnPlayer(playerId, playerArrayIdCount);
        playerSpawnSetup(playerNetworkObject, playerArrayIdCount);

        playerArrayIdCount++;


    }

    public void RespawnPlayer(ulong playerId, int playerArrayId)
    {
        Destroy(Players[playerArrayId].gameObject);

        NetworkObject playerNetworkObject = playerSpawner.RespawnPlayer(playerId, playerArrayId);
        playerSpawnSetup(playerNetworkObject, playerArrayId);
    }

    private void playerSpawnSetup(NetworkObject playerNetworkObject, int playerArrayId)
    {
        Players[playerArrayId] = playerNetworkObject.gameObject;
        NetworkPlayers[playerArrayId] = playerNetworkObject;
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
