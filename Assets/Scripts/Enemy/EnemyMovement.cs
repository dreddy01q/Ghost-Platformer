using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : NetworkBehaviour
{
    private EnemyDetection enemyDetection;
    private GameObject currentTargetPlayer;

    private NavMeshAgent agent;
    private Animator ani;
    private Rigidbody rb;

    public GameObject CurrentTargetPlayer { get => currentTargetPlayer; set => currentTargetPlayer = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        enemyDetection = GetComponent<EnemyDetection>();
    }

    public void MovementUpdate()
    {
        // Tries to get a player that is in range
        //GameObject detectedTarget = enemyDetection.GetClosestPlayer();
        GameObject detectedTarget = enemyDetection.GetClosestActivePlayer();

        // If there is a player in range
        if (detectedTarget != null) 
        {
            checkDetectionTarget(detectedTarget);
            moveTowardsPlayer(currentTargetPlayer);
        }
        else
        {
            // Still chasing a player, but out of range
            if (currentTargetPlayer != null) {
                if (!enemyDetection.ForgetTargetCheck(agent.remainingDistance))
                {
                    moveTowardsPlayer(currentTargetPlayer);
                    return;
                }
            }

            stopEnemyMovement();
        }

        GameObject enemyTarget= enemyDetection.GetEnemyTarget();

    }

    /*
     * Checks if the current detected target is the same as the current target
     */
    private void checkDetectionTarget(GameObject detectedTarget)
    {
        // There is no current target
        if (currentTargetPlayer == null)
        {
            currentTargetPlayer = detectedTarget;
        }

        // The current target is not the closests detected
        if (currentTargetPlayer != detectedTarget)
        {
            attemptSwitchEnemyTarget(detectedTarget);
        }
    }

    /*
     * WIll attempt to switch to a new targte if it is in a certain range
     */
    private void attemptSwitchEnemyTarget(GameObject targetPlayer)
    {
        // Checks if the new target is close enough to switch to
        float distance = Vector3.Distance(this.transform.position, targetPlayer.transform.position);
        if (enemyDetection.SwitchTargetCheck(distance))
        {
            currentTargetPlayer = targetPlayer;
        }
    }

    private void moveTowardsPlayer(GameObject playerTarget)
    {
        // Enable the navmesh agenet
        agent.enabled = true;

        // Sets the enemy movement animation
        ani.SetFloat("movement", rb.linearVelocity.magnitude);

        // Enemy will run towards the player and stop just in front of them
        Vector3 targetDirection = playerTarget.transform.position - transform.forward;
        agent.SetDestination(targetDirection);
    }

    private void stopEnemyMovement()
    {
        ani.SetFloat("movement", 0);
        agent.enabled = false;
    }
}
