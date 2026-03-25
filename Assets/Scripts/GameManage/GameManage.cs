using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class GameManage : NetworkBehaviour
{
    public GameObject Canvas;
    private CanvasManager CanvasManage;

    private PlayerManager playerManager;
    private GhostManager GhostManage;
    private GameSoundEffects GameSound;

    private bool endGame = false;

    public PlayerManager PlayerManager { get => playerManager; set => playerManager = value; }
    public GhostManager GhostManager { get => GhostManage; set => GhostManage = value; }
    public CanvasManager CanvasManager { get => CanvasManage; set => CanvasManage = value; }
    public GameSoundEffects GameSoundEffects { get => GameSound; set => GameSound = value; }

    private void Awake()
    {
        GhostManager = GetComponent<GhostManager>();
        CanvasManager = Canvas.GetComponent<CanvasManager>();
        GameSoundEffects = GetComponent<GameSoundEffects>();
        playerManager=GetComponent<PlayerManager>();
    }

    public void GhostFound()
    {
        GhostManager.GhostFound();
        string message = GhostManager.getGhostCountString() + " ghosts found.";
        CanvasManager.showText(message);
        GameSoundEffects.PlaySound(GameSoundEffects.SoundType_GhostFound);
    }

    public void EndGame(bool win)
    {
        endGame = true;
        GameSoundEffects.PlaySound(GameSoundEffects.SoundType_Music, "stop");

        foreach(NetworkObject player in PlayerManager.NetworkPlayers){
            performEndgameRpc(player, win);
        }
    }

    private void performEndgameRpc(NetworkObject player, bool win)
    {
        player.GetComponent<PlayerController>().GameManage.SetPlayerCanvasDisplayClientRpc(win);
        player.GetComponent<PlayerDeath>().StopRespawnClientRpc();
    }

    [ClientRpc]
    public void SetPlayerCanvasDisplayClientRpc(bool win)
    {
        Debug.Log("Time to end this");
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

    public void EndHostClient()
    {

    }
}
