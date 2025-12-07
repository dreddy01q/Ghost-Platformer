using UnityEngine;

public class GameManage : MonoBehaviour
{
    public static bool playerWin = false;
    public static bool playerLose = false;

    public GameObject Canvas;
    private Animator CanvasAnim;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CanvasAnim = Canvas.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void endGame(bool win)
    {
        if (win)
        {
            displayWin();
        }
        else
        {
            displayLose();
        }
    }

    private void displayWin()
    {
        CanvasAnim.SetTrigger("win");
    }

    private void displayLose()
    {
        CanvasAnim.SetTrigger("lose");
    }
}
