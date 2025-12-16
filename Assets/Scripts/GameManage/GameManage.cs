using TMPro;
using UnityEngine;

public class GameManage : MonoBehaviour
{
    public static bool playerWin = false;
    public static bool playerLose = false;

    public GameObject Canvas;
    private CanvasManager CanvasManage;

    private GhostManager GhostManage;

    public GhostManager GhostManager { get => GhostManage; set => GhostManage = value; }
    public CanvasManager CanvasManager { get => CanvasManage; set => CanvasManage = value; }

    private void Awake()
    {
        GhostManager = GetComponent<GhostManager>();
        CanvasManager = Canvas.GetComponent<CanvasManager>();
    }

    public void endGame(bool win)
    {
        if (win)
        {
            CanvasManager.displayWin(GhostManager);
        }
        else
        {
            CanvasManager.displayLose();
        }
    }
}
