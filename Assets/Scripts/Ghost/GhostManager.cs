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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GhostFound()
    {
        foundCount++;

        if (foundCount == GhostCount) {
            Debug.Log(getGhostCountString() + " ghosts found.");
        }
        else
        {
            Debug.Log("All ghosts freed!");
        }
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
