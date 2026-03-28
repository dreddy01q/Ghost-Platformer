using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    private GameObject currentTargetPlayer;
    private PlayerManager playerManager;

    private float detectionRange = 10;
    private float targetSwitchRange = 5f;
    private float targetForgetRange = 15f;

    public GameObject CurrentTargetPlayer { get => currentTargetPlayer; set => currentTargetPlayer = value; }

    //public GameObject CurrentPlayerTarget { get => currentPlayerTarget; set => currentPlayerTarget = value; }

    private void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerManager>();
    }

    public GameObject GetTargetPlayer()
    {
        if (checkCurrentTargetDefeated())
        {
            CurrentTargetPlayer = null;
        }

        GameObject closestTargetPlayer = getClosestPotetialTarget();

        // No player in range and not chasing a player
        if (closestTargetPlayer==null && currentTargetPlayer == null)
        {
            return null;
        }

        // Found a player in range
        if (closestTargetPlayer != null) 
        {
            checkClosestAgainstCurrent(closestTargetPlayer);
        }
        else
        {
            // The current target has fallen out of intial detection range
            if (currentTargetPlayer != null) {
                if (checkForgotTarget(currentTargetPlayer))
                {
                    currentTargetPlayer = null;
                }
            }
        }

        return currentTargetPlayer;
    }

    // Will get the cloest player that is A. Active and B. Visible
    private GameObject getClosestPotetialTarget()
    {
        // Intialy set to null
        GameObject closestPly = null;
        float distance = detectionRange;

        // Check each active player
        for (int i = 0; i < playerManager.Players.Length; i++)
        {
            if (playerManager.PlayersActive[i])
            {
                GameObject player = playerManager.Players[i];
                if (!player.GetComponent<PlayerController>().IsInvisible)
                {
                    float distanceToPly = Vector3.Distance(this.transform.position, player.transform.position);
                    if (distanceToPly <= distance)
                    {
                        distance = distanceToPly;
                        closestPly = player;
                    }
                }
                else
                {
                    Debug.Log("Where are you!");
                }
            }
        }
        if (closestPly != null)
        {
            Debug.Log("IM PURSING " + closestPly.name);
        }

        return closestPly;
    }

    // Checks the new closest target against the current closest target
    private void checkClosestAgainstCurrent(GameObject closestTargetPlayer)
    {
        // There is no current target
        if (currentTargetPlayer == null)
        {
            currentTargetPlayer = closestTargetPlayer;
        }

        // The current target is not the closests detected
        if (currentTargetPlayer != closestTargetPlayer)
        {
            // Checks to see if the target should be switched
            if (attemptSwitchEnemyTarget(closestTargetPlayer))
            {
                currentTargetPlayer = closestTargetPlayer;
            }
        }
    }

    // Checks to see if the closest player target is within a switch target range
    private bool attemptSwitchEnemyTarget(GameObject targetPlayer)
    {
        float distanceToTarget = Vector3.Distance(this.transform.position, targetPlayer.transform.position);
        return distanceToTarget <= targetSwitchRange;
    }

    // Checks to see if the closest player target is within a switch target range
    private bool checkForgotTarget(GameObject targetPlayer)
    {
        float distanceToTarget = Vector3.Distance(this.transform.position, targetPlayer.transform.position);
        return distanceToTarget >= targetForgetRange;
    }

    private bool checkCurrentTargetDefeated()
    {
        if (currentTargetPlayer != null) {
            return currentTargetPlayer.GetComponent<HealthSystem>().OpponentDefeated;
        }
        else
        {
            return false;
        }
    }

}