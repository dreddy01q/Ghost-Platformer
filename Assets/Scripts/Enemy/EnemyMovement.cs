using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : NetworkBehaviour
{
    private EnemyDetection enemyDetection;
    //private GameObject currentTargetPlayer;

    private NavMeshAgent agent;
    private Animator ani;
    private Rigidbody rb;

    //public GameObject CurrentTargetPlayer { get => currentTargetPlayer; set => currentTargetPlayer = value; }

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
        GameObject playerTarget=enemyDetection.GetTargetPlayer();
        if (playerTarget != null) {
            moveTowardsPlayer(playerTarget);
        }
        else
        {
            stopEnemyMovement();
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
