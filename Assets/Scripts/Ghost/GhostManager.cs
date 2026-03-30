using UnityEngine;

public class GhostManager : MonoBehaviour
{

    public GameObject[] ghosts;

    private int ghostCount;

    private int foundCount;

    public int GhostCount { get => ghostCount; set => ghostCount = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GhostCount = ghosts.Length;
    }

    public void GhostFound()
    {
        foundCount++;
    }

    public string getGhostCountString()
    {
        return foundCount + "/" + GhostCount;
    }

    public bool allGhostsFound()
    {
        if(foundCount== GhostCount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
