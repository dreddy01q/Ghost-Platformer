using Unity.Netcode;
using UnityEngine;

public class MultiplayerNetwork : MonoBehaviour
{
    public PlayerManager PlayerManager;

    public void StartHost()
    {
        PlayerManager.JoinPlayerHost();
    }

    public void StartClient()
    {
        PlayerManager.JoinPlayerClient();
    }
}
