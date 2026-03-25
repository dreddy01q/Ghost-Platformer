using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLobby : NetworkBehaviour
{
    public string MainLevel;

    public TextMeshProUGUI PlayerCountDisplay;
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

    private void Update()
    {
        UpdatePlayerCount();
    }

    public void UpdatePlayerCount()
    {
        if (IsHost)
        {
            PlayerCountDisplay.text = PlayerNetworkManager.PlayerIds.Count + " players joined";
        }
        else
        {
            PlayerCountDisplay.text = "Waiting for host to start...";
        }
    }
}
