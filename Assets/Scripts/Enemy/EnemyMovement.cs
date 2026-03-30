using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : NetworkBehaviour
{
    private EnemyDetection enemyDetection;

    private NavMeshAgent agent;
    private Animator ani;
    private Rigidbody rb;

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
        rb.linearVelocity = Vector3.zero;
        gameObject.transform.rotation = new Quaternion(0f, gameObject.transform.rotation.y, 0f, 0f);
        ani.SetFloat("movement", 0);
        agent.enabled = false;
    }
}
