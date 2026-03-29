using Unity.Netcode;
using UnityEngine;

public class MultiplayerNetworkTest : MonoBehaviour
{
    public void StartHostPlayer()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}
