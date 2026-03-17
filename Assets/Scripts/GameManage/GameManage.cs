using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GameManage : MonoBehaviour
{
    private static int playerCount = 0;
    public static int PlayerCount { get => playerCount; set => playerCount = value; }

    private GameObject[] players;


    public static bool playerWin = false;
    public static bool playerLose = false;

    private static bool[] playersActive;

    public GameObject Canvas;
    private CanvasManager CanvasManage;

    private GhostManager GhostManage;
    private GameSoundEffects GameSound;

    public GhostManager GhostManager { get => GhostManage; set => GhostManage = value; }
    public CanvasManager CanvasManager { get => CanvasManage; set => CanvasManage = value; }
    public GameSoundEffects GameSoundEffects { get => GameSound; set => GameSound = value; }
    public static bool[] PlayersActive { get => playersActive; set => playersActive = value; }

    private void Awake()
    {
        GhostManager = GetComponent<GhostManager>();
        CanvasManager = Canvas.GetComponent<CanvasManager>();
        GameSoundEffects = GetComponent<GameSoundEffects>();

        
    }

    private void Start()
    {
        SetPlayers(playerCount);
    }

    private void SetPlayers(int playerCount)
    {
        //HostClientManage hostClientManage = new HostClientManage();

        //hostClientManage.StartHost();
        //hostClientManage.StartClient();

        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.StartClient();

        PlayersActive = new bool[playerCount];

        players = GameObject.FindGameObjectsWithTag("Player");

        int ID = 0;
        foreach (GameObject ply in players)
        {
            ply.GetComponent<PlayerController>().PlayerID = ID;
            ID++;
        }

    }

    public void GhostFound()
    {
        GhostManager.GhostFound();
        string message = GhostManager.getGhostCountString() + " ghosts found.";
        CanvasManager.showText(message);
        GameSoundEffects.PlaySound(GameSoundEffects.SoundType_GhostFound);
    }

    // Sets a player death
    public void PlayerDeath(int playerID)
    {
        PlayersActive[playerID] = false;

        // Checks status of other players
        if (checkActivePlayers())
        {
            EndGame(false);
        }
    }

    // Checks the status of all players, returns true if there is a player alive.
    private bool checkActivePlayers()
    {
        foreach (bool player in PlayersActive) {
            if (player) {
                return true;
            }
        }

        return false;
    }

    public void EndGame(bool win)
    {
        GameSoundEffects.PlaySound(GameSoundEffects.SoundType_Music, "stop");

        if (win)
        {
            CanvasManager.displayWin(GhostManager);
            GameSoundEffects.PlaySound(GameSoundEffects.SoundType_Win);
        }
        else
        {
            CanvasManager.displayLose();
            GameSoundEffects.PlaySound(GameSoundEffects.SoundType_Lose);
        }
    }
}
