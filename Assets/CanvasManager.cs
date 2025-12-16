using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public TextMeshProUGUI TextDisplay;
    public TextMeshProUGUI GhostCountDisplay;

    private Animator CanvasAnim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CanvasAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showText(string text)
    {
        TextDisplay.text = text;
        CanvasAnim.SetTrigger("showText");
    }

    public void displayWin(GhostManager GhostManager)
    {
        CanvasAnim.SetTrigger("win");

        if (GhostManager.allGhostsFound())
        {
            GhostCountDisplay.text = "All ghosts found!";
        }
        else
        {
            GhostCountDisplay.text = "You found " + GhostManager.getGhostCountString() + "!";
        }
    }

    public void displayLose()
    {
        CanvasAnim.SetTrigger("lose");
    }
}
