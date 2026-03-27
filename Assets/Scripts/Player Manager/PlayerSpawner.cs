using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public NetworkObject PlayerPrefab;
    public GameObject[] PlayerIntialSpawnPoint;
    public GameObject PlayerRespawnPoint;

    public NetworkObject SpawnPlayer(ulong playerId, int playerArrayId)
    {
        return SpawnPlayer(playerId, playerArrayId, PlayerIntialSpawnPoint[playerArrayId].transform.position);
    }

    public NetworkObject RespawnPlayer(ulong playerId, int playerArrayId)
    {
        return SpawnPlayer(playerId, playerArrayId, PlayerRespawnPoint.transform.position);
    }

    private NetworkObject SpawnPlayer(ulong playerId, int playerArrayId, Vector3 playerSpawn)
    {
        NetworkObject playerNetworkObject = NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(PlayerPrefab, playerId, true, true, false, playerSpawn);

        playerNetworkObject.name += playerId;
        playerNetworkObject.GetComponent<PlayerController>().SetPlayerId(playerId, playerArrayId);
        playerNetworkObject.GetComponent<PlayerController>().SetPlayerIdClientRpc(playerId, playerArrayId);

        return playerNetworkObject;
    }
}
