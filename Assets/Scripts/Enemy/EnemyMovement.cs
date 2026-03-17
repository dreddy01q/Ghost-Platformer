using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
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
    }

    // Implement Detect range


    public void MovementUpdate(bool moveEnemy, GameObject playerTarget)
    {
        if (moveEnemy)
        {
            moveTowardsPlayer(playerTarget);
        }
        else
        {
            // Stop Enemy
            // Implement Enemy Patrol?
            agent.isStopped = true;
        }
    }


    private void moveTowardsPlayer(GameObject playerTarget)
    {
        // Sets the enemy movement animation
        Ani.SetFloat("movement", rb.linearVelocity.magnitude);

        // Enemy will run towards the player and stop just in front of them
        Vector3 targetDirection = playerTarget.transform.position - transform.forward;
        agent.SetDestination(targetDirection);
    }
}
