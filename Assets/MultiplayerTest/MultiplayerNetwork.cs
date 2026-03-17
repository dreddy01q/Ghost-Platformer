using Unity.Netcode;
using UnityEngine;

public class MultiplayerNetwork : MonoBehaviour
{
    public PlayerManager PlayerManager;
    public EnemyManager enemyManager;

    public GameObject enemy;

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
        PlayerManager.JoinPlayerHost();
        updatePlayerReference();
    }

    public void StartClient()
    {
        PlayerManager.JoinPlayerClient();
        updatePlayerReference();
    }

    public void SpawnEnemy()
    {
        
    }

    private void updatePlayerReference()
    {
        Debug.Log("Count: " + PlayerManager.PlayerCount);
        //enemyManager.UpdatePlayerRefrence(PlayerManager.Players[PlayerManager.PlayerCount - 1]);
    }
}
