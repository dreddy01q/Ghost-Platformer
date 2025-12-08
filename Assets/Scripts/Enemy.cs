using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject player;

    public float detectRange = 10;
    public float moveSpeed = 3;
    
    
    [Header("Attack Settings")]
    [SerializeField] int attackDamage = 1; 
    [SerializeField] float attackRange = 0.5f;
    [SerializeField] float attackCooldown = 3;
    private float attackCooldownCount = 0;

    private Animator ani;

    private Rigidbody rb;
    
    void Start()
    {
        player=GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    public void ManualUpdate()
    {
        enemyMovement();
    }

    void enemyMovement()
    {
        // Checks to see if player is in range
        if (playerInRange())
        {
            // Once the player is in the enemies range, they will move towards them
            moveTowardsPlayer();
            
            // Once chasing the player, will check to see if they can attack
            if (attackCooldownCount <= 0)
            {
                checkAttack();
            }
            else
            {
                attackCooldownCount -= Time.deltaTime;
            }
        }
    }

    /*
    private Vector3 enemyDirection;
    private void getDirection()
    {
        enemyDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * Vector3.forward;
    }
    
    */

    // If the player is in attack range
    private bool playerInRange()
    {
        if(Vector3.Distance(this.transform.position, player.transform.position) < detectRange)
        {
            return true;
        }
        return false;
    }

    private void moveTowardsPlayer()
    {
        agent.SetDestination(player.transform.position);
        ani.SetFloat("movement",rb.linearVelocity.magnitude);
    }
    
    
    
    
    private void checkAttack()
    {
        if(Vector3.Distance(transform.position, player.transform.position) < attackRange)
        {
            enemyAttack();
        }
    }

    private void enemyAttack()
    {
        attackCooldownCount = attackCooldown;
        
        ani.SetTrigger("attack");
        
        RaycastHit hit;
        
        Ray downRay = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(downRay, out hit) && hit.distance <= attackRange) 
        {
            if (hit.collider.tag == "Player")
            {
                hit.collider.GetComponent<HealthSystem>().takeDamage(attackDamage);
            }
            else
            {
                Debug.Log(hit.collider.tag);
            }
            
        }
        
        Debug.DrawRay(transform.position, transform.forward, Color.red,20);
    }
}
