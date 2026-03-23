using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GameManage : MonoBehaviour
{
    public GameObject Canvas;
    private CanvasManager CanvasManage;

    private PlayerManager playerManager;
    private GhostManager GhostManage;
    private GameSoundEffects GameSound;

    public PlayerManager PlayerManager { get => playerManager; set => playerManager = value; }
    public GhostManager GhostManager { get => GhostManage; set => GhostManage = value; }
    public CanvasManager CanvasManager { get => CanvasManage; set => CanvasManage = value; }
    public GameSoundEffects GameSoundEffects { get => GameSound; set => GameSound = value; }

    private void Awake()
    {
        GhostManager = GetComponent<GhostManager>();
        CanvasManager = Canvas.GetComponent<CanvasManager>();
        GameSoundEffects = GetComponent<GameSoundEffects>();
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

    public void EndHostClient()
    {

    }
}
