using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PlayerController : NetworkBehaviour
{
    private ulong playerId;
    private int playerArrayId;
    public ulong PlayerId { get => playerId; set => playerId = value; }
    public int PlayerArrayId { get => playerArrayId; set => playerArrayId = value; }


    public GameObject MainCameraObject;
    public Transform MainCam;
    public GameObject PlayerUI;
    public GameObject plyAppereance;

    private Rigidbody rb;
    private Animator ani;


    private GameManage gameManage;
    public GameManage GameManage
    {
        get => gameManage;
        set => gameManage = value;
    }

    private bool isInvisible = false;
    public bool IsInvisible
    {
        get => isInvisible;
        set => isInvisible = value;
    }

    private PlayerHealth playerHealth;
    private PlayerDeath playerDeath;
    private PlayerMovement playerMovement;
    private PlayerJump playerJump;
    private PlayerAttack playerAttack;
    private PlayerInvisibility playerInvisibility;
    public PlayerUI playerUI;

    [ClientRpc]
    public void SetPlayerIdClientRpc(ulong playerID, int playerArrayID)
    {
        SetPlayerId(playerID, playerArrayID);
    }

    public void SetPlayerId(ulong playerID, int playerArrayID)
    {
        this.playerId = playerID;
        this.playerArrayId = playerArrayID;

        Debug.Log("My name is "+gameObject.name+". My ID is " + playerId + ". My array id is " + playerArrayId);
        //Debug.Log(gameManage.PlayerManager.PlayersActive[playerArrayId]);
    }


    public override void OnNetworkSpawn()
    {
        ani = GetComponent<Animator>();

        if (IsOwner)
        {
            MainCameraObject.SetActive(true);
            PlayerUI.SetActive(true);
        }

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        playerMovement= GetComponent<PlayerMovement>();
        playerJump = GetComponent<PlayerJump>();
        playerAttack =GetComponent<PlayerAttack>();
        playerInvisibility = GetComponent<PlayerInvisibility>();

        playerHealth = GetComponent<PlayerHealth>();
        playerDeath = GetComponent<PlayerDeath>();

        GameManage = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManage>();
    }

    public void PlayerDefeated()
    {
        this.enabled = false;
        playerInvisibility.SetPlyApperanceSeverRpc(PlayerArrayId, false);
        rb.linearVelocity = Vector3.zero;

    }

    public void RespawnPlayer()
    {
        plyAppereance.SetActive(true);
        playerHealth.ResetHeatlth();
        playerDeath.RespawnUI.SetActive(false);

        if (IsClient)
        {
            gameManage.PlayerManager.RespawnPlayerServerRpc(playerId, PlayerArrayId);
        }
        else
        {
            gameManage.PlayerManager.RespawnPlayer(playerId, PlayerArrayId);
        }

        //gameManage.PlayerManager.RespawnPlayer(playerId, PlayerArrayId);
    }


    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        getPlyJump();
        getPlyCrouch();
        getPlyMovement();
        getPlyInvisible();
        getPlyScare();

        playerUI.updateHealthDisplay();
    }


    #region Player Input
    
    private void getPlyMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        playerMovement.Movement = new Vector3(horizontal, 0f, vertical);
    }
    
    private void getPlyJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerJump.OnJump(true);
        }
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            playerJump.OnJump(false);
        }
    }

    private void getPlyCrouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            playerMovement.OnCrouch(true);
        }
        
        if (Input.GetKeyUp(KeyCode.C))
        {
            playerMovement.OnCrouch(false);
        }
    }
    
    private void getPlyInvisible()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            playerInvisibility.OnInvisible(true);
        }
        
        if (Input.GetKeyUp(KeyCode.Q))
        {
            playerInvisibility.OnInvisible(false);
        }
    }
    
    private void getPlyScare()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            playerAttack.Scare();
        }
        
        if (Input.GetKeyUp(KeyCode.E))
        {
            //OnScare();
        }
    }

    

    #endregion
    
}
