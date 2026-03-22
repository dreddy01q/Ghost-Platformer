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

    public void StartRespawn()
    {
        Debug.Log("Called here");
        if (!respawning && IsOwner)
        {
            respawning = true;
            RespawnUI.SetActive(true);
            respawnCountdown = respawnTime;

            StartCoroutine(RespawnCount());
        }
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
