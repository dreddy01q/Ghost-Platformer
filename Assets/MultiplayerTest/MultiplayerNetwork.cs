using Unity.Netcode;
using UnityEngine;

public class MultiplayerNetwork : MonoBehaviour
{
    public EnemyManager enemyManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        updatePlayerReference();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    private void updatePlayerReference()
    {
        enemyManager.UpdatePlayerRefrence(GameObject.FindGameObjectWithTag("Player"));
    }
}
