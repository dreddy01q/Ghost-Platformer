using Unity.Netcode;
using UnityEngine.SceneManagement;

public class SceneLoader : NetworkBehaviour
{
    public string MainGameScene;
    public string PlayerLobby;
    public string MainMenu;

    public void ReloadGame()
    {
        jointLoadScene(MainGameScene);
    }

    public void LoadPlayerLobby()
    {
        jointLoadScene(PlayerLobby);
    }

    public void LoadMainMenu()
    {
        jointLoadScene(MainMenu);
    }

    private void jointLoadScene(string scene)
    {
        if (PlayerNetworkManager.isHost)
        {
            NetworkManager.SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
    }
}
