using Unity.Netcode;
using UnityEngine;

public class PlayerNetworkManager : MonoBehaviour
{
    private PlayerManager playerManager;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
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
        NetworkManager.Singleton.OnClientConnectedCallback -= ClientConnection;
    }

    private void ClientConnection(ulong clientID)
    {
        playerManager.PlayerCount++;
        NetworkObject playerNetworkObject = NetworkManager.Singleton.ConnectedClients[clientID].PlayerObject;

        playerNetworkObject.gameObject.name += playerManager.PlayerCount;

        playerManager.Players.Add(playerNetworkObject.gameObject);
        playerManager.PlayersActive.Add(true);
    }
}
