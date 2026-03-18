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

        Debug.Log("Cloest Player is " + closestPly.name);

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
