using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : NetworkBehaviour
{
    public string MainGameScene;
    public string PlayerLobby;

    public void ReloadGame()
    {
        jointLoadScene(MainGameScene);
    }

    public void LoadPlayerLobby()
    {
        jointLoadScene(PlayerLobby);
    }

    private void jointLoadScene(string scene)
    {
        if (PlayerNetworkManager.isHost)
        {
            NetworkManager.SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
    }
}
