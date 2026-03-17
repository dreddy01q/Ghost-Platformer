using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : NetworkBehaviour
{
    private EnemyDetection enemyDetection;
    private GameObject currentTargetPlayer;

    private NavMeshAgent agent;
    private Animator ani;
    private Rigidbody rb;
    public NavMeshAgent Agent
    {
        get => agent;
        set => agent = value;
    }
    public Animator Ani
    {
        get => ani;
        set => ani = value;
    }
    public Rigidbody Rb
    {
        get => rb;
        set => rb = value;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        enemyDetection = GetComponent<EnemyDetection>();
    }

    public void MovementUpdate()
    {
        // Has found a player in range of detection
        GameObject targetPlayer = enemyDetection.GetClosestPlayer();
        if (targetPlayer != null) 
        {
            // New Target Player
            if (currentTargetPlayer == null)
            {
                currentTargetPlayer = targetPlayer;
            }

            // Found a closer player
            if (currentTargetPlayer == targetPlayer) 
            {
                float distance=Vector3.Distance(this.transform.position, targetPlayer.transform.position);
                if (enemyDetection.SwitchTargetCheck(distance))
                {
                    currentTargetPlayer = targetPlayer;
                }
            }

            moveTowardsPlayer(currentTargetPlayer);
        }
        else
        {
            // Still chasing a player
            if (currentTargetPlayer != null) {
                if (!enemyDetection.ForgetTargetCheck(agent.remainingDistance))
                {
                    moveTowardsPlayer(currentTargetPlayer);
                    return;
                }
            }

            stopEnemyMovement();
        }
    }


    private void moveTowardsPlayer(GameObject playerTarget)
    {
        // Enable the navmesh agenet
        agent.enabled = true;

        // Sets the enemy movement animation
        Ani.SetFloat("movement", rb.linearVelocity.magnitude);

        // Enemy will run towards the player and stop just in front of them
        Vector3 targetDirection = playerTarget.transform.position - transform.forward;
        agent.SetDestination(targetDirection);
    }

    private void stopEnemyMovement()
    {
        Ani.SetFloat("movement", 0);
        agent.enabled = false;
    }
}
