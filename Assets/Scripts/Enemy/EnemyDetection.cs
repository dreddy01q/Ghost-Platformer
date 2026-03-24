using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    private PlayerManager playerManager;

    private float detectionRange = 10;
    private float targetSwitchRange = 5f;
    private float targetForgetRange = 5f;

    private void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerManager>();
    }

    // Will find the closest player in the detection range
    // This is used to intialy find a player, change player targets if another gets closer and 
    public GameObject GetClosestPlayer(bool findVisiblePlayer = true)
    {
        GameObject closestPly = null;
        float distance = detectionRange;

        foreach(GameObject player in playerManager.Players)
        {
            float distanceToPly = Vector3.Distance(this.transform.position, player.transform.position);

            // Closest player
            if (distanceToPly <= distance) {
                distance = distanceToPly;
                closestPly = player;
            }
        }
        return closestPly;
    }

    public GameObject GetClosestActivePlayer(bool findVisiblePlayer = true)
    {
        Debug.LogWarning("Player 1 active: "+ playerManager.Players.Count);
        GameObject closestPly = null;
        float distance = detectionRange;


        // Check each active player
        for(int i = 0; i < playerManager.Players.Count; i++)
        {
            if (playerManager.PlayersActive[i])
            {
                GameObject player = playerManager.Players[i];
                float distanceToPly = Vector3.Distance(this.transform.position, player.transform.position);

                // Closest player
                if (distanceToPly <= distance)
                {
                    distance = distanceToPly;
                    closestPly = player;
                    Debug.Log(playerManager.Players[i].name + " is targeted!");
                }
            }
        }

        Debug.Log(closestPly.name + " is the final target!");
        return closestPly;
    }

    // The enemy will only switch targets to another player if they get within a certain target switch Range
    public bool SwitchTargetCheck(float distanceToTarget)
    {
        return distanceToTarget <= targetSwitchRange;
    }

    // The enemy will only switch targets to another player if they get within a certain target switch Range
    public bool ForgetTargetCheck(float distanceToTarget)
    {
        return distanceToTarget >= targetForgetRange;
    }
}
