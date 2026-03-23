using UnityEngine;

public class PlayerInstance : MonoBehaviour
{
    private static PlayerManager playerManager;

    private static bool playerHost = false;

    public static bool PlayerHost { get => playerHost; set => playerHost = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerManager=GetComponent<PlayerManager>();

        if (playerHost)
        {
            Debug.Log("The host will join");
            playerManager.JoinPlayerHost();
        }
        else
        {
            Debug.Log("The client will join");
            playerManager.JoinPlayerClient();
        }
    }
}
