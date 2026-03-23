using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLobby : MonoBehaviour
{
    public string MainLevel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(MainLevel);
    }

    public void JoinLobby()
    {
        NetworkManager.Singleton.StartHost();
        Debug.Log("Host Joined");
    }
}
