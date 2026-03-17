using System;
using Unity.Netcode;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerManager : MonoBehaviour
{
    private GameManage gameManage;

    [Header("Player Trackers")]
    private int playerCount = 0;
    private GameObject[] players;
    private static bool[] playersActive;

    public int PlayerCount { get => playerCount; set => playerCount = value; }
    public GameObject[] Players { get => players; set => players = value; }
    public static bool[] PlayersActive { get => playersActive; set => playersActive = value; }

    private void Start()
    {
        gameManage = GetComponent<GameManage>();
        //SetPlayers(2);
    }

    private void SetPlayers(int playerCount)
    {
        //HostClientManage hostClientManage = new HostClientManage();

        //hostClientManage.StartHost();
        //hostClientManage.StartClient();

        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.StartClient();

        PlayersActive = new bool[playerCount];

        Players = GameObject.FindGameObjectsWithTag("Player");

        int ID = 0;
        foreach (GameObject ply in Players)
        {
            ply.GetComponent<PlayerController>().PlayerID = ID;
            ID++;
        }
    }

    public void JoinPlayerHost()
    {
        NetworkManager.Singleton.StartHost();
        setPlayerValues();
    }

    public void JoinPlayerClient()
    {
        NetworkManager.Singleton.StartClient();
        setPlayerValues();
    }

    private void setPlayerValues()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        PlayerCount = Players.Length;
        PlayersActive= new bool[PlayerCount];
        for(int i = 0; i < PlayerCount; i++)
        {
            PlayersActive[i]= true;
        }
    }

    private void setPlayer()
    {

    }


    // Sets a player death
    public void PlayerDeath(int playerID)
    {
        PlayersActive[playerID] = false;

        // Checks status of other players, if no players are alive ends game
        if (!checkActivePlayers())
        {
            gameManage.EndGame(false);
        }
    }

    // Checks the status of all players, returns true if there is a player alive.
    private bool checkActivePlayers()
    {
        foreach (bool player in PlayersActive)
        {
            if (player)
            {
                return true;
            }
        }

        return false;
    }
}
