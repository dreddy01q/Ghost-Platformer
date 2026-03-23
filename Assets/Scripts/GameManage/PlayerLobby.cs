using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLobby : NetworkBehaviour
{
    public string MainLevel;

    public void StartGame()
    {
        //SceneManager.LoadScene(MainLevel);
        NetworkManager.SceneManager.LoadScene(MainLevel,LoadSceneMode.Single);
    }

    public void JoinLobby()
    {
        NetworkManager.Singleton.StartHost();
        Debug.Log("Host Joined");
    }
}
