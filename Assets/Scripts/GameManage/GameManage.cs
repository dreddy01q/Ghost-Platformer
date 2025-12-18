using TMPro;
using UnityEngine;

public class GameManage : MonoBehaviour
{
    public static bool playerWin = false;
    public static bool playerLose = false;

    public GameObject Canvas;
    private CanvasManager CanvasManage;

    private GhostManager GhostManage;
    private GameSoundEffects GameSound;

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

    public void endGame(bool win)
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
