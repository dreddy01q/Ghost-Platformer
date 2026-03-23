using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInstance : NetworkBehaviour
{
    private static PlayerNetworkManager playerNetworkManager;

    private static bool playerHost = false;
    private static ulong playerClientId;

    public static bool PlayerHost { get => playerHost; set => playerHost = value; }
    public static ulong PlayerClientId { get => playerClientId; set => playerClientId = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerNetworkManager = GetComponent<PlayerNetworkManager>();

        if (playerHost)
        {
            Debug.Log("The host will join");
            playerNetworkManager.JoinPlayerHost();
        }
        else
        {
            Debug.Log("The client will join");
            playerNetworkManager.JoinPlayerClient();
        }
    }

    public void EndGame()
    {
        Debug.Log("I am disconnecting a host: " + playerHost);
        if (!playerHost)
        {
            //playerNetworkManager.DisconnectClient();
        }

        SceneManager.LoadSceneAsync("MainMenu");
        Debug.Log("We got here in the end");
    }
}
