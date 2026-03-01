using Unity.Netcode;
using UnityEngine;

public class HostClientManage
{

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}
