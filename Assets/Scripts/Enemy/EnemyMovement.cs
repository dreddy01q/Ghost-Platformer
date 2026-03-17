using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : NetworkBehaviour
{
    public float MovementRange = 10;

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


    public void MovementUpdate(GameObject playerTarget, bool playerInvisible)
    {
        if (Vector3.Distance(this.transform.position, playerTarget.transform.position) <= MovementRange && !playerInvisible) 
        {
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
