using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerDeath : NetworkBehaviour
{
    public GameObject RespawnUI;
    public TextMeshProUGUI RespawnCountDisplay;

    private float respawnTime = 8;
    private float respawnCountdown;

    private bool respawning = false;

    private PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }


    public void SetPlayerDeath()
    {
        if (IsClient)
        {
            playerController.GameManage.PlayerManager.PlayerDeathServerRpc(playerController.PlayerArrayId);
        }
        else
        {
            playerController.GameManage.PlayerManager.PlayerDeath(playerController.PlayerArrayId);
        }
    }

    public void StartRespawn()
    {
        Debug.Log("Called here");
        Debug.Log("I died name is " + gameObject.name + ". My ID is " + playerController.PlayerId + ". My array id is " + playerController.PlayerArrayId);
        if (!respawning && IsOwner)
        {
            respawning = true;
            RespawnUI.SetActive(true);
            respawnCountdown = respawnTime;

            StartCoroutine(RespawnCount());
        }
    }

    [ClientRpc]
    public void StopRespawnClientRpc()
    {
        RespawnUI.SetActive(false);
        StopCoroutine(RespawnCount());
    }

    IEnumerator RespawnCount()
    {
        while (respawnCountdown > 0) 
        {
            respawnCountdown -= Time.deltaTime;

            RespawnCountDisplay.text = "RESPAWNING IN..." + (int)respawnCountdown;
            yield return null;
        }
        respawn();
    }

    private void respawn()
    {
        playerController.RespawnPlayer();
    }
}
